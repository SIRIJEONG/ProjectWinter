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
    public int speed = 5;

    private bool doSomething = false;

    // 플레이어의 직업
    public static bool isSurvivor;
    public static bool isTrator;
    // 플레이어의 직업

    public GameObject weapon;
    public GameObject fist;
    public GameObject ui;

    private bool hand = false;

    private RaycastHit hitInfo;

    private Vector3 moveVec;

    private float doingTime;
    private float startTime;
    private bool shouldStartTiming = false;

    private UiFallowPlayer uiFallowPlayer;

    // 공격관련
    private float attackPower;  // 마우스로 공격 차지
    private bool isAttack;
    public int damage;         // 줄 데미지
    // 공격관련
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        doingTime = 0;

        uiFallowPlayer = ui.GetComponent<UiFallowPlayer>();
        

    }

    private void Update()
    {
        if (!doSomething)
        {
            PlayerMove();

            PLayerIsClick();
        }

        if (shouldStartTiming)
        {
            doingTime = Time.time - startTime;
            //Debug.Log("경과 시간: " + doingTime.ToString("F2") + "초");
        }
        if (Input.GetKeyDown(KeyCode.E) && !doSomething)
        {
            if (!doSomething && !Input.GetMouseButton(0))
            {
                // 플레이어의 앞에 있는 물체를 판별
                if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 1.0f))
                {
                    Debug.DrawLine(transform.position, hitInfo.point, Color.red);

                    //DoingTime();
                    if (hitInfo.collider.gameObject.tag == "Player")
                    {
                        PlayerHealth playerHealth = hitInfo.collider.gameObject.GetComponent<PlayerHealth>();
                        bool isPlayerDown = playerHealth.isDown;
                        if (isPlayerDown)
                        {
                            StartCoroutine(PressE());
                        }
                    }
                    if (hitInfo.collider.gameObject.tag == "Warehouse")     // 아래 else와 합쳐도 될듯?
                    {
                        StartCoroutine(PressE());
                    }
                    else
                    {
                        StartCoroutine(PressE());
                    }
                }
                // 플레이어의 앞에 있는 물체를 판별
                else
                {
                    Debug.DrawRay(transform.position, transform.forward * 1.0f, Color.green);

                }

            }

        }
        else if (Input.GetKey(KeyCode.E) && doSomething)
        {
            uiFallowPlayer.Gauge(60);

        }
        else if (Input.GetKeyUp(KeyCode.E) && doSomething)  // E키를 떼면 작업을 멈추기
        {
            shouldStartTiming = false;
            doingTime = 0;
            doSomething = false;
            animator.SetBool("DoSomething", doSomething);
            uiFallowPlayer.currentValue = 0;
            uiFallowPlayer.LoadingBar.fillAmount = uiFallowPlayer.currentValue / 100;
        }

        if (doingTime > 2 && doSomething)       // 작업이 끝났을 때 행동을 멈추기
        {
            shouldStartTiming = false;

            doingTime = 0;
            uiFallowPlayer.currentValue = 0;
            uiFallowPlayer.LoadingBar.fillAmount = uiFallowPlayer.currentValue / 100;
            doSomething = false;
            animator.SetBool("DoSomething", doSomething);

            // 여기에 완료됐을때 상호작용을 실행할 코드를 추가해야됨

        }
    }

    // 플레이어 움직임
    #region
    private void PlayerMove()          
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
    #endregion
    // 플레이어 움직임

    // 공격
    #region
    private void PLayerIsClick()
    {
        if (Input.GetMouseButton(0) && !isAttack)
        {
            Attack();
            uiFallowPlayer.Gauge(120);
        }
        else if (Input.GetMouseButtonUp(0) && !isAttack)
        {
            isAttack = true;
            StartCoroutine(EndAttack());
        }
    }

    private void Attack()
    {
        if(!hand)
        {       //아이템 안들었을때
            animator.Play("Punch_R", -1, 0.2f);

            attackPower += Time.deltaTime;
        }
        else
        {       //아이템 들었을때

            animator.Play("AttackAnimation", -1, 0.35f);

            attackPower += Time.deltaTime;
        }
    }

    private IEnumerator EndAttack()
    {
        uiFallowPlayer.currentValue = 0;
        uiFallowPlayer.LoadingBar.fillAmount = uiFallowPlayer.currentValue / 100;
        if (!hand)
        {       //아이템 안들었을때
            if (attackPower >= 0.9f)           // 데미지 수치 수정 필요
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
        {       //아이템 들었을때
            if (attackPower >= 0.9f)           // 데미지 수치 수정 필요
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


    #endregion
    // 공격   

    private IEnumerator PressE()
    {
        shouldStartTiming = true;
        startTime = Time.time;
        if (hitInfo.collider.gameObject.tag == "Item") // 루팅모션
        {
            doSomething = true;
            animator.SetBool("DoSomething", doSomething);
            animator.Play("Looting");

            yield return new WaitForSeconds(1.2f);
        }
        else if(hitInfo.collider.gameObject.tag == "Player")
        {

        }
        else if (hitInfo.collider.gameObject.tag == "Warehouse") // 작업모션
        {
            animator.SetFloat("Speed", 0);
            doSomething = true;
            animator.SetBool("DoSomething", doSomething);
            animator.Play("DoSomething");
            yield return new WaitForSeconds(1.2f);
        }
        // 행동이 완료되기까지 남은 시간 게이지
    }

    // 실내 여부
    #region
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Building"))           // 플레이어가 건물 안으로 들어갔으면
        {
            CameraFollow.inside = other.gameObject; // 카메라를 둘 오브잭트를 찾아 카메라를 둠
            CameraFollow.isInside = true;           // 수정필요
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Building"))
        {
            CameraFollow.isInside = false;
        }
    }
    #endregion
    // 실내 여부

}
//animator.SetLayerWeight(1, 0.0f);       // 두번째(1) 레이어의 애니메이션을 멈춤