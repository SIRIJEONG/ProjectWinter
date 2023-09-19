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
    private bool isSwap = true;         // �޴� �����ۿ��� ���ǿ� �ȸ����� ���� ���ϰ� false �� ����� �Լ����� return�����ٰ���
    private bool sameSlot = false;

    public void ItemSwap(SG_ItemDragScript _itemDragScript, int _GiveSlotCount, int _AcceptSlotCount,
                         SG_ItemSlot _giveSlots, SG_ItemSlot _accepSlots)
    {
        giveItemDragScript = _itemDragScript;
        ThisSameSlot(_GiveSlotCount,_AcceptSlotCount);  // ������ ������ Ȯ���� �ٸ����� Swap�� �̷������ ������ִ� �Լ�


        if(sameSlot == false)
        {
            //���ҽ� �ʿ��� ����
            SearchGiveSlot(_GiveSlotCount, _giveSlots);     // �ִ½����� ã�Ƽ� �������� �����ϴ� �Լ�
            SerchAccepSlots(_AcceptSlotCount, _accepSlots); // �޴� ������ ã�� �������� ���ǿ� ����ް� �˼��ϴ� �Լ��� �Ѱ��ִ� �Լ�
            GiveExchangeAfter();  // �������� ���������� �Ѱ������� �� ������ �ʱ�ȭ �ϴ� �Լ�
            AcceptExchangeAfter();// �������� ���������� �Ѱ����� ���������� ���������� ��µǵ��� ���ִ� �Լ�            
        }

        InitialValue();       // �ʱⰪ���� ���ƿ;��ϴ� ������ �ʱⰪ���� �����ִ� �Լ�


    }   // ItemSwap()

    // ���� ���Կ� ���Ҵ��� -> ex) �ִ½��� = 1 , �޴� ���� = 1 �ϰ��

    private void ThisSameSlot(int _GiveSlotCount, int _AcceptSlotCount)
    {
        if (_GiveSlotCount == _AcceptSlotCount)
        {
            sameSlot = true;
        }
        else { /*PASS*/ }
    }

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

            for (byte i = 0; i < giveInvenClass.slots.Length; i++)    //���Ե��� �� �����鼭 �ִ� Slot�� ������ȣ ã��
            {
                if (giveInvenClass.slots[i].slotCount == _GiveSlotCount) // ������ȣ�� ã������ �� ���ǹ�
                {
                    // ������ ������ �ӽ÷� ����־���
                    moveItem = giveInvenClass.slots[i].item;
                    tempItemCount = giveInvenClass.slots[i].itemCount;
                    giveSlotCount = i;
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

            for (byte i = 0; i < giveInvenClass.slots.Length; i++)    //���Ե��� �� �����鼭 �ִ� Slot�� ������ȣ ã��
            {
                if (giveInvenClass.slots[i].slotCount == _GiveSlotCount) // ������ȣ�� ã������ �� ���ǹ�
                {
                    // ������ ������ �ӽ÷� ����־���
                    moveItem = giveInvenClass.slots[i].item;
                    tempItemCount = giveInvenClass.slots[i].itemCount;
                    giveSlotCount = i;
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

            for (byte i = 0; i < acceptInvenClass.slots.Length; i++)    //���Ե��� �� �����鼭 �ִ� Slot�� ������ȣ ã��
            {
                if (acceptInvenClass.slots[i].slotCount == _AcceptSlotCount) // ������ȣ�� ã������ �� ���ǹ�
                {
                    //�������� ������� �ʰ� �ִ¾����۰� �� �ִ� �������� ���� ������
                    if (acceptInvenClass.slots[i].item != null && acceptInvenClass.slots[i].item != moveItem)
                    {
                        isSwap = false;
                    }
                    else if (acceptInvenClass.slots[i].item == moveItem)
                    {
                        acceptInvenClass.slots[i].itemCount = acceptInvenClass.slots[i].itemCount + tempItemCount;
                        //Debug.LogFormat("�������� ������ ���� -> {0}", acceptInvenClass.slots[i].itemCount);
                        accepSlotCount = i;
                        ItemExamine();
                    }
                    else if (acceptInvenClass.slots[i].item == null)
                    {
                        // ������ ������ ����־���
                        acceptInvenClass.slots[i].item = moveItem;
                        acceptInvenClass.slots[i].itemCount = tempItemCount;
                        accepSlotCount = i;
                        ItemExamine();
                        Debug.LogFormat("For_accepSlotCount -> {0}", accepSlotCount);
                    }
                    //Debug.LogFormat("Accept() Give -> {0} Accept -> {1}", giveSlotCount, accepSlotCount);
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

            for (byte i = 0; i < acceptInvenClass.slots.Length; i++)    //���Ե��� �� �����鼭 �ִ� Slot�� ������ȣ ã��
            {
                if (acceptInvenClass.slots[i].slotCount == _AcceptSlotCount) // ������ȣ�� ã������ �� ���ǹ�
                {
                    //�������� ������� �ʰ� �ִ¾����۰� �� �ִ� �������� ���� ������
                    if (acceptInvenClass.slots[i].item != null && acceptInvenClass.slots[i].item != moveItem)
                    {
                        isSwap = false;
                    }
                    else if (acceptInvenClass.slots[i].item == moveItem)
                    {
                        acceptInvenClass.slots[i].itemCount = acceptInvenClass.slots[i].itemCount + tempItemCount;
                        //Debug.LogFormat("�������� ������ ���� -> {0}", acceptInvenClass.slots[i].itemCount);
                        accepSlotCount = i;
                        ItemExamine();
                    }
                    else if (acceptInvenClass.slots[i].item == null)
                    {
                        // ������ ������ ����־���
                        acceptInvenClass.slots[i].item = moveItem;
                        acceptInvenClass.slots[i].itemCount = tempItemCount;
                        accepSlotCount = i;
                        ItemExamine();
                    }
                    //Debug.LogFormat("Accept() Give -> {0} Accept -> {1}", giveSlotCount, accepSlotCount);
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
        //Debug.Log("������ ���� üũ�ϴ� �Լ��� �� �������°�?");
        Debug.LogFormat("accepSlotCount -> {0}", accepSlotCount);
        //Debug.LogFormat("AccepSlot ItemCount -> {0}", acceptInvenClass.slots[accepSlotCount].itemCount);
        //Debug.LogFormat("AccInvenClassNotNull? -> {0}",acceptInvenClass != null);
        if (acceptInvenClass.slots[accepSlotCount].itemCount > 3)    // �޴����� �������� 3���� �Ѵ��� üũ
        {
            //Debug.LogFormat("�������� 3���� �Ѿ�� ���ǿ� �ߵ��Դ°�?");
            int tempItemCount001 = acceptInvenClass.slots[accepSlotCount].itemCount;
            int tempItemCount002 = tempItemCount001 - 3;

            acceptInvenClass.slots[accepSlotCount].itemCount = 3;
            //Debug.Log($"ī��Ʈ 3���� ������ ���� -> {acceptInvenClass.slots[accepSlotCount].itemCount}");

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
        if (isSwap == false)
        { return; }
        else { /*PASS*/ }

        //Debug.LogFormat("Give -> {0} Accept -> {1}", giveSlotCount, accepSlotCount);
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
        if (isSwap == false)
        { return; }
        else { /*PASS*/ }

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
        //if (acceptInvenClass.slots[accepSlotCount])
        acceptInvenClass.slots[accepSlotCount].MoveItemSet();
    }

    // �������� ������������ �ٽ� ������� �ʱ�ȭ �Ǿ�� �ϴ� ������ ������� ���ִ� �Լ�
    private void InitialValue()
    {
        isSwap = true;
        sameSlot = false;
    }

}   //NameSpace 
