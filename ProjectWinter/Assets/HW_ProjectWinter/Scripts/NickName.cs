using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NickName : MonoBehaviour
{

    private Camera mainCamera; // 메인 카메라에 대한 참조

    private void Start()
    {
        mainCamera = Camera.main; // 메인 카메라를 찾아서 저장
    }

    private void Update()
    {
        // 항상 카메라 방향으로 회전
        if (mainCamera != null)
        {
            // UI를 항상 카메라를 향하도록 회전
            transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                mainCamera.transform.rotation * Vector3.up);
        }
    }
}
