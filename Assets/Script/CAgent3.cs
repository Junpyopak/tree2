using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CAgent3 : MonoBehaviour
{
    [SerializeField]
    Vector3 mTargetPosition = Vector3.zero;

    //�ӷ�
    float mSpeed = 1f;

    //�ൿƮ�� �ֻ�� ��Ʈ���
    Sequence mRootNode = null;



    // Start is called before the first frame update
    void Start()
    {
        //���¸� �����ϴ� �κ��� Ʈ�������� �ڵ����·� ����ȭ�ȴ�.
        //��.'������ ����ȭ'�� �Ͼ��.
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
    //���� ������ �ൿ
    NodeStates DoMove()
    {
        Debug.Log("DoMove");

        //���������� ������� �̵��ϴ� �Լ�
        this.transform.position = Vector3.MoveTowards(this.transform.position, mTargetPosition, mSpeed * Time.deltaTime);

        return NodeStates.SUCCESS;


    }
}
