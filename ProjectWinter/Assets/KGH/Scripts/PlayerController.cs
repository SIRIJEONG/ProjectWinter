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

    // �÷��̾��� ����
    public static bool isSurvivor;
    public static bool isTrator;
    // �÷��̾��� ����

    private Vector3 moveVec;

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
        PlayerMove();

        PLayerIsClick();

        // �÷��̾��� �տ� �ִ� ��ü�� �Ǻ�
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 1.0f))
        {

        }
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
        animator.Play("AttackAnimation", -1, 0.35f);

        attackPower += Time.deltaTime;
    }

    private IEnumerator EndAttack()
    {
            Debug.Log(isAttack);
        animator.SetBool("attack", isAttack);

        yield return new WaitForSeconds(0.8f);
        isAttack = false;
        attackPower = 0;
    }
    #endregion
    // ����   
}
//animator.SetLayerWeight(1, 0.0f);       // �ι�°(1) ���̾��� �ִϸ��̼��� ����