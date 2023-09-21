using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviourPun
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
    private CinemachineVirtualCamera followCam;

    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            followCam = FindObjectOfType<CinemachineVirtualCamera>();
            followCam.LookAt = transform;
        }
        player = transform.gameObject;
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

            followCam.transform.position = targetPosition;
        }
        else
        {
            Transform cameraObject = inside.transform.Find("Inside Camera");    // Inside Camera = �ǹ� �ȿ� ������ ������ų ī�޶� ��ġ�� �� ������Ʈ�� �̸�
            GameObject moveCameraHere = cameraObject.gameObject;
            toFallow = moveCameraHere.transform;

            followCam.transform.position = toFallow.position;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
        { return; }
        if (other.CompareTag("Building"))           // �÷��̾ �ǹ� ������ ������
        {
            inside = other.gameObject;
            isInside = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!photonView.IsMine)
        { return; }
        if (other.CompareTag("Building"))
        {            
            isInside = false;
        }
    }
}