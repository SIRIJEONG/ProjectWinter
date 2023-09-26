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


    private void StartInIt()    // Start지점에서 실행될 함수
    {
        bunkerBoxObjs.SetActive(false);
        playerActionClass = FindAnyObjectByType<SG_PlayerActionControler>();
        EventSubscriber();      //이벤트 구독하는 함수
    }

    private void EventSubscriber()
    {
        playerActionClass.BunkerBoxEvent += BunkerBoxInvenController;
    }

    public void BunkerBoxInvenController()
    {
        //Debug.Log("이벤트로 발전기 여는 함수 조건이 잘들어와지나");
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
