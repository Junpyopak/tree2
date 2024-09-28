using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CAgent4 : MonoBehaviour
{
    [SerializeField]
    Vector3 mTargetPosition = Vector3.zero;

    //속력
    float mSpeed = 1f;

    //행동트리 최상단 루트노드
    Sequence mRootNode = null;



    // Start is called before the first frame update
    void Start()
    {
        //상태를 결정하는 부분이 트리구조의 코드형태로 국지화된다.
        //즉.'변경의 국지화'가 일어난다.
        //level4
        ActionNode tANMove = new ActionNode(DoMove);
        ActionNode tANDetect = new ActionNode(DoDetect);

        //level3
        //level3_0

        ActionNode tANisArrive = new ActionNode(DoisArrive);
        Inverter tNot = new Inverter(tANMove);
        List<Node> tLevel_3_0 = new List<Node>();
        tLevel_3_0.Add(tANisArrive);
        tLevel_3_0.Add(tNot);
        //level3_1
        ActionNode tANisDetect = new ActionNode(DoisDetect);
        Inverter tNotDetect = new Inverter(tANDetect);
        List<Node> tLevel_3_1 = new List<Node>();
        tLevel_3_1.Add(tANisDetect);
        tLevel_3_1.Add(tNotDetect);

        //level 2
        Selector tSelectArrived = new Selector(tLevel_3_0);
        Selector tSelectDetect = new Selector(tLevel_3_1);
        ActionNode tANAttack = new ActionNode(DoAttack);
        List<Node> tLevel_2 = new List<Node>();
        tLevel_2.Add(tSelectArrived);
        tLevel_2.Add(tSelectDetect);//ryu
        tLevel_2.Add(tANAttack);
        //Level 1
        mRootNode = new Sequence(tLevel_2);



    }

    // Update is called once per frame
    void Update()
    {
        mRootNode.Evaluate();
    }

    NodeStates DoisArrive()
    {
        if (Vector3.Distance(this.transform.position, mTargetPosition) <= 0.01f)
        {
            Debug.Log("<color='red'>Move complite!!</color>");

            return NodeStates.SUCCESS;
        }
        else
        {
            return NodeStates.FAILURE;
        }
    }


    NodeStates DoAttack()
    {

        Debug.Log("<color='Blue'>DoAttack</color>");

        return NodeStates.SUCCESS;

    }

    NodeStates DoDetect()
    {

        Debug.Log("<color='gray'>DoDetect</color>");

        return NodeStates.SUCCESS;

    }
    NodeStates DoisDetect()
    {
        GameObject tOpposite = GameObject.FindGameObjectWithTag("tagOpposite");

        if(tOpposite == null)
        {
            Debug.Log("null");
            return NodeStates.FAILURE;
        }

        if (Vector3.Distance(this.transform.position,tOpposite.transform.position) <= 2f)
        {
            Debug.Log("<size='15'><color='yellow'>Detect complite!!</color></size>");

            return NodeStates.SUCCESS;
        }
        else
        {
            Debug.Log("else");
            return NodeStates.FAILURE;
        }
    }

    //실제 수행할 행동
    NodeStates DoMove()
    {
        Debug.Log("DoMove");

        //선형보간을 기반으로 이동하는 함수
        this.transform.position = Vector3.MoveTowards(this.transform.position, mTargetPosition, mSpeed * Time.deltaTime);

        return NodeStates.SUCCESS;


    }
    void OnDrawGizmos()
    {
        //목적지점 확인용
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(mTargetPosition, Vector3.one * 0.5f);

        //탐지지점확인용
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(this.transform.position, 2f);
    }
}
