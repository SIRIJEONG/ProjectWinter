using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class SG_ItemSwapManager : MonoBehaviour
{
    // �÷��̾� ���� ��ȣ�� 10�� ���ؼ� ��ġ�� �κ��� ������ �Ͽ��� 10 11 12 13 �̷���ȣ�� ���ð���

    // �Ʒ� �Լ��� �Ű������� �÷��̾�� â��� �����ʰ� �ִ��κ��丮 , �޴� �κ��丮 ����� �޾ƾ��ҰŰ���

    // �÷��̾� �κ��丮 10 ~ 13
    // ����â�� �κ��丮 100 ~ 115

    // �޴� �Ű����� accept , �ִ� �Ű���� give

    // �������� �ً��� ���� �˻��ϰ� �ӽ� ������ ��Ƶδ��� �Ѱ����� ������ �ؾ��ҰŰ���


    // �Ű����� ���� : �ִ¾� �ּ�, �޴¾��ּ� , �ִ� �����۽���, �޴� �����۽���

    //SG_ItemSlot giveSlotClass;  // �������� �� ������ Ŭ����
    //SG_ItemSlot accepSlotClass; // �������� ���� ������ Ŭ����


    private SG_Inventory giveInvenClass;    // �������� �� �κ��丮 Ŭ����
    private SG_Inventory acceptInvenClass;   // �������� ���� �κ��丮 Ŭ����

    //Ʈ���������� �޾ƾ��� GameObject�� ������ Grid���� �����Ұ�
    private Transform tempTrans001;
    private Transform tempTrans002;
    private SG_ItemDragScript giveItemDragScript; // 3�� �̻��϶� ���� �������� �־���ϰ� ���Կ� �°� �����ϱ� ������ �ʿ�

    private SG_Item moveItem;   // �ű� ������ ����

    private int tempItemCount;  // �ӽ� ������ ���� ������ ����

    private byte giveSlotCount; // ���ʿ��� for���� ���̱� ���� ó�� ���� ã���� �� ������ ��ǥ�� ������ byte ����
    private byte accepSlotCount;

    private bool isPassExamine = true;  // ������ �˻��ϰ� �˻� ���ǿ� ���� false �ιٲ��ٰ���

    public void ItemSwap(SG_ItemDragScript _itemDragScript, int _GiveSlotCount, int _AcceptSlotCount,
                         SG_ItemSlot _giveSlots, SG_ItemSlot _accepSlots)
    {
        giveItemDragScript = _itemDragScript;
        //Debug.Log("Swap �Լ��� ������� �ϴ°�?");
        SearchGiveSlot(_GiveSlotCount, _giveSlots);
        SerchAccepSlots(_AcceptSlotCount, _accepSlots);
        GiveExchangeAfter();
        // �������ִ� ������ �ʰ��ߴٸ� �ʰ��Ѹ�ŭ �����ִ� �Լ�
        AcceptExchangeAfter();

    }   // ItemSwap()


    // �ִ� ������ ã�Ƽ� �������� ������ �����ϴ� �Լ�
    public void SearchGiveSlot(int _GiveSlotCount, SG_ItemSlot _giveSlots)
    {

        #region _GiveSlotCount�� �÷��̾��� ���
        if (_GiveSlotCount > 1 && _GiveSlotCount < 20) // �÷��̾ �ִ� �����϶���
        {
            // �̹��� �θ��� �θ��� �θ� 3�� ã�ƾ��� 
            //1 -> Grid
            tempTrans001 = _giveSlots.transform.parent.GetComponent<Transform>();
            //2->InventoryImg
            tempTrans002 = tempTrans001.transform.parent.GetComponent<Transform>();
            //3 -> Inventory
            giveInvenClass = tempTrans002.transform.parent.GetComponent<SG_Inventory>();


            // ����� ����ֱ�
            tempTrans001 = null;
            tempTrans002 = null;

            for (byte i = 0; i < giveInvenClass.slots.Length; i++)    //���Ե��� �� �����鼭 �ִ� Slot�� ������ȣ ã��
            {
                if (giveInvenClass.slots[i].slotCount == _GiveSlotCount) // ������ȣ�� ã������ �� ���ǹ�
                {
                    // ������ ������ �ӽ÷� ����־���
                    moveItem = giveInvenClass.slots[i].item;
                    tempItemCount = giveInvenClass.slots[i].itemCount;
                    giveSlotCount = i;
                    //giveInvenClass.slots[i].ClearSlot();
                    //Debug.LogFormat("moveItem -> {0} tempItemCount -> {1}", moveItem, tempItemCount);
                    break;
                }
            }
            return; // ã�Ƽ� �������� ������ �־��־��ٸ� return���� �Լ� ���Ż��
        }
        else { /*PASS*/ }
        #endregion _GiveSlotCount�� �÷��̾��� ���

        #region _GiveSlotCount�� ����â���� ���
        if (_GiveSlotCount > 99 && _GiveSlotCount < 120)
        {
            // �̹��� �θ��� �θ��� �θ� 3�� ã�ƾ���
            //1 -> Grid
            tempTrans001 = _giveSlots.transform.parent.GetComponent<Transform>();
            //2 -> InventoryImg
            tempTrans002 = tempTrans001.transform.parent.GetComponent<Transform>();
            //3 -> Inventory
            giveInvenClass = tempTrans002.transform.parent.GetComponent<SG_Inventory>();
            tempTrans001 = null;
            tempTrans002 = null;

            for (byte i = 0; i < giveInvenClass.slots.Length; i++)    //���Ե��� �� �����鼭 �ִ� Slot�� ������ȣ ã��
            {
                if (giveInvenClass.slots[i].slotCount == _GiveSlotCount) // ������ȣ�� ã������ �� ���ǹ�
                {
                    // ������ ������ �ӽ÷� ����־���
                    moveItem = giveInvenClass.slots[i].item;
                    tempItemCount = giveInvenClass.slots[i].itemCount;
                    giveSlotCount = i;
                    //giveInvenClass.slots[i].ClearSlot();
                    //Debug.LogFormat("moveItem -> {0} tempItemCount -> {1}", moveItem, tempItemCount);
                    break;
                }
            }
            return; // ã�Ƽ� �������� ������ �־��־��ٸ� return���� �Լ� ���Ż��
        }
        else { /*PASS*/ }
        #endregion _GiveSlotCount�� ����â���� ���

    }   //SerchGiveSlot()

    //�޴� ������ ã�Ƽ� ���������־��ִ� �Լ�
    public void SerchAccepSlots(int _AcceptSlotCount, SG_ItemSlot _accepSlots)
    {        
        #region �÷��̾ �޴� �����϶�
        if (_AcceptSlotCount > 1 && _AcceptSlotCount < 20) // �÷��̾ �޴� Slot�϶���
        {
            // �̹��� �θ��� �θ��� �θ� 3�� ã�ƾ���
            //1 -> Grid
            tempTrans001 = _accepSlots.transform.parent.GetComponent<Transform>();
            //2 -> InventoryImg
            tempTrans002 = tempTrans001.transform.parent.GetComponent<Transform>();
            //3 -> Inventory
            acceptInvenClass = tempTrans002.transform.parent.GetComponent<SG_Inventory>();
            // ����� ����ֱ�
            tempTrans001 = null;
            tempTrans002 = null;

            #region LEGACY
            //if (acceptInvenClass.slots[i].slotCount == _AcceptSlotCount) // ������ȣ�� ã������ �� ���ǹ�
            //{
            //    // ������ ������ ����־���
            //    acceptInvenClass.slots[i].item = moveItem;
            //    acceptInvenClass.slots[i].itemCount = tempItemCount;
            //    accepSlotCount = i;
            //    Debug.LogFormat("moveItem -> {0} tempItemCount -> {1}", moveItem, tempItemCount);
            //    //�������� �ְ� ������ ���� üũ�ѵڿ� �������� 3���� ������ְ� GiveSlot�� �ʱ�ȭ ��Ű�� �ʰ�
            //    //Item�ִ�ġ�� �Ѿ��ŭ �� 
            //    ItemExamine();
            //    break;
            //}
            #endregion LEGACY
            for (byte i = 0; i < acceptInvenClass.slots.Length; i++)    //���Ե��� �� �����鼭 �ִ� Slot�� ������ȣ ã��
            {
                if (acceptInvenClass.slots[i].slotCount == _AcceptSlotCount) // ������ȣ�� ã������ �� ���ǹ�
                {
                    if (acceptInvenClass.slots[i].item == null)
                    {
                        // ������ ������ ����־���
                        acceptInvenClass.slots[i].item = moveItem;
                        acceptInvenClass.slots[i].itemCount = tempItemCount;
                        accepSlotCount = i;
                        //Debug.LogFormat("moveItem -> {0} tempItemCount -> {1}", moveItem, tempItemCount);
                        //�������� �ְ� ������ ���� üũ�ѵڿ� �������� 3���� ������ְ� GiveSlot�� �ʱ�ȭ ��Ű�� �ʰ�
                        //Item�ִ�ġ�� �Ѿ��ŭ �� 
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
            return; // ã�Ƽ� ���Կ� �������� �־��־��ٸ� return���� �Լ� ���Ż��
        }
        else { /*PASS*/ }
        #endregion �÷��̾ �޴� �����϶�

        #region ����â�� �޴� �����ϋ�
        if (_AcceptSlotCount > 99 && _AcceptSlotCount < 120) // �÷��̾ �޴� Slot�϶���
        {
            // �̹��� �θ��� �θ��� �θ� 3�� ã�ƾ���
            //1 -> Grid
            tempTrans001 = _accepSlots.transform.parent.GetComponent<Transform>();
            //2 -> InventoryImg
            tempTrans002 = tempTrans001.transform.parent.GetComponent<Transform>();
            //3 -> Inventory
            acceptInvenClass = tempTrans002.transform.parent.GetComponent<SG_Inventory>();
            // ����� ����ֱ�
            tempTrans001 = null;
            tempTrans002 = null;

            for (byte i = 0; i < acceptInvenClass.slots.Length; i++)    //���Ե��� �� �����鼭 �ִ� Slot�� ������ȣ ã��
            {
                if (acceptInvenClass.slots[i].slotCount == _AcceptSlotCount) // ������ȣ�� ã������ �� ���ǹ�
                {
                    if (acceptInvenClass.slots[i].item == null)
                    {
                        // ������ ������ ����־���
                        acceptInvenClass.slots[i].item = moveItem;
                        acceptInvenClass.slots[i].itemCount = tempItemCount;
                        accepSlotCount = i;
                        //Debug.LogFormat("moveItem -> {0} tempItemCount -> {1}", moveItem, tempItemCount);
                        //�������� �ְ� ������ ���� üũ�ѵڿ� �������� 3���� ������ְ� GiveSlot�� �ʱ�ȭ ��Ű�� �ʰ�
                        //Item�ִ�ġ�� �Ѿ��ŭ �� 
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
            return; // ã�Ƽ� ���Կ� �������� �־��־��ٸ� return���� �Լ� ���Ż��
        }
        else { /*PASS*/ }
        #endregion ����â�� �޴� �����϶�
       
    }   //SerchAccepSlots


    // ������ ������ �Ѵ��� ���� �ʴ��� �˻��ϴ� �Լ�
    private void ItemExamine()
    {
        if (acceptInvenClass.slots[accepSlotCount].itemCount > 3)    // �޴����� �������� 3���� �Ѵ��� üũ
        {
            int tempItemCount001 = acceptInvenClass.slots[accepSlotCount].itemCount;
            int tempItemCount002 = tempItemCount001 - 3;

            acceptInvenClass.slots[accepSlotCount].itemCount = 3;
            isPassExamine = false;
            ItemExanineAfter(tempItemCount002);

        }
        else { /*PASS*/ }

    }

    // ������ �˻� �Լ����� �����ϸ� ���� �Լ�
    // Give Item Canvas��ġ ����������ٰŰ� �����ۿ� ���� ������ ����� ó�����ٰ���
    private void ItemExanineAfter(int _itemCount)
    {
        giveItemDragScript.SetTransformParent();
        giveInvenClass.slots[giveSlotCount].item = moveItem;
        giveInvenClass.slots[giveSlotCount].itemCount = _itemCount;
        giveInvenClass.slots[giveSlotCount].MoveItemSet();
    }

    // ���������ְ� ��ó�����ִ� �Լ� -> ���� �ʱ�ȭ
    public void GiveExchangeAfter()
    {
        Debug.LogFormat("Give -> {0} Accept -> {1}", giveSlotCount, accepSlotCount);
        #region LEGACY
        //// �κ��丮 Ŭ������ ���� �κ��丮�� Script�� ��� �ֱ� ������ �״�� ����ص� ��
        //for (byte i = 0; i < giveInvenClass.slots.Length; i++)    //���Ե��� �� �����鼭 �ִ� Slot�� ������ȣ ã��
        //{
        //    if (giveInvenClass.slots[i].slotCount == _GiveSlotCount) // ������ȣ�� ã������ �� ���ǹ�
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
                    // TODO : �������� ���������� Clear���� ���ϵ��� ������ �����ؾ���
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


    // ���������ް� ��ó�����ִ� �Լ� -> ������ �ٽ� ������������ ����ϱ�
    public void AcceptExchangeAfter()
    {
        #region LEGACY
        // �κ��丮 Ŭ������ ���� �κ��丮�� Script�� ��� �ֱ� ������ �״�� ����ص� ��       
        //for (byte i = 0; i < acceptInvenClass.slots.Length; i++)    //���Ե��� �� �����鼭 �ִ� Slot�� ������ȣ ã��
        //{
        //    if (acceptInvenClass.slots[i].slotCount == _AcceptSlotCount) // ������ȣ�� ã������ �� ���ǹ�
        //    {
        //        acceptInvenClass.slots[i].MoveItemSet();
        //        break;
        //    }
        //}
        #endregion LEGACY

            acceptInvenClass.slots[accepSlotCount].MoveItemSet();
    }

}   //NameSpace 
