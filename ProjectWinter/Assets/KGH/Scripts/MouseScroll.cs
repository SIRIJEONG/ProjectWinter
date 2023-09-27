using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class MouseScroll : MonoBehaviour
{
    public float slot = 1;

    public delegate void ScrollDelegate();

    public event ScrollDelegate scrollEvent;

    private PlayerController playerController;
    void Start()
    {
        playerController = transform.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        int verticalMovement = Mathf.RoundToInt(scrollInput * 10); // 휠 움직임에 따라 정수값으로 변환합니다.

        if (!playerController.doSomething && !playerController.eat && !playerController.isAttack)
        {
            if (verticalMovement != 0) // 0이 아닌 정수값을 검사합니다.
            {
                slot += verticalMovement;
                invenCheck();

                scrollEvent?.Invoke();
            }
        }
    }

    private void invenCheck()
    {
        if (slot < 1)
        {
            slot = 4;
        }
        else if (slot > 4)
        {
            slot = 1;
        }
    }
}
