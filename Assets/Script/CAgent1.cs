using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class CAgent1 : MonoBehaviour
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


        //�ൿƮ���� ����
        //level 2
        ActionNode tANMove = new ActionNode(DoMove);
        
        List<Node> tLevel_2 = new List<Node>();
        tLevel_2.Add(tANMove);
        

        //Level 1
        mRootNode = new Sequence(tLevel_2);


    }

    // Update is called once per frame
    void Update()
    {
        mRootNode.Evaluate();
    }


    



    //���� ������ �ൿ
    NodeStates DoMove()
    {
        Debug.Log("DoMove");

        //���������� ������� �̵��ϴ� �Լ�
        this.transform.position = Vector3.MoveTowards(this.transform.position, mTargetPosition, mSpeed * Time.deltaTime);

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

    void DrawGizmo()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(mTargetPosition, Vector3.one * 0.5f);
    }
}
