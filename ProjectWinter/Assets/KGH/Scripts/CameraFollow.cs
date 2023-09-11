using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform toFallow;

    public float distance = 5.0f; 
    public float height = 2.0f;

    public static bool isInside = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!isInside)
        {
            GameObject playerObject = GameObject.Find("Player");
            toFallow = playerObject.transform;
            Vector3 targetPosition = toFallow.position - toFallow.forward * distance + Vector3.up * height;

            transform.position = targetPosition;
        }
        else
        {
            GameObject cameraObject = GameObject.Find("�ǹ� �� ī�޶� ��ġ ������Ʈ�� �̸�");
            toFallow = cameraObject.transform;

            // ī�޶�� ������Ʈ�� �����ְ�
        }
    }
}
