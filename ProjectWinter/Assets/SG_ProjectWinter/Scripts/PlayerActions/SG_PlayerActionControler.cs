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
    private LayerMask layerMask;


    // �ʿ��� ������Ʈ
    [SerializeField]
    private TextMeshProUGUI actionText;

    [SerializeField]
    private SG_Inventory theInventory;


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
        TryAction();

    }

    private void TryAction()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            CanPickUp();
        }
    }

    private void CheckItem()
    {
        if (Physics.Raycast
            (transform.position, transform.TransformDirection(Vector3.forward), out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.tag == "Item")
            {
                ItemInfoAppear();
            }
        }
        else { InfoDisappear(); }
    }


    private void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<SG_ItemPickUp>().item.itemName
            + " Get Input Key E";
    }


    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }


    private void CanPickUp()
    {
        if(pickupActivated == true)
        {
            if(hitInfo.transform != null)
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

}   // NAMESPACE
