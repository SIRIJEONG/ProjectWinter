using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SG_PlayerMove : MonoBehaviour
{

    private Rigidbody rigid;
    private float moveSpeed = 5f;


    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        // �÷��̾��� ������ �Է��� ����
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // ������ ���� ���
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput);
        moveDirection.Normalize(); // ���� ����ȭ

        // �÷��̾ ������
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }

}
