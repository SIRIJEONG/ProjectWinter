using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

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



    private void Awake()
    {

    }

    void Start()
    {
        slots = slotsParent.GetComponentsInChildren<SG_ItemSlot>();

        // ó���� �����ϸ� ���Կ� ������ȣ �־���
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].slotCount = i + 10;
        }

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

                    // ���ҽ� �ڽ��� ���° �迭���� �˷��ֱ� ���� ����
                    slots[i].slotCount = i + 10;

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

    }   //NAMESPACE
