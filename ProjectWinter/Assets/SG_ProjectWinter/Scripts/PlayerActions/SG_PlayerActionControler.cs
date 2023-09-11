using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SG_PlayerActionControler : MonoBehaviour
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

    // â�� ��������  �÷��̾��� �̺�Ʈ ���� â�� �ν��ϸ� â�� ���� �ݰ� ���� �̺�Ʈ ��������Ʈ
    public delegate void WareHouseDelegate();

    public event WareHouseDelegate WareHouseEvent;


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
        CheckWarehouse();
        TryAction();

    }

    private void TryAction()
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
        if (Physics.Raycast
            (transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, itemLayerMask))
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

    // ������ �ݰ��� E �� ������� text ���� �Լ�
    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }

    // �������� �κ��丮�� �ִ� �Լ�
    private void CanPickUp()
    {
        if (pickupActivated == true)
        {
            if (hitInfo.transform != null)
            {
                //Debug.Log("������ ȹ��");
                theInventory.AcquireItem(hitInfo.transform.GetComponent<SG_ItemPickUp>().item);
                //Destroy(hitInfo.transform.gameObject);
                InfoDisappear();

            }
        }
    }

    // TODO : �� �Լ��� �ߵ� �Ǿ������� �κ��丮���� ����ִ°��� ã�� �ִµ� ������ �������� Ȯ���ϰ�
    //   �������� �� ������ �κ��丮�� ActionControler�� Destroy�Լ��� �ߵ����ְ� ���� ���ߴٸ� return
    //      �ϵ��� �������߰���

    // �������� �Ծ��ٸ� Distroy ���� �Լ�
    public void ItemDestroy()
    {
        Destroy(hitInfo.transform.gameObject);
    }

    // ---------------------------------- ���� �Ͽ콺 �Լ� -----------------------------------

    private void CheckWarehouse()
    {
        if (Physics.Raycast
        (transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, whreHouseMask))
        {
            if (hitInfo.transform.CompareTag("Warehouse"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //Debug.Log("���̸� ��� â�� ������ E �� �������Ѵٴ� ������ �ߵ�����?");
                    // ����â�� ���� �ݴ� �Լ� �θ���
                    WareHouseEvent?.Invoke();
                }
            }
        }
    }

}   // NAMESPACE
