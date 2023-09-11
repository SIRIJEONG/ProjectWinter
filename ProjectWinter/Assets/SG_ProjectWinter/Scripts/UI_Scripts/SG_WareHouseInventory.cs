using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SG_WareHouseInventory : MonoBehaviour
{
    // 23.09.10 �Ʒ� ��ũ��Ʈ�� �÷��̾��� �κ��丮�� ����ִ� ��ũ��Ʈ�� Clone ����
    // ���� â�� �°� �����ؼ� ����ؾ���

    // 23.09.10 �ѹ� Ȯ�� �غ��� �ּ��� �޾Ƴ�����
    // (1) : â�� �Ǻ��Ұ� �÷��̾ Ray�� ��Ƽ� Ȯ������ �����ؾ���
    // (2) : ����â���� Destroy�Լ� ������ �ʿ��� �÷��̾��� �κ��丮���� ������ ���־����
    // (3) : â�� Open �� Close �� �Ʒ��� TryOpenInventory �Լ��� �̿��ؼ� ����ϸ� �ɰŰ���

    // �κ��丮�� ������������ �ٸ� ����� �������� ���Ǵ� bool ����
    // �÷��̾�� ���� ģ������ ����ؾ��ϱ⿡ static���� ����
    // ������ �κ��丮�� ProjcetWinter �� �κ��丮�� ������ �ٸ� �ൿ�� ������ �Ǿ��Ҷ���
    // ũ������,â����,�����ⰰ������ ��Ḧ ���������̱⿡ ���� �ٲ���� ���� 23.09.07

    public static bool inventoryActicated = false;


    // �ʿ��� ������Ʈ

    // �κ��丮 �� ���̽� (�׸����� �θ�ü)
    [SerializeField]
    private GameObject inventoryBase;
    // ���Ե��� �θ�ü -> �׸���
    [SerializeField]
    private GameObject slotsParent;
    [SerializeField]
    private GameObject warehouseTextObj;

    // â���� ������ �迭�� ����
    private SG_WareHouseItemSlot[] slots;

    // �κ��丮���� �ֿ����� Ȯ���� PlayerAction Class���ִ� Distroy�����Ű�� �������θ����

    // ������ Distroy �̺�Ʈ�� ���� ��������Ʈ ����
    public delegate void ItemDestroyDelegate();

    // event�� ��������Ʈ�� �����ؼ� ��������Ʈ�� ������ �ִ� void �� ���ϰ��� �Ű������� ������Ѵٴ� �������־��ػ��̵�
    public event ItemDestroyDelegate ItemDestroyEvent;



    private void Awake()
    {

    }

    void Start()
    {
        slots = slotsParent.GetComponentsInChildren<SG_WareHouseItemSlot>();        
    }

    void Update()
    {
        //TryOpenInventory();
    }

    // { ProjectWiter ���ӿ��� �ʿ����� �ʴ� ���
    // I��ư�� ���������� �κ��丮 ������ ���
    // ProjectWinter�� �� ����� �ʿ�������� ����

    // 23.09.10 ������ â��� EŰ�� ������ ������ϱ⿡ �Ʒ��� �Լ��� ������ �߰��ؼ� �� ����ϸ� �����Ұ����� ����
    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            inventoryActicated = !inventoryActicated;

            if (inventoryActicated)
            {                
                OpenInventory();
            }
            else
            {
                CloseInventory();
            }
        }
    }

    private void OpenInventory()
    {
        inventoryBase.SetActive(true);
        warehouseTextObj.SetActive(true);
    }

    private void CloseInventory()
    {
        inventoryBase.SetActive(false);
        warehouseTextObj.SetActive(false);
    }
    
    // } ProjectWiter ���ӿ��� �ʿ����� �ʴ� ���

    // { AcquireItem()
    public void AcquireItem(SG_Item _item, int _count = 1)
    {
        // ���� �������� ItemType�� Weapon �� �ƴҰ�쿡�� ���� ���� ���� ���� ����
        if (SG_Item.ItemType.Weapon != _item.itemType)
        {
            // �������� �ѹ� �� �Ⱦ�� ���� �������� �ִٸ� �������� ��������
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        if (slots[i].itemCount == 3)
                        {
                            Debug.Log("3�� �̻��� �ֿ�� ����");
                            continue;
                        }
                        slots[i].SetSlotCount(_count);
                        // TODO : Distroy ���� �������� Distroy �ϵ��� �߰��ؾ���
                        ItemDestroyEventShot();
                        return;
                    }
                }
            }
        }
        else { /*PASS*/ }


        // �κ��丮�� ���� �������� ���ٸ� ������ �߰�
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                // TODO : Distroy ���� �������� Distroy �ϵ��� �߰��ؾ���
                ItemDestroyEventShot();
                return;
            }
            else { Debug.Log("�κ��丮�� ������� ����"); }
        }
    }   // } AcquireItem()

    // â������ Destroy�� �������ִ°��̾ƴ� �÷��̾��� â���� �ű�°��̱⿡ �Ʒ� �κ� ������ �ʿ��Ұ����� ����
    public void ItemDestroyEventShot()
    {
        // �� �Լ��� �θ��� ItemDistroyEvent�� �����ϰ� �ִ� ��� �Լ��� ȣ����
        ItemDestroyEvent?.Invoke();
    }

}   //NAMESPACE
