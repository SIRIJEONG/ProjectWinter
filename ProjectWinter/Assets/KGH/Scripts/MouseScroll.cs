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
        if (!playerController.doSomething && !playerController.eat && !playerController.isAttack)
        {

            if (scrollInput > 0f)
            {
                float verticalMovement = scrollInput * 10;

                slot += verticalMovement;
                invenCheck();

                scrollEvent?.Invoke();
            }
            else if (scrollInput < 0f)
            {
                float verticalMovement = scrollInput * 10;

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
