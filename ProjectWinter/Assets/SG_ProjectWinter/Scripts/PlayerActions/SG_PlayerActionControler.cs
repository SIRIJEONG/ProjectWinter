using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SG_PlayerActionControler : MonoBehaviour
{

    [SerializeField]
    private float range;    //습득 거리

    private bool pickupActivated = false;   //습득 가능할 때 true

    private RaycastHit hitInfo; // 충돌체 저장

    // 아이템 레이어에만 반응 하도록 레이어 마스크 설정
    [SerializeField]
    private LayerMask layerMask;


    // 필요한 컴포넌트
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
                //Debug.Log("아이템 획득");
                theInventory.AcquireItem(hitInfo.transform.GetComponent<SG_ItemPickUp>().item);
                //Destroy(hitInfo.transform.gameObject);
                InfoDisappear();
                
            }
        }
    }

    // TODO : 위 함수가 발동 되었을때에 인벤토리에서 비어있는곳을 찾아 넣는데 들어갔는지 못들어갔는지 확인하고
    //   아이템이 잘 들어갔으면 인벤토리가 ActionControler의 Destroy함수를 발동해주고 들어가지 못했다면 return
    //      하도록 만들어봐야겠음

    // 아이템을 먹었다면 Distroy 해줄 함수
    public void ItemDestroy()
    {
        Destroy(hitInfo.transform.gameObject);
    }

}   // NAMESPACE
