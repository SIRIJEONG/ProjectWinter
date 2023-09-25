using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_PowerStationControler : MonoBehaviour
{

    [SerializeField]
    private GameObject poweStationObjs;

    private bool isOpen = false;
        
    SG_PlayerActionControler playerActionClass;

    Transform topParentTrans;

    private void Start()
    {
        FirstInIt();
    }

    private void FirstInIt()
    {
        poweStationObjs.SetActive(false);
        playerActionClass = FindAnyObjectByType<SG_PlayerActionControler>();
        SerchTopParentTrans();  //�ֻ��� �θ� ã���ִ� �Լ�
        EventSubscriber();  // �ֻ��� �θ� ���� �����ϴ� �̺�Ʈ�� �������� �Լ�

    }
    private void SerchTopParentTrans()  //�ֻ��� �θ� ã���ִ� �Լ�
    {
        topParentTrans = transform;

        while(topParentTrans.parent != null)
        {
            topParentTrans = topParentTrans.parent;
        }
    }

    private void EventSubscriber()
    {
        if(topParentTrans.CompareTag("PowerStation"))
        {
            playerActionClass.PowerStationInventoryEvent += PowerStationInvenController;
        }
        else if(topParentTrans.CompareTag("HeliPad"))
        {
            playerActionClass.HeliPadInventoryEvent += PowerStationInvenController;
        }
    }

    public void PowerStationInvenController()
    {
        //Debug.Log("�̺�Ʈ�� ������ ���� �Լ� ������ �ߵ�������");
        if (isOpen == false)
        {
            OpenPowerStation();
        }
        else if (isOpen == true)
        {
            ClosePowerStation();
        }
    }

    private void OpenPowerStation()
    {
        isOpen = true;
        poweStationObjs.SetActive(true);
     
    }
    private void ClosePowerStation()
    {
        isOpen = false;
        poweStationObjs.SetActive(false);
    }



}
