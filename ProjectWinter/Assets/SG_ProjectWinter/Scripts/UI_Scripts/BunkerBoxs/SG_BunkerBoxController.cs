using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SG_BunkerBoxController : MonoBehaviour
{
    [SerializeField]
    private GameObject bunkerBoxObjs;

    private bool isOpen = false;

    SG_PlayerActionControler playerActionClass;


    private void Awake()
    {
      
    }
    private void Start()
    {
        StartInIt();
    }


    private void StartInIt()    // Start�������� ����� �Լ�
    {
        bunkerBoxObjs.SetActive(false);
        playerActionClass = FindAnyObjectByType<SG_PlayerActionControler>();
        EventSubscriber();      //�̺�Ʈ �����ϴ� �Լ�
    }

    private void EventSubscriber()
    {
        playerActionClass.BunkerBoxEvent += BunkerBoxInvenController;
    }

    public void BunkerBoxInvenController()
    {
        //Debug.Log("�̺�Ʈ�� ������ ���� �Լ� ������ �ߵ�������");
        if (isOpen == false)
        {
            OpenBunkerBox();
        }
        else if (isOpen == true)
        {
            CloseBunkerBox();
        }
    }

    private void OpenBunkerBox()
    {
        isOpen = true;
        bunkerBoxObjs.SetActive(true);

    }
    private void CloseBunkerBox()
    {
        isOpen = false;
        bunkerBoxObjs.SetActive(false);
    }
}
