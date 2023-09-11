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

    // event�� ��������Ʈ�� �����ؼ� ��������Ʈ�� ������ �ִ� void �� ���ϰ��� �Ű������� ������Ѵٴ� �������־��ػ��̵�
    public event warehouseInventoryDelegate warehouseInventoryEvent;

    private void Start()
    {
        warehouseObjs.SetActive(false);
        playerActionClass = FindAnyObjectByType<SG_PlayerActionControler>();
        playerActionClass.WareHouseEvent += WareHouseInvenController;
    }

    public void WareHouseInvenController()
    {
        //Debug.Log("�̺�Ʈ�� â�� ���� �Լ� ������ �ߵ�������");
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
