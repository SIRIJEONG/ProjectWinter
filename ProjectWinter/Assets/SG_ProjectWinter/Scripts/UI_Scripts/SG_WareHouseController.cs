using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_WareHouseController : MonoBehaviour
{
    [SerializeField]
    private GameObject warehouseObjs;

    private bool isOpen = false;

    SG_PlayerActionControler playerActionClass;

    public delegate void warehouseInventoryDelegate();

    // event를 델리게이트를 지정해서 델리게이트가 가지고 있는 void 의 리턴값과 매개변수가 없어야한다는 조건을넣어준샘이됨
    public event warehouseInventoryDelegate warehouseInventoryEvent;

    private void Start()
    {
        warehouseObjs.SetActive(false);
        playerActionClass = FindAnyObjectByType<SG_PlayerActionControler>();
        playerActionClass.WareHouseEvent += WareHouseInvenController;
    }

    public void WareHouseInvenController()
    {
        //Debug.Log("이벤트로 창고 여는 함수 조건이 잘들어와지나");
        if (isOpen == false)
        {
            OpenWareHouse();
        }
        else if (isOpen == true)
        {
            CloseWareHouse();
        }
    }

    private void OpenWareHouse()
    {
        isOpen = true;
        warehouseObjs.SetActive(true);
    }
    private void CloseWareHouse()
    {
        isOpen = false;
        warehouseObjs.SetActive(false);
    }


}
