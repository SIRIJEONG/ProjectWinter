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
            playerActionClass.PowerStationInventoryEvent += PoweStationInvenController;
        }
        else if(topParentTrans.CompareTag("HeliPad"))
        {
            playerActionClass.HeliPadInventoryEvent += PoweStationInvenController;
        }
    }

    public void PoweStationInvenController()
    {
        //Debug.Log("�̺�Ʈ�� ������ ���� �Լ� ������ �ߵ�������");
        if (isOpen == false)
        {
            OpenPoweStation();
        }
        else if (isOpen == true)
        {
            ClosePoweStation();
        }
    }

    private void OpenPoweStation()
    {
        isOpen = true;
        poweStationObjs.SetActive(true);
    }
    private void ClosePoweStation()
    {
        isOpen = false;
        poweStationObjs.SetActive(false);
    }
}
