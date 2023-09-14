using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeNoticeCanvas : MonoBehaviour
{
    // 알림 UI 오브젝트
    public GameObject noticeCanvas;

    // Start is called before the first frame update
    void Start()
    {
        noticeCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AppearUi()
    {
        noticeCanvas.SetActive(true);
    }

    public void DisappearUi()
    {
        noticeCanvas.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            // 플레이어의 정면 방향 벡터
            Vector3 playerForward = other.transform.forward;
            // 플레이어와 오브젝트 사이의 방향 벡터
            Vector3 objectDirection = (transform.position - other.transform.position).normalized;

            // 둘의 내적을 계산하여 각도를 구한다.
            float dotProduct = Vector3.Dot(playerForward, objectDirection);

            if(dotProduct > 0.5f && noticeCanvas.activeSelf == false)
            {
                AppearUi();
            }
            else if(dotProduct < 0.5f && noticeCanvas.activeSelf == true)
            {
                DisappearUi();
            }
        }
    }
}
