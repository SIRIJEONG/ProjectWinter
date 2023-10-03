using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class SG_PlayerActionControler : MonoBehaviourPun
{

    [SerializeField]
    private float range;    //���� �Ÿ�

    private bool pickupActivated = false;   //���� ������ �� true

    private RaycastHit hitInfo; // �浹ü ����

    // ������ ���̾�� ���� �ϵ��� ���̾� ����ũ ����
    [SerializeField]
    private LayerMask itemLayerMask;
    // ���� �Ͽ콺�� �ƴϸ� ������ ������� ���ϰ� ���� LayerMask ����
    [SerializeField]
    private LayerMask whreHouseMask;


    // �ʿ��� ������Ʈ
    [SerializeField]
    private TextMeshProUGUI actionText;

    [SerializeField]
    private SG_Inventory theInventory;

    //â��
    // â���� ��������  �÷��̾��� �̺�Ʈ ���� â���� �ν��ϸ� â���� ���� �ݰ� ���� �̺�Ʈ ��������Ʈ
    public delegate void WareHouseDelegate();
    public event WareHouseDelegate WareHouseEvent;

    // ������
    public delegate void PowerStationInventoryDelegate();
    // event�� ��������Ʈ�� �����ؼ� ��������Ʈ�� ������ �ִ� void �� ���ϰ��� �Ű������� ������Ѵٴ� �������־��ػ��̵�
    public event PowerStationInventoryDelegate PowerStationInventoryEvent;

    // �︮���
    public delegate void HeliPadInventoryDelegate();
    public event PowerStationInventoryDelegate HeliPadInventoryEvent;

    // ���۴�
    public delegate void WorkStationOpenDelegate();
    public event WorkStationOpenDelegate WorkStationOpenEvent;

    // �ֹ�
    public delegate void KitchenOpenDelegate();
    public event KitchenOpenDelegate KitchenOpenEvent;

    public event System.Action<SG_Inventory> tossInventoryEvent;

    // ��Ŀ�ڽ�
    public event System.Action<int> BunkerBoxEvent;

    private SG_Inventory boxInventoryClass;
    //public delegate int BunkerBoxDelegate();
    //public event BunkerBoxDelegate BunkerBoxEvent;

    //public SG_Inventory inventoryClass;

    private void Awake()
    {

    }

    void Start()
    {
        theInventory.ItemDestroyEvent += ItemDestroy;
    }

    // Update is called once per frame
    void Update()
    {
        CheckItem();
        CheckOpenObj();
        //TryAction();
        //TESTTryAction();

    }

    public void TryAction()
    {
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        CheckItem();
        CanPickUp();
        //}
    }

    public void TESTTryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            CanPickUp();
        }
    }

    // ���̸� ��Ƽ� �������� Item �̶�� Layer�� ������ �ִٸ�����ϴ� ����
    private void CheckItem()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 1.0f, itemLayerMask))
        // if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, itemLayerMask))
        {
            if (hitInfo.transform.CompareTag("Item"))
            {
                ItemInfoAppear();
            }
        }
        else { InfoDisappear(); }
    }


    // ���̸� ��Ƽ� �������̶�� ���̾� ����ũ�� ������ �������̸� E Ű�� ������ ��� text ����ϴ� �Լ�
    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<SG_ItemPickUp>().item.itemName
            + " Get Input Key E";
    }

    // ������ �ݰ����� E �� ������� text ���� �Լ�
    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }

    // �������� �κ��丮�� �ִ� �Լ�

    // �����Ͱ� ������ҵ�?
    public void CanPickUp()
    {
        //if (pickupActivated == true)
        //{
        if (hitInfo.transform != null)
        {
            //Debug.Log("������ ȹ��");
            theInventory.AcquireItem(hitInfo.transform.GetComponent<SG_ItemPickUp>().item);
            //Destroy(hitInfo.transform.gameObject);
            InfoDisappear();

        }
        //}
    }

    // TODO : �� �Լ��� �ߵ� �Ǿ������� �κ��丮���� ����ִ°��� ã�� �ִµ� ������ �������� Ȯ���ϰ�
    //   �������� �� ������ �κ��丮�� ActionControler�� Destroy�Լ��� �ߵ����ְ� ���� ���ߴٸ� return
    //      �ϵ��� �������߰���

    // �������� �Ծ��ٸ� Distroy ���� �Լ�      // Photon Destroy�� �����ؾ���
    public void ItemDestroy()
    {
        PhotonNetwork.Destroy(hitInfo.transform.gameObject);
    }

    // ---------------------------------- �κ��丮 Open �Լ� -----------------------------------

    private void CheckOpenObj()
    {
        if (Physics.Raycast
        (transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range))
        {

            if (hitInfo.transform.CompareTag("Warehouse"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //Debug.Log("���̸� ��� â���� ������ E �� �������Ѵٴ� ������ �ߵ�����?");
                    // ����â�� ���� �ݴ� �Լ� �θ���
                    WareHouseEvent?.Invoke();
                }
            }
            else { /*PASS*/ }

            //if (GameManager.instance.isRepairPowerStation == false)   // �̼� ������ ������ ������ �� if ��
            //{

            if (hitInfo.transform.CompareTag("PowerStation"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    PowerStationInventoryEvent?.Invoke();
                }
            }
            else { /*PASS*/ }
            //}   // �̼� ������ ������ ������ �� if ��

            //if (GameManager.instance.isRepairHeliPad == false)  // �̼� ������ �︮�е� ������ �� if ��
            //{
            if (hitInfo.transform.CompareTag("HeliPad"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    HeliPadInventoryEvent?.Invoke();
                }
            }
            else { /*PASS*/ }
            //}   // �̼� ������ �︮�е� ������ �� if ��

            if (hitInfo.transform.CompareTag("Workstation"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    WorkStationOpenEvent?.Invoke();
                    //Invoke�ѵڿ� action event�� �ҷ����� �ɵ�
                    tossInventoryEvent?.Invoke(theInventory);
                }
            }
            else { /*PASS*/ }

            if (hitInfo.transform.CompareTag("Kitchen"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    KitchenOpenEvent?.Invoke();
                    tossInventoryEvent?.Invoke(theInventory);
                }
                else { /*PASS*/ }

            }

            if(hitInfo.transform.CompareTag("Box"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
              
                    boxInventoryClass = hitInfo.transform.GetChild(0).GetChild(0).GetComponent<SG_Inventory>();

                    Debug.LogFormat("BoxInvenClassNull? -> {0},     WhatName? -> {1}", boxInventoryClass == null, boxInventoryClass);
                    Debug.LogFormat("thisBoxIndex -> {0}", boxInventoryClass.thisBoxindex);
                    BunkerBoxEvent?.Invoke(boxInventoryClass.thisBoxindex);

                }
                else { /*PASS*/ }
            }
            else { /*PASS*/ }


        }   // Ray IF

    }   //CheckOpenObj()

}   // NAMESPACE
