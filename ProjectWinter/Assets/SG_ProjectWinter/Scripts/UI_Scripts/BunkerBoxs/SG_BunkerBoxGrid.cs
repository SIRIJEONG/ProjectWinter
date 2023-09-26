using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SG_BunkerBoxGrid : MonoBehaviourPun
{
    [SerializeField]
    private GameObject slot;

    private GameObject slotClone;

    // 원하는 아이템 슬롯을 몇개를 만들지 알려줄 변수
    private int makeSlotCount;  // 1 ~ 2 들어갈 예정

    private short repetitionMakeSlotCount = 1;  // MakeSlot 함수를 재귀함수로 사용하기 위해 변수 선언


    private void Start()
    {
        MakeSlot(); // 슬롯을 랜덤하게 만들어서 자식오브젝트로 넣는 함수
    }


    // Photon으로 해야할듯
    private void MakeSlot() // 슬롯을 랜덤하게 만들어서 자식오브젝트로 넣는 함수
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
