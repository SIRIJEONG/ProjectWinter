using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_PowerStationGridScript : MonoBehaviour
{
    [SerializeField]
    private GameObject slot;

    private GameObject slotClone;
    private Transform topParentTrans;

    // ���ϴ� ������ ������ ��� ������ �˷��� ����
    private int makeSlotCount;  // 1 ~ 2 �� ����

    private short repetitionMakeSlotCount = 1;  // MakeSlot �Լ��� ����Լ��� ����ϱ� ���� ���� ����
    

    private void Awake()
    {

        //Debug.Log(makeSlotCount);
    }
    private void Start()
    {
        SerchTopParentTrans();
        makeSlotCount = Random.Range(1, 3);  // 1 ~ 2 ������ ���ڸ� ����
        MakeSlot(); // ������ �����ϰ� ���� �ڽĿ�����Ʈ�� �ִ� �Լ�
    }

    private void SerchTopParentTrans() // �ֻ��� �θ� ������Ʈ ã�� ����
    {
        topParentTrans = transform;

        while (topParentTrans.parent != null)
        {
            topParentTrans = topParentTrans.parent;
        }
    }

    private void MakeSlot() // ������ �����ϰ� ���� �ڽĿ�����Ʈ�� �ִ� �Լ�
    {
        if (topParentTrans.CompareTag("PowerStation"))  // �������϶� Instance Slot
        {
            slotClone = Instantiate(slot);
            slotClone.transform.SetParent(this.transform);

            if (repetitionMakeSlotCount < makeSlotCount)
            {
                repetitionMakeSlotCount++;
                MakeSlot();
            }
            else { /*PASS*/ }
        }        
        else if(topParentTrans.CompareTag("HeliPad"))   // �︮�е��϶� Instance Slot
        {
            slotClone = Instantiate(slot);
            slotClone.transform.SetParent(this.transform);
        }
        else { /*PASS*/ }

    }   // MakeSlot()

}   //NameSpace
