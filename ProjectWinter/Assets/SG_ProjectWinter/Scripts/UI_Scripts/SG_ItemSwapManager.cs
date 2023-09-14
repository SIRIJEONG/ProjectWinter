using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class SG_ItemSwapManager : MonoBehaviour
{
    // �������� �����Ҷ� �� ��ũ��Ʈ�� ���ؼ� ���� �迭�� �����ͼ� �ٲ��� ����

    // �÷��̾� ���� ��ȣ�� 10�� ���ؼ� ��ġ�� �κ��� ������ �Ͽ��� 10 11 12 13 �̷���ȣ�� ���ð���

    // �Ʒ� �Լ��� �Ű������� �÷��̾�� â��� �����ʰ� �ִ��κ��丮 , �޴� �κ��丮 ����� �޾ƾ��ҰŰ���

    // �÷��̾� �κ��丮 10 ~ 13
    // ����â�� �κ��丮 100 ~ 115

    // �޴� �Ű����� accept , �ִ� �Ű���� give

    // �Ʒ� �Լ��� �ʿ��� �Ű����� �ִ��κ� , �ִ� �κ��� Slot �ּ�,�޴��κ� ,�޴��κ��� Slot �ּ�, ������ ���� , ������ ����

    // �ּҸ� �̿��ؼ� for������ ���� �ּҸ� ã�� �ִ¾��� �κ��� �ٻ��� �޴¾��κ��� ä���ֱ⸸ �ϸ� �ɼ��� �����Ű���

    // �Ű����� ���� : �ִ¾� �ּ�, �޴¾��ּ� , �÷��̾� ����, �κ��丮 ����
    // �� 5��

    // �Ʒ� �ΰ��� �κ��丮 ����
    SG_Inventory playerInven;
    SG_WareHouseInventory wareHouseInven;

    SG_ItemSlot plyaerItemSlotClass;
    SG_WareHouseItemSlot wareHouseItemSlot;

    // �ű� ������ ����
    SG_Item moveItem;

    // �κ��丮 ã���� �ӽ÷� ����� ���� ����
    GameObject tempObj001;
    GameObject tempObj002;

    // �ӽ� ������ ���� ������ ����
    int tempItemCount;


    public void ItemSwap(int _GiveSlotCount, int _AcceptSlotCount,
                         SG_ItemSlot _playerSlots, SG_WareHouseItemSlot _wereHouseSlots)
    {

        //Debug.LogFormat("Player�Ű�����slots�� ��Ȯ�� ������ǥ�� ������ �ֳ�? -> {0}", _playerSlots.slotCount);
        //Debug.LogFormat("â�� �Ű�����slots�� ��Ȯ�� ������ǥ�� ������ �ֳ�? -> {0}", _wereHouseSlots.wareHouseSlotCount);

        #region _GiveSlotCount�� �÷��̾��� ���
        if (_GiveSlotCount > 1 && _GiveSlotCount < 20) // �÷��̾� -> �κ��丮 �̵���
        {
            // �̹��� �θ��� �θ��� �θ� 3�� ã�ƾ���
            //1 -> Grid
            tempObj001 = _playerSlots.transform.parent.GetComponent<GameObject>();
            //2 -> InventoryImg
            tempObj002 = tempObj001.transform.parent.GetComponent<GameObject>();
            //3 -> Inventory
            playerInven = tempObj002.transform.parent.GetComponent<SG_Inventory>();
            // ����� ����ֱ�
            tempObj001 = null;
            tempObj002 = null;

            for (int i = 0; i < playerInven.slots.Length; i++)    //���Ե��� �� �����鼭 �ִ� Slot�� ������ȣ ã��
            {
                if (playerInven.slots[i].slotCount == _GiveSlotCount) // ������ȣ�� ã������ �� ���ǹ�
                {
                    // ������ ������ �ӽ÷� ����־���
                    moveItem = playerInven.slots[i].item;
                    // ������ ���� �� �Ű� ������ �ȹ޾Ƶ� �ɵ�?
                    tempItemCount = playerInven.slots[i].itemCount;
                    playerInven.slots[i].ClearSlot();
                    Debug.Log("Swap�� �÷��̾� For�� ������?");
                    Debug.LogFormat("moveItem -> {0} tempItemCount -> {1}", moveItem, tempItemCount);
                    break;
                }
            }
        }
        #endregion _GiveSlotCount�� �÷��̾��� ���



        else if (_GiveSlotCount >= 100) // �κ��丮 -> �÷��̾� �̵���
        {

        }

        #region _AcceptSlotCount�� ����â���ϰ��
        if(_AcceptSlotCount >= 100 && _AcceptSlotCount <= 120)
        {
            // â�� ���� Inventory Slots[] �� ã�ƿ��� ���� �ڵ�
            tempObj001 = _wereHouseSlots.transform.parent.GetComponent<GameObject>();
            tempObj002 = tempObj001.transform.parent.GetComponent<GameObject>();
            wareHouseInven = tempObj002.transform.parent.GetComponent<SG_WareHouseInventory>();
            // �ӽ� ���� Obj ����� null�� ����
            tempObj001 = null;
            tempObj002 = null;

            for(int i =0; i < wareHouseInven.slots.Length; i++)
            {
                if(_AcceptSlotCount == wareHouseInven.slots[i].wareHouseSlotCount)
                {
                    wareHouseItemSlot = wareHouseInven.slots[i].GetComponent<SG_WareHouseItemSlot>();
                    wareHouseItemSlot.item = moveItem;
                    wareHouseItemSlot.itemCount = tempItemCount;
                    Debug.LogFormat("â�� �������� Null? -> {0}", wareHouseItemSlot.item == null);
                    Debug.LogFormat("�� ������ -> {0}", moveItem.name);

                    break;
                }
            }
        }
        #endregion _AcceptSlotCount�� ����â���ϰ��

    }   // ItemSwap()

    private void SerchGiveSlot(int _GiveSlotCount,
        SG_ItemSlot playerSlots, SG_WareHouseItemSlot _wereHouseSlots)
    {
        if (10 <= _GiveSlotCount && _GiveSlotCount < 100)
        {

        }
    }



}   //NameSpace
