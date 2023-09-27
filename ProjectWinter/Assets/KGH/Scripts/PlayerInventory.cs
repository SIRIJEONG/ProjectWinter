using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;
using Photon.Pun;
using static SG_Item;
//using static UnityEditor.Progress;

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

    public float slotNum = 1;   // 현재 슬롯이 무었인지 체크할 변수

    public Transform handChildTrans;   // 자식오브젝트가 존재하는지 판별해줄 Transform

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


    public void InItNowSlotNum() // 현재 슬롯이 무엇인지 이벤트로 호출당할함수
    {
        if (!photonView.IsMine)
        { return; }
        slotNum = mouseScroll.slot;

        CheckHand();
    }

    private void CheckHand()    // 현재 손에 든것이 있는지 확인하는 함수
    {
        handChildTrans = this.transform;

        if (handChildTrans.childCount > 0)  //손에 무언가 들고 있는경우
        {
            DestroyImmediate(handItemClone);
            handItemClone = null;
            foodInHand = false;
            weapomInHand = false;
            StartCoroutine(NowItemSlotInstance());
        }

        else if (handChildTrans.childCount == 0) //손에 들고 있는 아이템이 없는경우
        {
            handItemClone = null;
            foodInHand = false;
            weapomInHand = false;
            StartCoroutine(NowItemSlotInstance());
        }

        else { Debug.Log("뭔가 잘못되었다"); }
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
            handItemClone = Instantiate(playerinventory.slots[(int)slotNum - 1].item.handPrefab);
            handItemClone.transform.SetParent(this.transform);

            handItemClone.transform.localPosition = Vector3.zero;
            handItemClone.transform.localRotation = Quaternion.identity;

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
    public void Drop()
    {
        Debug.LogFormat("slotNum  {0}", playerinventory.slots[(int)slotNum]);
        handItemCopy = Instantiate(playerinventory.slots[(int)slotNum - 1].item.itemPrefab);

        handItemCopy.transform.SetParent(this.transform);
        handItemCopy.transform.localPosition = Vector3.zero;
        handItemCopy.transform.localRotation = Quaternion.identity;
        Rigidbody newRigidbody = handItemCopy.AddComponent<Rigidbody>();

        handItemCopy.transform.SetParent(null);
        if(playerinventory.slots[(int)slotNum - 1].itemCount == 1)
        {
            Destroy(handItemClone);
        }
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
}

    // ------------------------------------------------------------------------------------------------------