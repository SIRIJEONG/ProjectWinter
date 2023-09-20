using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviourPun
{
    private Rigidbody rb;
    private Animator animator;
    private float aniSpeed = 0;

    private float horizontal;
    private float vertical;
    public int speed = 5;

    private bool doSomething = false;

    // �÷��̾��� ����
    public static bool isSurvivor;
    public static bool isTrator;
    // �÷��̾��� ����

    public GameObject weapon;       // ���� ������� ���ݹ���
    public GameObject fist;         // �ָ��϶� ���ݹ���
    public GameObject ui;           // �Ŀ� ������ ui
    //public GameObject cameraObject;

    private bool hand = false;      // �ӽ� : �տ� ���� �������

    private RaycastHit hitInfo;
    private Vector3 moveVec;

    private float doingTime;    // �ൿ�� �� �ð� üũ
    private float startTime;
    private bool shouldStartTiming = false;
    private int doingCase;      // �� �ϴ� ��������

    private UiFallowPlayer uiFallowPlayer;

    private PlayerHealth health;    // ������ PlayerHealth

    // ���ݰ���
    private float attackPower;  // ���콺�� ���� ����
    private bool isAttack;
    public int damage;         // �� ������
    private bool eat = false;       // �ӽ÷� ������, ���߿� ������ ������ on, ȸ�� �� off�� ��Ȱ��
    // ���ݰ���

    private CameraFollow cameraFollow;

    public PlayerHealth playerHealth;   // ���� PlayerHealth

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        doingTime = 0;

        uiFallowPlayer = ui.GetComponent<UiFallowPlayer>();

        health = transform.GetComponent<PlayerHealth>();

        if (photonView.IsMine)
        {
            CinemachineVirtualCamera followCam = FindObjectOfType<CinemachineVirtualCamera>();
            followCam.LookAt = transform;           
            cameraFollow = followCam.GetComponent<CameraFollow>();
            cameraFollow.playerController = this;
            cameraFollow.toFallow = transform;
            //followCam.LookAt = transform;
        }
    }

    private void Update()
    {

        animator.SetBool("attack", isAttack);
        if (!doSomething && !health.isDead)
        {
            PlayerMove();
            if (!health.isDown)
            {
                PLayerIsClick();
            }
        }

        if (shouldStartTiming)
        {
            doingTime = Time.time - startTime;
        }

        Debug.DrawRay(transform.position, transform.forward * 1.0f, Color.magenta);

        if (Input.GetKeyDown(KeyCode.E) && !doSomething && !health.isDead && !health.isDown)
        {
            if (!doSomething && !Input.GetMouseButton(0))
            {
                // �÷��̾��� �տ� �ִ� ��ü�� �Ǻ�
                if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 1.0f))
                {
                    Debug.DrawLine(transform.position, hitInfo.point, Color.red);

                    if (hitInfo.collider.gameObject.tag == "Player")
                    {
                        playerHealth = hitInfo.collider.gameObject.GetComponent<PlayerHealth>();
                        bool isPlayerDown = playerHealth.isDown;
                        if (isPlayerDown)
                        {
                            StartCoroutine(PressE());
                        }
                    }
                    if (hitInfo.collider.gameObject.tag == "Warehouse")     // �Ʒ� else�� ���ĵ� �ɵ�?
                    {
                        StartCoroutine(PressE());
                    }
                    else
                    {
                        StartCoroutine(PressE());
                    }
                }
                // �÷��̾��� �տ� �ִ� ��ü�� �Ǻ�
                else
                {

                }

            }

        }
        else if (Input.GetKey(KeyCode.E) && doSomething && !health.isDead && !health.isDown)
        {
            uiFallowPlayer.Gauge(60);

        }
        else if (Input.GetKeyUp(KeyCode.E) && doSomething && !health.isDead && !health.isDown)  // EŰ�� ���� �۾��� ���߱�
        {
            shouldStartTiming = false;
            doingTime = 0;
            doSomething = false;
            animator.SetBool("DoSomething", doSomething);
            uiFallowPlayer.currentValue = 0;
            uiFallowPlayer.LoadingBar.fillAmount = uiFallowPlayer.currentValue / 100;
        }

        if (isAttack && health.health <= 0)
        {
            isAttack = false;
            animator.SetBool("attack", isAttack);
        }

        if (doingTime > 2 && doSomething)       // �۾��� ������ �� �ൿ�� ���߱�
        {
            shouldStartTiming = false;

            doingTime = 0;
            uiFallowPlayer.currentValue = 0;
            uiFallowPlayer.LoadingBar.fillAmount = uiFallowPlayer.currentValue / 100;
            doSomething = false;
            animator.SetBool("DoSomething", doSomething);

            if (eat)
            {
                // ���� ���Ŀ� ���� ȸ��
            }

            // ���⿡ �Ϸ������ ��ȣ�ۿ��� ������ �ڵ带 �߰��ؾߵ�
            if (doingCase == 1)
            {
                //������

                doingCase = 0;
            }
            else if (doingCase == 2)    // �ٿ�� �÷��̾��϶� �츲
            {
                //�÷��̾�
                Collider hitCollider = hitInfo.collider;

                GameObject toRevive = hitCollider.gameObject;

                playerHealth = toRevive.GetComponent<PlayerHealth>();

                playerHealth.health = 20;
                playerHealth.playerDown = 100;
                playerHealth.isDown = false;

                doingCase = 0;
            }
            else if (doingCase == 3)
            {
                //����

                doingCase = 0;
            }
        }
    }

    // �÷��̾� ������
    #region
    private void PlayerMove()          
    {
        // ��ǥ�̵�
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
        // �ִϸ��̼�
    }
    #endregion
    // �÷��̾� ������

    // ����
    #region
    private void PLayerIsClick()
    {
        if (Input.GetMouseButton(0) && !isAttack && !health.isInside && !health.isInside)   // �߰����� : �տ� ������ ������
        {
            Attack();
            uiFallowPlayer.Gauge(120);
        }
        //else if(true)//(������ �տ� ������)
        //{
           

        //    uiFallowPlayer.Gauge(120);
        //    StartCoroutine(Eat());
        //}
        else if (Input.GetMouseButtonUp(0) && !isAttack && !health.isInside && !health.isInside)    // �߰����� : �տ� ������ ������
        {
            if (!eat)
            {

                isAttack = true;
                StartCoroutine(EndAttack());
            }
            else
            {
                eat = false;
                doSomething = false;
                shouldStartTiming = false;

            }
        }
    }

    private void Attack()
    {
        if(!hand)
        {       //������ �ȵ������
            animator.Play("Punch_R", -1, 0.2f);

            attackPower += Time.deltaTime;
        }
        else
        {       //������ �������

            animator.Play("AttackAnimation", -1, 0.35f);

            attackPower += Time.deltaTime;
        }
    }

    private IEnumerator EndAttack()
    {
        uiFallowPlayer.currentValue = 0;
        uiFallowPlayer.LoadingBar.fillAmount = uiFallowPlayer.currentValue / 100;
        if (!hand)
        {       //������ �ȵ������
            if (attackPower >= 0.9f)           // ������ ��ġ ���� �ʿ�
            { damage = 2; }
            else
            { damage = 1; }
            Debug.Log(damage);

            animator.SetBool("attack", isAttack);
            yield return new WaitForSeconds(0.2f);
            fist.SetActive(true);
            yield return new WaitForSeconds(0.6f);
            fist.SetActive(false);

            isAttack = false;
            attackPower = 0;
        }
        else
        {       //������ �������
            if (attackPower >= 0.9f)           // ������ ��ġ ���� �ʿ�
            { damage = 2; }
            else
            { damage = 1; }

            animator.SetBool("attack", isAttack);
            yield return new WaitForSeconds(0.1f);
            weapon.SetActive(true);
            yield return new WaitForSeconds(0.9f);
            weapon.SetActive(false);

            isAttack = false;
            attackPower = 0;
        }
        
    }

    private IEnumerator Eat()
    {
        eat = true;

        shouldStartTiming = true;
        startTime = Time.time;
        animator.Play("Eat");
        yield return new WaitForSeconds(1.2f);

    }

    #endregion
    // ����   

    private IEnumerator PressE()
    {
        shouldStartTiming = true;
        startTime = Time.time;
        if (hitInfo.collider.gameObject.tag == "Item") // ���ø��
        {
            doSomething = true;
            animator.SetBool("DoSomething", doSomething);
            animator.Play("Looting");
            yield return new WaitForSeconds(1.2f);

            doingCase = 1;
        }
        else if(hitInfo.collider.gameObject.tag == "Player")
        {
            animator.SetFloat("Speed", 0);
            doSomething = true;
            animator.SetBool("DoSomething", doSomething);
            animator.Play("DoSomething");
            yield return new WaitForSeconds(1.2f);

            doingCase = 2;
        }
        else if (hitInfo.collider.gameObject.tag == "Warehouse") // �۾����
        {
            animator.SetFloat("Speed", 0);
            doSomething = true;
            animator.SetBool("DoSomething", doSomething);
            animator.Play("DoSomething");
            yield return new WaitForSeconds(1.2f);

            doingCase = 3;
        }
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
            //cameraObject = GameObject.Find("CM vcam1");
            //CameraFollow cameraFollow = cameraObject.gameObject.GetComponent<CameraFollow>(); // ī�޶� �� ������Ʈ�� ã�� ī�޶� ��
            cameraFollow.inside = other.gameObject;
            cameraFollow.isInside = true;

           
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Building"))
        {
            //cameraObject = GameObject.Find("CM vcam1");
            //CameraFollow cameraFollow = cameraObject.gameObject.GetComponent<CameraFollow>(); // ī�޶� �� ������Ʈ�� ã�� ī�޶� ��
            //bool isInside = cameraFollow.isInside;
            cameraFollow.isInside =  false;
            
            //if (photonView.isMine)
            //{
            //    CinemachineVirtualCamera followCam = FindObjectOfType<CinemachineVirtualCamera>();
            //    followCam.LookAt = transform;
            //}
        }
    }
    #endregion
    // �ǳ� ����

}
//animator.SetLayerWeight(1, 0.0f);       // �ι�°(1) ���̾��� �ִϸ��̼��� ����