using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform toFallow;

    private float distance = 8.0f; 
    private float height = 11.0f;

    public bool isInside = false;
    public GameObject inside;

    private Vector3 offset = new Vector3(0.0f, 8.0f, -11.0f);

    public PlayerController playerController;
    public GhostController ghostController;
    private GameObject player;

    Coroutine coroutine;
    // Start is called before the first frame update
    void Start()
    {
        coroutine = StartCoroutine(CameraSet());               
    }

    // Update is called once per frame
    void Update()
    {
        
        if (ghostController != null)
        {
            player = ghostController.gameObject;

        }
        if (!isInside)
        {
            toFallow = player.transform;
            Vector3 targetPosition = toFallow.position + offset;

            transform.position = targetPosition;
        }
        else
        {
            Transform cameraObject = inside.transform.Find("Inside Camera");    // Inside Camera = 건물 안에 들어갔을떄 고정시킬 카메라 위치에 둘 오브잭트의 이름
            GameObject moveCameraHere = cameraObject.gameObject;
            toFallow = moveCameraHere.transform;

            transform.position = toFallow.position;
        }
    }

    IEnumerator CameraSet()
    {
        yield return new WaitForSeconds(0.3f);
        player = playerController.gameObject;
    }
}