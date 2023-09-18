using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class SG_ItemSwapManager : MonoBehaviour
{
    // 플레이어 슬롯 번호는 10을 더해서 곂치는 부분이 없도록 하였음 10 11 12 13 이런번호로 나올거임

    // 아래 함수는 매개변수를 플레이어와 창고로 받지않고 주는인벤토리 , 받는 인벤토리 나누어서 받아야할거같음

    // 플레이어 인벤토리 10 ~ 13
    // 산장창고 인벤토리 100 ~ 115

    // 받는 매개변수 accept , 주는 매개면수 give

    // 아이템을 줄떄에 갯수 검사하고 임시 변수에 담아두던지 넘겨주지 말던지 해야할거같음


    // 매개변수 정보 : 주는얘 주소, 받는얘주소 , 주는 아이템슬롯, 받는 아이템슬롯

    //SG_ItemSlot giveSlotClass;  // 아이템을 줄 슬롯의 클래스
    //SG_ItemSlot accepSlotClass; // 아이템을 받을 슬롯의 클래스


    private SG_Inventory giveInvenClass;    // 아이템을 줄 인벤토리 클래스
    private SG_Inventory acceptInvenClass;   // 아이템을 받을 인벤토리 클래스

    //트랜스폼으로 받아야함 GameObject로 받으니 Grid에서 참조불가
    private Transform tempTrans001;
    private Transform tempTrans002;
    private SG_ItemDragScript giveItemDragScript; // 3개 이상일때 원래 아이템은 있어야하고 슬롯에 맞게 가야하기 떄문에 필요

    private SG_Item moveItem;   // 옮길 아이템 정보

    private int tempItemCount;  // 임시 아이템 갯수 저장할 변수

    private byte giveSlotCount; // 불필요한 for문을 줄이기 위해 처음 슬롯 찾을때 그 슬롯의 좌표를 저장할 byte 변수
    private byte accepSlotCount;

    private bool isPassExamine = true;  // 아이템 검사하고 검사 조건에 들어가면 false 로바꿔줄거임

    public void ItemSwap(SG_ItemDragScript _itemDragScript, int _GiveSlotCount, int _AcceptSlotCount,
                         SG_ItemSlot _giveSlots, SG_ItemSlot _accepSlots)
    {
        giveItemDragScript = _itemDragScript;
        //Debug.Log("Swap 함수는 들어오기는 하는가?");
        SearchGiveSlot(_GiveSlotCount, _giveSlots);
        SerchAccepSlots(_AcceptSlotCount, _accepSlots);
        GiveExchangeAfter();
        // 아이템최대 갯수를 초과했다면 초과한만큼 돌려주는 함수
        AcceptExchangeAfter();

    }   // ItemSwap()


    // 주는 슬롯을 찾아서 아이템을 변수에 저장하는 함수
    public void SearchGiveSlot(int _GiveSlotCount, SG_ItemSlot _giveSlots)
    {

        #region _GiveSlotCount가 플레이어일 경우
        if (_GiveSlotCount > 1 && _GiveSlotCount < 20) // 플레이어가 주는 슬롯일때에
        {
            // 이번엔 부모의 부모의 부모 3번 찾아야함 
            //1 -> Grid
            tempTrans001 = _giveSlots.transform.parent.GetComponent<Transform>();
            //2->InventoryImg
            tempTrans002 = tempTrans001.transform.parent.GetComponent<Transform>();
            //3 -> Inventory
            giveInvenClass = tempTrans002.transform.parent.GetComponent<SG_Inventory>();


            // 사용후 비워주기
            tempTrans001 = null;
            tempTrans002 = null;

            for (byte i = 0; i < giveInvenClass.slots.Length; i++)    //슬롯들을 싹 뒤지면서 주는 Slot의 고유번호 찾기
            {
                if (giveInvenClass.slots[i].slotCount == _GiveSlotCount) // 고유번호를 찾았을때 들어갈 조건문
                {
                    // 아이템 정보를 임시로 집어넣어줌
                    moveItem = giveInvenClass.slots[i].item;
                    tempItemCount = giveInvenClass.slots[i].itemCount;
                    giveSlotCount = i;
                    //giveInvenClass.slots[i].ClearSlot();
                    //Debug.LogFormat("moveItem -> {0} tempItemCount -> {1}", moveItem, tempItemCount);
                    break;
                }
            }
            return; // 찾아서 아이템의 정보를 넣어주었다면 return으로 함수 즉시탈출
        }
        else { /*PASS*/ }
        #endregion _GiveSlotCount가 플레이어일 경우

        #region _GiveSlotCount가 산장창고일 경우
        if (_GiveSlotCount > 99 && _GiveSlotCount < 120)
        {
            // 이번엔 부모의 부모의 부모 3번 찾아야함
            //1 -> Grid
            tempTrans001 = _giveSlots.transform.parent.GetComponent<Transform>();
            //2 -> InventoryImg
            tempTrans002 = tempTrans001.transform.parent.GetComponent<Transform>();
            //3 -> Inventory
            giveInvenClass = tempTrans002.transform.parent.GetComponent<SG_Inventory>();
            tempTrans001 = null;
            tempTrans002 = null;

            for (byte i = 0; i < giveInvenClass.slots.Length; i++)    //슬롯들을 싹 뒤지면서 주는 Slot의 고유번호 찾기
            {
                if (giveInvenClass.slots[i].slotCount == _GiveSlotCount) // 고유번호를 찾았을때 들어갈 조건문
                {
                    // 아이템 정보를 임시로 집어넣어줌
                    moveItem = giveInvenClass.slots[i].item;
                    tempItemCount = giveInvenClass.slots[i].itemCount;
                    giveSlotCount = i;
                    //giveInvenClass.slots[i].ClearSlot();
                    //Debug.LogFormat("moveItem -> {0} tempItemCount -> {1}", moveItem, tempItemCount);
                    break;
                }
            }
            return; // 찾아서 아이템의 정보를 넣어주었다면 return으로 함수 즉시탈출
        }
        else { /*PASS*/ }
        #endregion _GiveSlotCount가 산장창고일 경우

    }   //SerchGiveSlot()

    //받는 슬롯을 찾아서 아이템을넣어주는 함수
    public void SerchAccepSlots(int _AcceptSlotCount, SG_ItemSlot _accepSlots)
    {        
        #region 플레이어가 받는 슬롯일때
        if (_AcceptSlotCount > 1 && _AcceptSlotCount < 20) // 플레이어가 받는 Slot일때에
        {
            // 이번엔 부모의 부모의 부모 3번 찾아야함
            //1 -> Grid
            tempTrans001 = _accepSlots.transform.parent.GetComponent<Transform>();
            //2 -> InventoryImg
            tempTrans002 = tempTrans001.transform.parent.GetComponent<Transform>();
            //3 -> Inventory
            acceptInvenClass = tempTrans002.transform.parent.GetComponent<SG_Inventory>();
            // 사용후 비워주기
            tempTrans001 = null;
            tempTrans002 = null;

            #region LEGACY
            //if (acceptInvenClass.slots[i].slotCount == _AcceptSlotCount) // 고유번호를 찾았을때 들어갈 조건문
            //{
            //    // 아이템 정보를 집어넣어줌
            //    acceptInvenClass.slots[i].item = moveItem;
            //    acceptInvenClass.slots[i].itemCount = tempItemCount;
            //    accepSlotCount = i;
            //    Debug.LogFormat("moveItem -> {0} tempItemCount -> {1}", moveItem, tempItemCount);
            //    //아이템을 넣고 아이템 갯수 체크한뒤에 아이템을 3개로 만들어주고 GiveSlot을 초기화 시키지 않고
            //    //Item최대치를 넘어간만큼 줌 
            //    ItemExamine();
            //    break;
            //}
            #endregion LEGACY
            for (byte i = 0; i < acceptInvenClass.slots.Length; i++)    //슬롯들을 싹 뒤지면서 주는 Slot의 고유번호 찾기
            {
                if (acceptInvenClass.slots[i].slotCount == _AcceptSlotCount) // 고유번호를 찾았을때 들어갈 조건문
                {
                    if (acceptInvenClass.slots[i].item == null)
                    {
                        // 아이템 정보를 집어넣어줌
                        acceptInvenClass.slots[i].item = moveItem;
                        acceptInvenClass.slots[i].itemCount = tempItemCount;
                        accepSlotCount = i;
                        //Debug.LogFormat("moveItem -> {0} tempItemCount -> {1}", moveItem, tempItemCount);
                        //아이템을 넣고 아이템 갯수 체크한뒤에 아이템을 3개로 만들어주고 GiveSlot을 초기화 시키지 않고
                        //Item최대치를 넘어간만큼 줌 
                    }
                    else if (acceptInvenClass.slots[i].item == moveItem)
                    {
                        acceptInvenClass.slots[i].itemCount = acceptInvenClass.slots[i].itemCount + tempItemCount;
                        ItemExamine();
                    }
                    Debug.LogFormat("Accept() Give -> {0} Accept -> {1}", giveSlotCount, accepSlotCount);
                    break;
                }
            }
            return; // 찾아서 슬롯에 아이템을 넣어주었다면 return으로 함수 즉시탈출
        }
        else { /*PASS*/ }
        #endregion 플레이어가 받는 슬롯일때

        #region 산장창고가 받는 슬롯일떄
        if (_AcceptSlotCount > 99 && _AcceptSlotCount < 120) // 플레이어가 받는 Slot일때에
        {
            // 이번엔 부모의 부모의 부모 3번 찾아야함
            //1 -> Grid
            tempTrans001 = _accepSlots.transform.parent.GetComponent<Transform>();
            //2 -> InventoryImg
            tempTrans002 = tempTrans001.transform.parent.GetComponent<Transform>();
            //3 -> Inventory
            acceptInvenClass = tempTrans002.transform.parent.GetComponent<SG_Inventory>();
            // 사용후 비워주기
            tempTrans001 = null;
            tempTrans002 = null;

            for (byte i = 0; i < acceptInvenClass.slots.Length; i++)    //슬롯들을 싹 뒤지면서 주는 Slot의 고유번호 찾기
            {
                if (acceptInvenClass.slots[i].slotCount == _AcceptSlotCount) // 고유번호를 찾았을때 들어갈 조건문
                {
                    if (acceptInvenClass.slots[i].item == null)
                    {
                        // 아이템 정보를 집어넣어줌
                        acceptInvenClass.slots[i].item = moveItem;
                        acceptInvenClass.slots[i].itemCount = tempItemCount;
                        accepSlotCount = i;
                        //Debug.LogFormat("moveItem -> {0} tempItemCount -> {1}", moveItem, tempItemCount);
                        //아이템을 넣고 아이템 갯수 체크한뒤에 아이템을 3개로 만들어주고 GiveSlot을 초기화 시키지 않고
                        //Item최대치를 넘어간만큼 줌 
                    }
                    else if (acceptInvenClass.slots[i].item == moveItem)
                    {
                        acceptInvenClass.slots[i].itemCount = acceptInvenClass.slots[i].itemCount + tempItemCount;
                        ItemExamine();
                    }
                    Debug.LogFormat("Accept() Give -> {0} Accept -> {1}", giveSlotCount, accepSlotCount);
                    break;
                }
            }
            return; // 찾아서 슬롯에 아이템을 넣어주었다면 return으로 함수 즉시탈출
        }
        else { /*PASS*/ }
        #endregion 산장창고가 받는 슬롯일때
       
    }   //SerchAccepSlots


    // 아이템 갯수가 넘는지 넘지 않는지 검사하는 함수
    private void ItemExamine()
    {
        if (acceptInvenClass.slots[accepSlotCount].itemCount > 3)    // 받는쪽의 아이템이 3개가 넘는지 체크
        {
            int tempItemCount001 = acceptInvenClass.slots[accepSlotCount].itemCount;
            int tempItemCount002 = tempItemCount001 - 3;

            acceptInvenClass.slots[accepSlotCount].itemCount = 3;
            isPassExamine = false;
            ItemExanineAfter(tempItemCount002);

        }
        else { /*PASS*/ }

    }

    // 아이템 검사 함수에서 불통하면 들어올 함수
    // Give Item Canvas위치 원래대로해줄거고 아이템에 대한 정보와 출력을 처리해줄거임
    private void ItemExanineAfter(int _itemCount)
    {
        giveItemDragScript.SetTransformParent();
        giveInvenClass.slots[giveSlotCount].item = moveItem;
        giveInvenClass.slots[giveSlotCount].itemCount = _itemCount;
        giveInvenClass.slots[giveSlotCount].MoveItemSet();
    }

    // 아이템을주고 후처리해주는 함수 -> 슬롯 초기화
    public void GiveExchangeAfter()
    {
        Debug.LogFormat("Give -> {0} Accept -> {1}", giveSlotCount, accepSlotCount);
        #region LEGACY
        //// 인벤토리 클래스는 아직 인벤토리의 Script를 담고 있기 때문에 그대로 사용해도 됨
        //for (byte i = 0; i < giveInvenClass.slots.Length; i++)    //슬롯들을 싹 뒤지면서 주는 Slot의 고유번호 찾기
        //{
        //    if (giveInvenClass.slots[i].slotCount == _GiveSlotCount) // 고유번호를 찾았을때 들어갈 조건문
        //    {
        //        giveInvenClass.slots[i].DisconnectedItem();
        //        break;
        //    }
        //}
        #endregion  LEGACY
        //if (giveSlotCount >= 0 && giveSlotCount < giveInvenClass.slots.Length)
        //{
            if (giveInvenClass.slots[giveSlotCount].item == acceptInvenClass.slots[accepSlotCount].item)
            {
                if (isPassExamine == true)
                {
                    // TODO : 아이템이 못들어갔을때에 Clear하지 못하도록 조건을 수정해야함
                    giveInvenClass.slots[giveSlotCount].DisconnectedItem();
                    //isPassExamine = false;
                }
                else
                {
                    isPassExamine = true;
                }
            }
            else { /*PASS*/ }
        //}
        //else { Debug.LogError("Invalid giveSlotCount: " + giveSlotCount); }
    }


    // 아이템을받고 후처리해주는 함수 -> 슬롯이 다시 받은아이템을 출력하기
    public void AcceptExchangeAfter()
    {
        #region LEGACY
        // 인벤토리 클래스는 아직 인벤토리의 Script를 담고 있기 때문에 그대로 사용해도 됨       
        //for (byte i = 0; i < acceptInvenClass.slots.Length; i++)    //슬롯들을 싹 뒤지면서 주는 Slot의 고유번호 찾기
        //{
        //    if (acceptInvenClass.slots[i].slotCount == _AcceptSlotCount) // 고유번호를 찾았을때 들어갈 조건문
        //    {
        //        acceptInvenClass.slots[i].MoveItemSet();
        //        break;
        //    }
        //}
        #endregion LEGACY

            acceptInvenClass.slots[accepSlotCount].MoveItemSet();
    }

}   //NameSpace 
