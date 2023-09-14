using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class SG_ItemSwapManager : MonoBehaviour
{
    // 아이템을 스왑할때 이 스크립트를 통해서 서로 배열을 가져와서 바꿔줄 거임

    // 플레이어 슬롯 번호는 10을 더해서 곂치는 부분이 없도록 하였음 10 11 12 13 이런번호로 나올거임

    // 아래 함수는 매개변수를 플레이어와 창고로 받지않고 주는인벤토리 , 받는 인벤토리 나누어서 받아야할거같음

    // 플레이어 인벤토리 10 ~ 13
    // 산장창고 인벤토리 100 ~ 115

    // 받는 매개변수 accept , 주는 매개면수 give

    // 아래 함수에 필요한 매개변수 주는인벤 , 주는 인벤의 Slot 주소,받는인벤 ,받는인벤의 Slot 주소, 아이템 정보 , 아이템 갯수

    // 주소를 이용해서 for문으로 서로 주소를 찾고 주는얘의 인벤을 다빼고 받는얘인벤을 채워주기만 하면 될수도 있을거같음

    // 매개변수 정보 : 주는얘 주소, 받는얘주소 , 플레이어 슬롯, 인벤토리 슬롯
    // 총 5개

    // 아래 두개는 인벤토리 선언
    SG_Inventory playerInven;
    SG_WareHouseInventory wareHouseInven;

    SG_ItemSlot plyaerItemSlotClass;
    SG_WareHouseItemSlot wareHouseItemSlot;

    // 옮길 아이템 정보
    SG_Item moveItem;

    // 인벤토리 찾을때 임시로 사용할 변수 선언
    GameObject tempObj001;
    GameObject tempObj002;

    // 임시 아이템 갯수 저장할 변수
    int tempItemCount;


    public void ItemSwap(int _GiveSlotCount, int _AcceptSlotCount,
                         SG_ItemSlot _playerSlots, SG_WareHouseItemSlot _wereHouseSlots)
    {

        //Debug.LogFormat("Player매개변수slots이 정확한 지정좌표를 가지고 있나? -> {0}", _playerSlots.slotCount);
        //Debug.LogFormat("창고 매개변수slots이 정확한 지정좌표를 가지고 있나? -> {0}", _wereHouseSlots.wareHouseSlotCount);

        #region _GiveSlotCount가 플레이어일 경우
        if (_GiveSlotCount > 1 && _GiveSlotCount < 20) // 플레이어 -> 인벤토리 이동시
        {
            // 이번엔 부모의 부모의 부모 3번 찾아야함
            //1 -> Grid
            tempObj001 = _playerSlots.transform.parent.GetComponent<GameObject>();
            //2 -> InventoryImg
            tempObj002 = tempObj001.transform.parent.GetComponent<GameObject>();
            //3 -> Inventory
            playerInven = tempObj002.transform.parent.GetComponent<SG_Inventory>();
            // 사용후 비워주기
            tempObj001 = null;
            tempObj002 = null;

            for (int i = 0; i < playerInven.slots.Length; i++)    //슬롯들을 싹 뒤지면서 주는 Slot의 고유번호 찾기
            {
                if (playerInven.slots[i].slotCount == _GiveSlotCount) // 고유번호를 찾았을때 들어갈 조건문
                {
                    // 아이템 정보를 임시로 집어넣어줌
                    moveItem = playerInven.slots[i].item;
                    // 아이템 갯수 를 매개 변수로 안받아도 될듯?
                    tempItemCount = playerInven.slots[i].itemCount;
                    playerInven.slots[i].ClearSlot();
                    Debug.Log("Swap의 플레이어 For문 들어오나?");
                    Debug.LogFormat("moveItem -> {0} tempItemCount -> {1}", moveItem, tempItemCount);
                    break;
                }
            }
        }
        #endregion _GiveSlotCount가 플레이어일 경우



        else if (_GiveSlotCount >= 100) // 인벤토리 -> 플레이어 이동시
        {

        }

        #region _AcceptSlotCount가 산장창고일경우
        if(_AcceptSlotCount >= 100 && _AcceptSlotCount <= 120)
        {
            // 창고가 받을 Inventory Slots[] 를 찾아오기 위한 코드
            tempObj001 = _wereHouseSlots.transform.parent.GetComponent<GameObject>();
            tempObj002 = tempObj001.transform.parent.GetComponent<GameObject>();
            wareHouseInven = tempObj002.transform.parent.GetComponent<SG_WareHouseInventory>();
            // 임시 선언 Obj 사용후 null값 삽입
            tempObj001 = null;
            tempObj002 = null;

            for(int i =0; i < wareHouseInven.slots.Length; i++)
            {
                if(_AcceptSlotCount == wareHouseInven.slots[i].wareHouseSlotCount)
                {
                    wareHouseItemSlot = wareHouseInven.slots[i].GetComponent<SG_WareHouseItemSlot>();
                    wareHouseItemSlot.item = moveItem;
                    wareHouseItemSlot.itemCount = tempItemCount;
                    Debug.LogFormat("창고에 아이템이 Null? -> {0}", wareHouseItemSlot.item == null);
                    Debug.LogFormat("들어간 아이템 -> {0}", moveItem.name);

                    break;
                }
            }
        }
        #endregion _AcceptSlotCount가 산장창고일경우

    }   // ItemSwap()

    private void SerchGiveSlot(int _GiveSlotCount,
        SG_ItemSlot playerSlots, SG_WareHouseItemSlot _wereHouseSlots)
    {
        if (10 <= _GiveSlotCount && _GiveSlotCount < 100)
        {

        }
    }



}   //NameSpace
