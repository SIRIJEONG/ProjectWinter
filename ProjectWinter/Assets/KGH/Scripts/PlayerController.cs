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

    // 플레이어의 직업
    public static bool isSurvivor;
    public static bool isTrator;
    // 플레이어의 직업

    private Vector3 moveVec;

    // 공격관련
    private float attackPower;  // 마우스로 공격 차지
    private bool isAttack;
    // 공격관련
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        PlayerMove();

        PLayerIsClick();

        // 플레이어의 앞에 있는 물체를 판별
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 1.0f))
        {

        }
        // 플레이어의 앞에 있는 물체를 판별


        
    }


    private void PlayerMove()           // 플레이어 움직임
    {
        // 좌표이동
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        moveVec = new Vector3(horizontal, 0, vertical).normalized;

        transform.position += moveVec * speed * Time.deltaTime;

        transform.LookAt(transform.position + moveVec);
        // 좌표이동

        // 애니메이션
        if (Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0)
        {
            aniSpeed = Mathf.MoveTowards(aniSpeed, 1, Time.deltaTime * 3);
        }
        else
        {
            aniSpeed = Mathf.MoveTowards(aniSpeed, 0, Time.deltaTime * 3);
        }
        animator.SetFloat("Speed", aniSpeed);
        // 애니메이션
    }

    // 공격
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
    // 공격   
}
//animator.SetLayerWeight(1, 0.0f);       // 두번째(1) 레이어의 애니메이션을 멈춤