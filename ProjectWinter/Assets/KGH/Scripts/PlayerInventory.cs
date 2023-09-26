using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;
using Photon.Pun;
using static SG_Item;

public class PlayerInventory : MonoBehaviourPun
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
        //Debug.LogFormat("handChildTransCount ->  {0}", handChildTrans.childCount);
        if (handChildTrans.childCount > 0)  //�տ� ���� ��� �ִ°��
        {
            Destroy(handItemClone);
            //handItemClone = null;
            foodInHand = false;
            weapomInHand = false;
            StartCoroutine(NowItemSlotInstance());


            //NowItemSlotInstance();
        }

        else if (handChildTrans.childCount == 0) //�տ� ��� �ִ� �������� ���°��
        {
            handItemClone = null;
            foodInHand = false;
            weapomInHand = false;
            StartCoroutine(NowItemSlotInstance());

            //NowItemSlotInstance();
        }

        else { Debug.Log("���� �߸��Ǿ���"); }


    }

    private IEnumerator NowItemSlotInstance()
    {
        yield return null;

        if (playerinventory.slots[(int)slotNum - 1].item != null)
        {
            handItemClone = playerinventory.slots[(int)slotNum - 1].item.handPrefab;
        }
        else { /*PASS*/ }

        if (handItemClone != null && handChildTrans.childCount == 0)
        {
            handItemClone = PhotonNetwork.Instantiate
                (playerinventory.slots[(int)slotNum - 1].item.handPrefab.name,transform.position,Quaternion.identity);
            photonView.RPC("ChangePositionItem", RpcTarget.All, handItemClone);
            //Collider collider = handItemClone.GetComponent<Collider>();
            //Rigidbody itemRb = handItemClone.transform.GetComponent<Rigidbody>();
            //collider.enabled = false;               // �ݶ��̴� ������Ʈ ����
            //Destroy(itemRb);                        // ������ٵ� ���� ( �� ������� �ϱ� ����)
            //handItemClone.transform.localScale = Vector3.one;


            HandItemCheck();
        }
        else { /*PASS*/ }

    }

    
    public void Eat()
    {
        hp = playerinventory.slots[(int)slotNum - 1].item.itemHealth;
        cold = playerinventory.slots[(int)slotNum - 1].item.itemWarmth;
        hunger = playerinventory.slots[(int)slotNum - 1].item.itemSatiety;
        Destroy(handItemClone);

    }
    public void MissItem()
    {
        playerinventory.slots[(int)slotNum - 1].itemCount -= 1;
        if (playerinventory.slots[(int)slotNum - 1].itemCount <= 0)
        {
            playerinventory.slots[(int)slotNum - 1].DisconnectedItem();
        }
        foodInHand = false;
        weapomInHand = false;
    }

    private void HandItemCheck()
    {
        if (playerinventory.slots[(int)slotNum - 1].item.itemType == ItemType.Weapon)
        {
            damage = playerinventory.slots[(int)slotNum - 1].item.itemDamage;
            weapomInHand = true;
            foodInHand = false;
        }
        else if (playerinventory.slots[(int)slotNum - 1].item.itemType == ItemType.Used)
        {
            foodInHand = true;
            weapomInHand = false;
        }
        else
        {
            foodInHand = false;
            weapomInHand = false;
        }
    }

    public void Drop()
    {
        handItemCopy = PhotonNetwork.Instantiate(playerinventory.slots[(int)slotNum - 1].item.name,
            transform.position,Quaternion.identity);
        photonView.RPC("ChangePositionItem", RpcTarget.All, handItemCopy);
        //Rigidbody newRigidbody = handItemCopy.AddComponent<Rigidbody>();
        //handItemCopy.GetComponent<Collider>().enabled = true;
        photonView.RPC("SetParentNull", RpcTarget.All, handItemCopy);
        PhotonNetwork.Destroy(handItemClone);
    }

    [PunRPC]
    public void ChangePositionItem(GameObject item)
    {
        item.transform.SetParent(transform);
        //Collider collider = handItemClone.GetComponent<Collider>();
        //Rigidbody itemRb = handItemClone.transform.GetComponent<Rigidbody>();
        //collider.enabled = false;               // �ݶ��̴� ������Ʈ ����
        //Destroy(itemRb);                        // ������ٵ� ���� ( �� ������� �ϱ� ����)
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;
    }

    [PunRPC]
    public void SetParentNull(GameObject item)
    {
        item.transform.SetParent(null);
    }
}

    // ------------------------------------------------------------------------------------------------------