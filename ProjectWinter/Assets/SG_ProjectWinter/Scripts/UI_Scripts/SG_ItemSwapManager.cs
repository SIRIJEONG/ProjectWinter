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


    // �Ű����� ���� : �ִ¾� �ּ�, �޴¾��ּ� , �ִ� �����۽���, �޴� �����۽���

    //SG_ItemSlot giveSlotClass;  // �������� �� ������ Ŭ����
    //SG_ItemSlot accepSlotClass; // �������� ���� ������ Ŭ����


    SG_Inventory giveInvenClass;    // �������� �� �κ��丮 Ŭ����
    SG_Inventory accepInvenClass;   // �������� ���� �κ��丮 Ŭ����

    //Ʈ���������� �޾ƾ��� GameObject�� ������ Grid���� �����Ұ�
    Transform tempTrans001;
    Transform tempTrans002;

    SG_Item moveItem;   // �ű� ������ ����

    int tempItemCount;  // �ӽ� ������ ���� ������ ����


    public void ItemSwap(int _GiveSlotCount, int _AcceptSlotCount,
                         SG_ItemSlot _giveSlots, SG_ItemSlot _accepSlots)
    {
        //Debug.Log("Swap �Լ��� ������� �ϴ°�?");
        SerchGiveSlot(_GiveSlotCount, _giveSlots);
        SerchAccepSlots(_AcceptSlotCount, _accepSlots);
        GiveExchangeAfter(_GiveSlotCount);
        AcceptExchangeAfter(_AcceptSlotCount);

    }   // ItemSwap()


    // �ִ� ������ ã�Ƽ� �������� ������ �����ϴ� �Լ�
    public void SerchGiveSlot(int _GiveSlotCount, SG_ItemSlot _giveSlots)
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

            for (int i = 0; i < giveInvenClass.slots.Length; i++)    //���Ե��� �� �����鼭 �ִ� Slot�� ������ȣ ã��
            {
                if (giveInvenClass.slots[i].slotCount == _GiveSlotCount) // ������ȣ�� ã������ �� ���ǹ�
                {
                    // ������ ������ �ӽ÷� ����־���
                    moveItem = giveInvenClass.slots[i].item;
                    tempItemCount = giveInvenClass.slots[i].itemCount;
                    giveInvenClass.slots[i].ClearSlot();
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
            for (int i = 0; i < giveInvenClass.slots.Length; i++)    //���Ե��� �� �����鼭 �ִ� Slot�� ������ȣ ã��
            {
                if (giveInvenClass.slots[i].slotCount == _GiveSlotCount) // ������ȣ�� ã������ �� ���ǹ�
                {
                    // ������ ������ �ӽ÷� ����־���
                    moveItem = giveInvenClass.slots[i].item;
                    tempItemCount = giveInvenClass.slots[i].itemCount;
                    giveInvenClass.slots[i].ClearSlot();
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
            accepInvenClass = tempTrans002.transform.parent.GetComponent<SG_Inventory>();
            // ����� ����ֱ�
            tempTrans001 = null;
            tempTrans002 = null;

            for (int i = 0; i < accepInvenClass.slots.Length; i++)    //���Ե��� �� �����鼭 �ִ� Slot�� ������ȣ ã��
            {
                if (accepInvenClass.slots[i].slotCount == _AcceptSlotCount) // ������ȣ�� ã������ �� ���ǹ�
                {
                    // ������ ������ �ӽ÷� ����־���
                    accepInvenClass.slots[i].item = moveItem;
                    accepInvenClass.slots[i].itemCount = tempItemCount;
                    Debug.LogFormat("moveItem -> {0} tempItemCount -> {1}", moveItem, tempItemCount);
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
            accepInvenClass = tempTrans002.transform.parent.GetComponent<SG_Inventory>();
            // ����� ����ֱ�
            tempTrans001 = null;
            tempTrans002 = null;

            for (int i = 0; i < accepInvenClass.slots.Length; i++)    //���Ե��� �� �����鼭 �ִ� Slot�� ������ȣ ã��
            {
                if (accepInvenClass.slots[i].slotCount == _AcceptSlotCount) // ������ȣ�� ã������ �� ���ǹ�
                {
                    // ������ ������ �ӽ÷� ����־���
                    accepInvenClass.slots[i].item = moveItem;
                    accepInvenClass.slots[i].itemCount = tempItemCount;
                    Debug.LogFormat("moveItem -> {0} tempItemCount -> {1}", moveItem, tempItemCount);
                    break;
                }
            }
            return; // ã�Ƽ� ���Կ� �������� �־��־��ٸ� return���� �Լ� ���Ż��
        }
        else { /*PASS*/ }
        #endregion ����â�� �޴� �����϶�
    }   //SerchAccepSlots


    // ���������ְ� ��ó�����ִ� �Լ� -> ���� �ʱ�ȭ
    public void GiveExchangeAfter(int _GiveSlotCount)
    {
        // �κ��丮 Ŭ������ ���� �κ��丮�� Script�� ��� �ֱ� ������ �״�� ����ص� ��
        for (int i = 0; i < giveInvenClass.slots.Length; i++)    //���Ե��� �� �����鼭 �ִ� Slot�� ������ȣ ã��
        {
            if (giveInvenClass.slots[i].slotCount == _GiveSlotCount) // ������ȣ�� ã������ �� ���ǹ�
            {
                giveInvenClass.slots[i].DisconnectedItem();
                break;
            }
        }

    }
    // ���������ް� ��ó�����ִ� �Լ� -> ������ �ٽ� ������������ ����ϱ�
    public void AcceptExchangeAfter(int _AcceptSlotCount)
    {
        // �κ��丮 Ŭ������ ���� �κ��丮�� Script�� ��� �ֱ� ������ �״�� ����ص� ��       
        for (int i = 0; i < accepInvenClass.slots.Length; i++)    //���Ե��� �� �����鼭 �ִ� Slot�� ������ȣ ã��
        {
            if (accepInvenClass.slots[i].slotCount == _AcceptSlotCount) // ������ȣ�� ã������ �� ���ǹ�
            {
                accepInvenClass.slots[i].MoveItemSet();
                break;
            }
        }
    }

}   //NameSpace
