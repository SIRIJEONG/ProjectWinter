using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NickName : MonoBehaviour
{

    private Camera mainCamera; // ���� ī�޶� ���� ����

    private void Start()
    {
        mainCamera = Camera.main; // ���� ī�޶� ã�Ƽ� ����
    }

    private void Update()
    {
        // �׻� ī�޶� �������� ȸ��
        if (mainCamera != null)
        {
            // UI�� �׻� ī�޶� ���ϵ��� ȸ��
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                mainCamera.transform.rotation * Vector3.up);
        }
    }
}
