using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_PowerStationGridScript : MonoBehaviour
{
    [SerializeField]
    private GameObject slot;

    private GameObject slotClone;

    // ���ϴ� ������ ������ ��� ������ �˷��� ����
    private int makeSlotCount;  // 1 ~ 2 �� ����

    private short repetitionMakeSlotCount = 1;  // MakeSlot �Լ��� ����Լ��� ����ϱ� ���� ���� ����

    private void Awake()
    {       
        makeSlotCount = Random.Range(1, 3);  // 1 ~ 2 ������ ���ڸ� ����
        MakeSlot(); // ������ �����ϰ� ���� �ڽĿ�����Ʈ�� �ִ� �Լ�
        //Debug.Log(makeSlotCount);
    }

    void Start()
    {

        
    }
    
    void Update()
    {
        
    }


    private void MakeSlot() // ������ �����ϰ� ���� �ڽĿ�����Ʈ�� �ִ� �Լ�
    {
        
        slotClone = Instantiate(slot);
        slotClone.transform.SetParent(this.transform);

        if (repetitionMakeSlotCount < makeSlotCount)
        {
            repetitionMakeSlotCount++;
            MakeSlot();
        }
        else { /*PASS*/ }
    }   // MakeSlot()

}   //NameSpace
