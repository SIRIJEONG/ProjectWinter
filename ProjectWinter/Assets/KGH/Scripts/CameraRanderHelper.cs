using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRanderHelper : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Canvas canvas = GetComponent<Canvas>();

        if (canvas != null)
        {
            // Canvas�� ������ ī�޶� �����մϴ�.
            canvas.renderMode = RenderMode.ScreenSpaceCamera;
            canvas.worldCamera = Camera.main; // �Ǵ� �ٸ� ���ϴ� ī�޶�� �����մϴ�.
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
