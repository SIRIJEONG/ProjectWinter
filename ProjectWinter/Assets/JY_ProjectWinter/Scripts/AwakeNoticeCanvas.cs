using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AwakeNoticeCanvas : MonoBehaviour
{
    // �˸� UI ������Ʈ
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
            // �÷��̾��� ���� ���� ����
            Vector3 playerForward = other.transform.forward;
            // �÷��̾�� ������Ʈ ������ ���� ����
            Vector3 objectDirection = (transform.position - other.transform.position).normalized;

            // ���� ������ ����Ͽ� ������ ���Ѵ�.
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
