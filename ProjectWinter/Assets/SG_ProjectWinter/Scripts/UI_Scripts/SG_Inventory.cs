using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.Progress;

public class SG_Inventory : MonoBehaviour
{
    // �κ��丮�� ������������ �ٸ� ����� �������� ���Ǵ� bool ����
    // �÷��̾�� ���� ģ������ ����ؾ��ϱ⿡ static���� ����
    // ������ �κ��丮�� ProjcetWinter �� �κ��丮�� ������ �ٸ� �ൿ�� ������ �Ǿ��Ҷ���
    // ũ������,â����,�����ⰰ������ ��Ḧ ���������̱⿡ ���� �ٲ���� ���� 23.09.07
    public static bool inventoryActicated = false;


    // �ʿ��� ������Ʈ

    // �κ��丮 �� ���̽� (������ �θ�ü)
    [SerializeField]
    private GameObject inventoryBase;
    // ���Ե��� �θ�ü
    [SerializeField]
    private GameObject slotsParent;

    // Swap���� slots �� ���������ϱ� ������ public
    public SG_ItemSlot[] slots;

    // �κ��丮���� �ֿ����� Ȯ���� PlayerAction Class���ִ� Distroy�����Ű�� �������θ����

    // ������ Distroy �̺�Ʈ�� ���� ��������Ʈ ����
    public delegate void ItemDestroyDelegate();

    // event�� ��������Ʈ�� �����ؼ� ��������Ʈ�� ������ �ִ� void �� ���ϰ��� �Ű������� ������Ѵٴ� �������־��ػ��̵�
    public event ItemDestroyDelegate ItemDestroyEvent;

    // �Ʒ� ������ �±׷� �÷��̾����� â������ �����Ұ���
    GameObject topParentObj;

    private void Awake()
    {

    }

    void Start()
    {
        slots = slotsParent.GetComponentsInChildren<SG_ItemSlot>();
        topParentObj = this.transform.parent.gameObject;
        // topParentObj������ �ֻ��� ���ӿ�����Ʈ�� ������� 
        GetThisTopParentObj();
        // ������ ������ȣ �־��ִ� �Լ�
        InItSlotCount();

    }
    void Update()
    {
        //TryOpenInventory();
    }

    /*LEGACY

    // { ProjectWiter ���ӿ��� �ʿ����� �ʴ� ���
    // I��ư�� ���������� �κ��丮 ������ ���
    // ProjectWinter�� �� ����� �ʿ�������� ����
    private void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
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
    }

    private void CloseInventory()
    {
        inventoryBase.SetActive(false);
    }
    // } ProjectWiter ���ӿ��� �ʿ����� �ʴ� ���

    LEGACY*/

    // { AcquireItem()
    public void AcquireItem(SG_Item _item, int _count = 1)
    {
        // �������� �ֿ�� �������Լ������� �÷��̾ �ƴ϶�� �ٷ� ������ ����
        if(!topParentObj.CompareTag("Player"))
        {
            return;
        }

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
                            //Debug.Log("3�� �̻��� �ֿ�� ����");
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
                ItemDestroyEventShot();
                return;
            }
            else { /*Debug.Log("�κ��丮�� ������� ����");*/ }
        }
    }   // } AcquireItem()


    public void ItemDestroyEventShot()
    {
        // �� �Լ��� �θ��� ItemDistroyEvent�� �����ϰ� �ִ� ��� �Լ��� ȣ����
        ItemDestroyEvent?.Invoke();
    }


    private void GetThisTopParentObj()
    {
        //�ֻ��� �θ� ������Ʈ �±׸� �������� ���� ã�� ����
        while (topParentObj.transform.parent != null)
        {
            topParentObj = topParentObj.transform.parent.gameObject;
        }
    }

    private void InItSlotCount()
    {
        // �÷��̾� �κ��丮 �϶��� �÷��̾��� ������ȣ �־���
        if (topParentObj.CompareTag("Player"))
        {
            // ó���� �����ϸ� ���Կ� ������ȣ �־���    10 ~ 13
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].slotCount = i + 10;
            }
        }
        // ����â�� �κ��丮 �϶��� ����â���� ������ȣ �־��� 100 ~ 115 
        else if (topParentObj.CompareTag("Warehouse"))
        {
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].slotCount = i + 100;
            }
        }

        // ������ ������ȣ ���� ���� else if
        //else if(topParentObj.CompareTag(""))
        //{

        //}
    }


    // �������� �޾� �������� ������ Ȯ������ �Լ�
    // 23.09.14 ���� �ǵ��� �Լ��� �ƴѰŰ��� ���߿� Ʋ�� ��� �����Ұ�
    // { AcquireItem()
    public void AcquireMoveItem(SG_Item _item, int _count)
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
                            //Debug.Log("3�� �̻��� �ֿ�� ����");
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
                ItemDestroyEventShot();
                return;
            }
            else { /*Debug.Log("�κ��丮�� ������� ����");*/ }
        }
    }   // } AcquireItem()

}   //NAMESPACE
