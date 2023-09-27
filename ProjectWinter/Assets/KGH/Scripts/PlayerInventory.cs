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

    }

    //-----------------------------------------------------------------------------------------------------
    private void StartInIt()
    {
        GetTopParent();

        handChildTrans = transform;

        mouseScroll = topParentTrans.GetComponent<MouseScroll>();

        playercontroller = topParentTrans.GetComponent<PlayerController>();

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


    public void InItNowSlotNum() // ���� ������ �������� �̺�Ʈ�� ȣ������Լ�
    {
        if (!photonView.IsMine)
        { return; }
        slotNum = mouseScroll.slot;

        CheckHand();
    }

    private void CheckHand()    // ���� �տ� ����� �ִ��� Ȯ���ϴ� �Լ�
    {
        handChildTrans = this.transform;
        //Debug.LogFormat("handChildTransCount ->  {0}", handChildTrans.childCount);
        if (handChildTrans.childCount > 0)  //�տ� ���� ��� �ִ°��
        {
            PhotonNetwork.Destroy(handItemClone);
            //handItemClone = null;
            foodInHand = false;
            weapomInHand = false;
            StartCoroutine(NowItemSlotInstance());
        }

        else if (handChildTrans.childCount == 0) //�տ� ��� �ִ� �������� ���°��
        {
            handItemClone = null;
            foodInHand = false;
            weapomInHand = false;
            StartCoroutine(NowItemSlotInstance());
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
                (playerinventory.slots[(int)slotNum - 1].item.handPrefab.name, transform.position, Quaternion.identity);

            Debug.Log("RPC 부르기 전까진 오는가?");

            photonView.RPC("ChangePositionItemScroll", RpcTarget.All);

            Debug.Log("RPC 부른후 오는가?");
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
        PhotonNetwork.Destroy(handItemClone);
    }
    public void MissItem()
    {
        playerinventory.slots[(int)slotNum - 1].itemCount -= 1;
        playerinventory.slots[(int)slotNum - 1].TextUpdate();
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
            transform.position, Quaternion.identity);
        //Rigidbody newRigidbody = handItemCopy.AddComponent<Rigidbody>();
        //handItemCopy.GetComponent<Collider>().enabled = true;
        photonView.RPC("ChangePositionItemDrop", RpcTarget.All);
        if (playerinventory.slots[(int)slotNum - 1].itemCount == 1)
        {
            PhotonNetwork.Destroy(handItemClone);
        }
        
    }

    [PunRPC]
    public void ChangePositionItemScroll()
    {
        //Debug.LogFormat("매개변수 정보 -> {0} , 현재 transform 정보 -> {1}",item.name,this.transform.name);
        handItemClone.transform.SetParent(transform);
        //Collider collider = handItemClone.GetComponent<Collider>();
        //Rigidbody itemRb = handItemClone.transform.GetComponent<Rigidbody>();
        //collider.enabled = false;               // �ݶ��̴� ������Ʈ ����
        //Destroy(itemRb);                        // ������ٵ� ���� ( �� ������� �ϱ� ����)
        handItemClone.transform.localPosition = Vector3.zero;
        handItemClone.transform.localRotation = Quaternion.identity;
    }

    [PunRPC]
    public void ChangePositionItemDrop()
    {
        handItemCopy.transform.SetParent(transform);
        handItemCopy.transform.localPosition = Vector3.zero;
        handItemCopy.transform.localRotation = Quaternion.identity;
        handItemCopy.transform.SetParent(null);
    }
}

// ------------------------------------------------------------------------------------------------------