using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SG_BunkerBoxItemInIt : MonoBehaviourPun
{
    public SG_Item[] itemList;    // 줄아이템 List

    public GameObject itemImageObj;     // Instance 해줄 아이템 이미지
    private GameObject itemImageObjClone;

    private Image itemImage;    // 아이템 Image를 Instace한뒤에 이 변수에 사용할수 있도록 넣어줄거임

    private SG_ItemSlot itemSlotClass;  //  슬롯이 가지고 있는 아이템 클래스 sprite,item 넣어줄거임

    private int giveItemCount;  // 아이템 갯수 몇개 줄지 랜덤으로 정해질 변수

    private bool firstOpen = false;     // 처음 열때에만 적용되도록 할 bool 변수
    private int tempItemListIndex;      // 랜덤하게 선택한 배열의 Index 변수

    void Start()
    {
        StartInIt();
    }


    private void StartInIt()    // Start함수에서 넣어줄 변수들
    {
        if(firstOpen == false)
        {
            firstOpen = true;
            itemSlotClass = GetComponent<SG_ItemSlot>();

            RandomGiveItemCount();      // 줄 아이템 갯수 랜덤하게 정해주는 함수
            ItemImageInIt();            // ItemImage Prefab을 Instance 해서 자식으로 설정해주는 함수
            SlotItemInIt();             // 랜덤하게 나온 아이템과 아이템 갯수를 넣어주는 함수
            itemSlotClass.MoveItemSet();

        }

    }

    // 줄 아이템 갯수 랜덤하게 정해주는 함수
    private void RandomGiveItemCount()
    {
        giveItemCount = Random.Range(1, 4); //1 ~ 3 이라는 수가 정해짐
        tempItemListIndex = Random.Range(0, 10);    // 0 ~ 9 랜덤한 수 정해짐 
    }

    // ItemImage Prefab을 Instance 해서 자식으로 설정해주는 함수
    private void ItemImageInIt()
    {
        // 이미지 가져오는것이 자식 오브젝트에 Image를 낳아서 거기다 넣는 식이기 떄문에 원래 있는 prefab 에서 조금 수정해서 
        // 얘만의 ItemImage를 instance해서 GetParent시키고 그거를 땡겨와서 출력해야할거같음 
        // ItemSlotClass.itemImage = 아이템 슬롯의 테두리임

        itemImageObjClone = Instantiate(itemImageObj);
        itemImageObjClone.transform.SetParent(this.transform);



    }   // ItemImageInIt()

    // 아이템 슬롯에 아이템 정보와 아이템 카운트 넣어줄 함수
    private void SlotItemInIt()
    {
        //Debug.LogFormat("RandomIndet -> {0}",tempItemListIndex);
        //Debug.LogFormat("Item 정보 -> {0}", itemList[tempItemListIndex]);
        itemSlotClass.item = itemList[tempItemListIndex];
        itemSlotClass.itemCount = giveItemCount;
    }

}
