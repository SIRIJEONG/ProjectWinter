using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;
    private float aniSpeed = 0;

    private float horizontal;
    private float vertical;
    public static int speed = 5;

    private bool doSomething = false;

    // �÷��̾��� ����
    public static bool isSurvivor;
    public static bool isTrator;
    // �÷��̾��� ����

    private bool hand = false;

    private RaycastHit hitInfo;

    private Vector3 moveVec;

    private float doingTime;
    private float startTime;
    private bool shouldStartTiming = false;

    // ���ݰ���
    private float attackPower;  // ���콺�� ���� ����
    private bool isAttack;
    // ���ݰ���
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        doingTime = 0;
        
    }

    private void Update()
    {
        Debug.LogFormat("����Ÿ�� {0}", doingTime);
        if (!doSomething)
        {
            PlayerMove();

            PLayerIsClick();
        }

        if (shouldStartTiming)
        {
            doingTime = Time.time - startTime;
            //Debug.Log("��� �ð�: " + doingTime.ToString("F2") + "��");
        }

        if (Input.GetKeyDown(KeyCode.E) && !doSomething)
        {
            if (!doSomething)
            {
                // �÷��̾��� �տ� �ִ� ��ü�� �Ǻ�
                if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 1.0f))
                {
                    DoingTime();
                    if (hitInfo.collider.gameObject.tag == "Player")
                    {
                        PlayerHealth playerHealth = hitInfo.collider.gameObject.GetComponent<PlayerHealth>();
                        bool isPlayerDown = playerHealth.isDown;
                        if (isPlayerDown)
                        {
                            StartCoroutine(PressE());
                        }
                    }
                    if (hitInfo.collider.gameObject.tag == "Warehouse")
                    {
                        StartCoroutine(PressE());
                    }
                }
                // �÷��̾��� �տ� �ִ� ��ü�� �Ǻ�

                // �ൿ�� �Ϸ�� ���������� �ν�� �������� �����
            }

        }
        else if (Input.GetKey(KeyCode.E) && doSomething)
        {
            UiFallowPlayer.Gauge();

        }
        else if (Input.GetKeyUp(KeyCode.E) && doSomething)
        {
            doSomething = false;
            animator.SetBool("DoSomething", doSomething);
            UiFallowPlayer.currentValue = 0;
            UiFallowPlayer.LoadingBar.fillAmount = UiFallowPlayer.currentValue / 100;
        }

        if (doingTime > 2)
        {
            shouldStartTiming = false;

            doingTime = 0;
            UiFallowPlayer.currentValue = 0;
            UiFallowPlayer.LoadingBar.fillAmount = UiFallowPlayer.currentValue / 100;
            doSomething = false;
            animator.SetBool("DoSomething", doSomething);

            Debug.Log("1");
            // ���⿡ �Ϸ������ ��ȣ�ۿ��� ������ �ڵ带 �߰��ؾߵ�

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
        if (Input.GetMouseButton(0) && !isAttack)
        {
            Attack();
            UiFallowPlayer.Gauge();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isAttack = true;
            StartCoroutine(EndAttack());
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
        UiFallowPlayer.currentValue = 0;
        UiFallowPlayer.LoadingBar.fillAmount = UiFallowPlayer.currentValue / 100;
        if (!hand)
        {       //������ �ȵ������
            animator.SetBool("attack", isAttack);

            yield return new WaitForSeconds(0.8f);
            isAttack = false;
            attackPower = 0;
        }
        else
        {       //������ �������
            animator.SetBool("attack", isAttack);

            yield return new WaitForSeconds(0.8f);
            isAttack = false;
            attackPower = 0;
        }
        
    }


    #endregion
    // ����   

    private IEnumerator PressE()
    {
        shouldStartTiming = true;
        startTime = Time.time;
        if (hitInfo.collider.gameObject.tag == "Item") // ���ø��
        {

            yield return new WaitForSeconds(1.2f);
        }
        else if(hitInfo.collider.gameObject.tag == "Player")
        {

        }
        else        // �۾����
        {
            animator.SetFloat("Speed", 0);
            doSomething = true;
            animator.SetBool("DoSomething", doSomething);
            animator.Play("DoSomething");
            yield return new WaitForSeconds(1.2f);
        }
        // �ൿ�� �Ϸ�Ǳ���� ���� �ð� ������
    }
        
    private void DoingTime()
    {
        doingTime = 
            
            - startTime;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Building"))           // �÷��̾ �ǹ� ������ ������
        {
            CameraFollow.inside = other.gameObject; // ī�޶� �� ������Ʈ�� ã�� ī�޶� ��
            CameraFollow.isInside = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Building"))
        {
            CameraFollow.isInside = false;
        }
    }

}
//animator.SetLayerWeight(1, 0.0f);       // �ι�°(1) ���̾��� �ִϸ��̼��� ����