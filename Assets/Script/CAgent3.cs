using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CAgent3 : MonoBehaviour
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

        //level3
        ActionNode tANisArrive = new ActionNode(DoisArrive);
        Inverter tNot = new Inverter(tANMove);
        List<Node> tLevel_3 = new List<Node>();
        tLevel_3.Add(tANisArrive);
        tLevel_3.Add(tNot);
        

        //level 2
        Selector tSelectArrived = new Selector(tLevel_3);
        ActionNode tANAttack = new ActionNode(DoAttack);
        List<Node> tLevel_2 = new List<Node>();
        tLevel_2.Add(tSelectArrived);
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
    //실제 수행할 행동
    NodeStates DoMove()
    {
        Debug.Log("DoMove");

        //선형보간을 기반으로 이동하는 함수
        this.transform.position = Vector3.MoveTowards(this.transform.position, mTargetPosition, mSpeed * Time.deltaTime);

        return NodeStates.SUCCESS;


    }
}
