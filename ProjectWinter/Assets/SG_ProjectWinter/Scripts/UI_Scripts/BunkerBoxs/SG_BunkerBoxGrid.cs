using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SG_BunkerBoxGrid : MonoBehaviourPun
{
    [SerializeField]
    private GameObject slot;

    private GameObject slotClone;

    // ���ϴ� ������ ������ ��� ������ �˷��� ����
    private int makeSlotCount;  // 1 ~ 2 �� ����

    private short repetitionMakeSlotCount = 1;  // MakeSlot �Լ��� ����Լ��� ����ϱ� ���� ���� ����


    private void Start()
    {
        MakeSlot(); // ������ �����ϰ� ���� �ڽĿ�����Ʈ�� �ִ� �Լ�
    }


    // Photon���� �ؾ��ҵ�
    private void MakeSlot() // ������ �����ϰ� ���� �ڽĿ�����Ʈ�� �ִ� �Լ�
    {
        makeSlotCount = 4;


        slotClone = Instantiate(slot);
        slotClone.transform.SetParent(this.transform);

        if (repetitionMakeSlotCount < makeSlotCount)
        {
            repetitionMakeSlotCount++;
            MakeSlot();
        }
        else { /*PASS*/ }

    }   // MakeSlot()
}
