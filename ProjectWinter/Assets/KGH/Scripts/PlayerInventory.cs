using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;
using static SG_Item;

public class PlayerInventory : MonoBehaviour
{
    //public List<GameObject> inventory;

    private Transform topParentTrans;

    private PlayerController playercontroller;

    public SG_Item item;

    private MouseScroll mouseScroll;
    public SG_Inventory playerinventory;


    public float hp;
    public float cold;
    public float hunger;

    public float damage;

    public int invenNumber;

    public bool weapomInHand = false;
    public bool foodInHand = false;

    public float slotNum = 1;   // ���� ������ �������� üũ�� ����

    public Transform handChildTrans;   // �ڽĿ�����Ʈ�� �����ϴ��� �Ǻ����� Transform

    public GameObject handItemClone;

    public GameObject handItemCopy;

    public PlayerHealth health;


    //public bool 
    public void Start()
    {
        StartInIt();
    }

    private void Update()
    {
        //for(int i = 0; i < inventory.Count; i++)
        //{         
        //    if((int)inven - 1 == i)
        //    {
        //        inventory[i].SetActive(true);
        //        playercontroller.inven = inventory[i].transform;
        //        invenNumber = i;

        //        if(inventory[i].transform.GetChild(0) != null)
        //        {
        //            playercontroller.itemInHand = inventory[i].transform.GetChild(0);
        //        }

        //    }
        //    else
        //    {
        //        Debug.Log(i + 1);
        //        Debug.LogFormat("�κ� {0}", inven);

        //        inventory[i].SetActive(false);
        //    }            
        //}



        //if (playerinventory.slots[(int)slotNum].item != null)
        //{
        //    if (playerinventory.slots[(int)slotNum].item.itemType == ItemType.Weapon)
        //    {
        //        Damage();
        //        weapomInHand = true;
        //        foodInHand = false;
        //    }
        //    else if (playerinventory.slots[(int)slotNum].item.itemType == ItemType.Used)
        //    {
        //        Heal();
        //        foodInHand = true;
        //        weapomInHand = false;
        //    }
        //    else
        //    {
        //        weapomInHand = false;
        //        foodInHand = false;
        //    }
        //}
    }

    //-----------------------------------------------------------------------------------------------------
    private void StartInIt()
    {
        GetTopParent();

        handChildTrans = transform;

        mouseScroll = topParentTrans.GetComponent<MouseScroll>();

        playercontroller = topParentTrans.GetComponent<PlayerController>();

        //playerinventory = gameObject.GetComponent<SG_Inventory>();

        playercontroller.inventoryClass = playerinventory;

        mouseScroll.scrollEvent += InItNowSlotNum;

        health = topParentTrans.GetComponent<PlayerHealth>();
    }

    private void GetTopParent()
    {
        topParentTrans = transform;

        while (topParentTrans.parent != null)
        {
            topParentTrans = topParentTrans.parent;
        }
    }


    private void InItNowSlotNum() // ���� ������ �������� �̺�Ʈ�� ȣ������Լ�
    {
        slotNum = mouseScroll.slot;

        CheckHand();
    }

    private void CheckHand()    // ���� �տ� ����� �ִ��� Ȯ���ϴ� �Լ�
    {
        //handChildTrans = this.transform.GetChild(0);

        handChildTrans = this.transform;
        Debug.LogFormat("handChildTransCount ->  {0}", handChildTrans.childCount);
        if (handChildTrans.childCount > 0)  //�տ� ���� ��� �ִ°��
        {
            Destroy(handItemClone);
            NowItemSlotInstance();
        }

        else if (handChildTrans.childCount == 0) //�տ� ��� �ִ� �������� ���°��
        {
            NowItemSlotInstance();
        }

        else { Debug.Log("���� �߸��Ǿ���"); }


    }

    private void NowItemSlotInstance()
    {
        if (playerinventory.slots[(int)slotNum - 1].item != null)
        {
            handItemClone = playerinventory.slots[(int)slotNum - 1].item.itemPrefab;
        }
        else { /*PASS*/ }

        if (handItemClone != null)
        {
            handItemClone = Instantiate(playerinventory.slots[(int)slotNum - 1].item.itemPrefab);
            handItemClone.transform.SetParent(this.transform);

            Collider collider = handItemClone.GetComponent<Collider>();
            Rigidbody itemRb = handItemClone.transform.GetComponent<Rigidbody>();
            collider.enabled = false;               // �ݶ��̴� ������Ʈ ����
            Destroy(itemRb);                        // ������ٵ� ���� ( �� ������� �ϱ� ����)
            handItemClone.transform.localPosition = Vector3.zero;
            handItemClone.transform.localRotation = Quaternion.identity;
            //handItemClone.transform.localScale = Vector3.one;
        }
        else { /*PASS*/ }

    }
    public void Eat()
    {
        health.health += playerinventory.slots[(int)slotNum - 1].item.itemHealth;
        health.cold += playerinventory.slots[(int)slotNum - 1].item.itemWarmth;
        health.hunger += playerinventory.slots[(int)slotNum - 1].item.itemSatiety;
    }
    public void MissItem()
    {
        playerinventory.slots[(int)slotNum - 1].itemCount -= 1;
        if (playerinventory.slots[(int)slotNum - 1].itemCount <= 0)
        {
            playerinventory.slots[(int)slotNum - 1].DisconnectedItem();
        }

    }

    public void Drop()
    {

        handItemCopy = Instantiate(handItemClone);
        handItemCopy.transform.SetParent(this.transform);
        handItemCopy.transform.localPosition = Vector3.zero;
        handItemCopy.transform.localRotation = Quaternion.identity;
        Rigidbody newRigidbody = handItemCopy.AddComponent<Rigidbody>();
        handItemCopy.GetComponent<Collider>().enabled = true;

        handItemCopy.transform.SetParent(null);

        Destroy(handItemClone);
    }

    // ------------------------------------------------------------------------------------------------------

    private void Heal()
    {
        if (item.itemName == "Carrot")
        {
            hp = 10;
            cold = 5;
            hunger = 20;
        }
    }

    private void Damage()
    {
        if (item.name == "")
        {
            damage = 0;
        }
    }
}
