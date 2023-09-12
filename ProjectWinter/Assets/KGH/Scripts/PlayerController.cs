using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

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

    private GameObject hitObject;
    // ���ݰ���
    private float attackPower;  // ���콺�� ���� ����
    private bool isAttack;
    // ���ݰ���
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        if (!doSomething)
        {
            PlayerMove();

            PLayerIsClick();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if(!doSomething)
            {
                // �÷��̾��� �տ� �ִ� ��ü�� �Ǻ�
                if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 1.0f))
                {
                    if (hitInfo.collider.gameObject.tag == "Player")
                    {
                        PlayerHealth playerHealth = hitInfo.collider.gameObject.GetComponent<PlayerHealth>();
                        bool isPlayerDown = playerHealth.isDown;
                        if (isPlayerDown)
                        {
                            PressE();        
                        }
                            
                    }
                }
                // �÷��̾��� �տ� �ִ� ��ü�� �Ǻ�

                // �ൿ�� �Ϸ�� ���������� �ν�� �������� �����
            }
            else
            {
                doSomething = false;
                animator.SetBool("DoSomething", doSomething);
            }

        }
       

        // �÷��̾��� �տ� �ִ� ��ü�� �Ǻ�
        //if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 1.0f))
        //{
        //    
        //
        //}
        // �÷��̾��� �տ� �ִ� ��ü�� �Ǻ�        

    }


    private void PlayerMove()           // �÷��̾� ������
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

    // ����
    #region
    private void PLayerIsClick()
    {
        if (Input.GetMouseButton(0) && !isAttack)
        {
            Attack();
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
            animator.Play("Punch_R", -1, 0.35f);

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

    private void PressE()
    {
        if (hitInfo.collider.gameObject.tag == "Item") // ���ø��
        {
            
        }
        else        // �۾����
        {
            animator.SetFloat("Speed", 0);
            doSomething = true;
            animator.SetBool("DoSomething", doSomething);
            animator.Play("DoSomething");
        }
        // �ൿ�� �Ϸ�Ǳ���� ���� �ð� ������
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Building"))
        {
            Debug.Log("1");
            CameraFollow.inside = other.gameObject;
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