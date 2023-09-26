using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GhostController : MonoBehaviourPun
{
    private float aniSpeed = 0;

    private float horizontal;
    private float vertical;
    public int speed = 10; 
    private Vector3 moveVec;
    private Animator animator;

    private CameraFollow cameraFollow;
    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        if (photonView.IsMine)
        {
            CinemachineVirtualCamera followCam = FindObjectOfType<CinemachineVirtualCamera>();
            followCam.LookAt = transform;

            cameraFollow = player.GetComponent<CameraFollow>();
            cameraFollow.ghostController = this;
            //followCam.LookAt = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        GhostMove();
    }

    private void GhostMove()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        moveVec = new Vector3(horizontal, 0, vertical).normalized;

        transform.position += moveVec * speed * Time.deltaTime;

        transform.LookAt(transform.position + moveVec);
        // ��ǥ�̵�

        // �ִϸ��̼�
        if (Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0)
        {
            aniSpeed = Mathf.MoveTowards(aniSpeed, 1, Time.deltaTime * 3);
        }
        else
        {
            aniSpeed = Mathf.MoveTowards(aniSpeed, 0, Time.deltaTime * 3);
        }
        animator.SetFloat("Speed", aniSpeed);
    }

    // �ǳ� ����
    #region
    // ###########################
    // isMine�϶��� ����
    // ###########################

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Building"))           // �÷��̾ �ǹ� ������ ������
        {
            cameraFollow.inside = other.gameObject;
            cameraFollow.isInside = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Building"))
        {
            cameraFollow.isInside = false;
        }
    }
    #endregion
    // �ǳ� ����
}
