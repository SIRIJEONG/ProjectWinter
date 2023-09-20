using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    private Animator animator;

    // 체력관련
    public float playerDown = 100;
    public float hunger = 100;
    public float cold = 100;
    public bool isDown = false;
    public float maxHP = 100;

    private bool playerEnd;

    public GameObject ghost;        // 죽었을때 불러올 오브잭트
    public GameObject powerGauge;
    public GameObject downGauge;
    //private bool isDead = false;
    // 체력관련

    public bool isInside;

    private bool playOne = true;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        // onDeath += Down;
        playerEnd = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (health > maxHP)     //최대치를 넘길시 최대치로 초기화
        { health = maxHP;}
        if ( hunger > 100)
        { hunger = 100;}
        if ( cold > 100)
        { cold = 100;}

        if ( health < 0) //최소치를 넘길시 최소치로 초기화
        { health = 0;}
        if ( cold < 0) 
        {  cold = 0;}
        if ( hunger < 0) 
        {  hunger = 0;}

        if(!isDown)
        {
            animator.SetBool("Down", isDown);
        }

        if (isDown)
        {
            onDeath();

            powerGauge.SetActive(false);
            downGauge.SetActive(true);
        }
        else
        {
            powerGauge.SetActive(true); 
            downGauge.SetActive(false);
        }

        if (hunger > 0)
        {
            hunger -= Time.deltaTime / 5;       // 매 프레임마다 허기 감소
        }

        if (!isInside && cold > 0)         // 산장에 있을 시 온도 증가, 없을 시 감소
        {
            cold -= Time.deltaTime / 2.5f;
        }
        else if(isInside && cold <= 100)
        {
            cold += Time.deltaTime * 5;
        }

        if(hunger < 25)                     // 허기가 일정수치 미만일때 체력 감소
        {
            health -= Time.deltaTime;
            if ( health <= 0 )
            {
                Die();
            }
        }

        if (cold <= 25 && maxHP > 25)       // 추위가 일정수치 이하일때 최대체력 감소
        {
            maxHP -= Time.deltaTime * 5;
        }
        else if (cold > 25 && maxHP <= 100) // 증가
        {
            maxHP += Time.deltaTime * 5;
        }
    }

    public override void Die()
    {
        //base.Die();        
        onDeath();
    }

    private void onDeath()
    {
        isDown = true;
        animator.SetBool("Down", isDown);
        PlayerController playerController = transform.gameObject.GetComponent<PlayerController>();
        playerController.speed = 2;

        // 시간당 playerDown을 줄여나감
        playerDown -= Time.deltaTime * 3;

        if (playerDown <= 0)
        {
            Dead();     // 죽고

            GhostOn();

            playerController.enabled = false;       // 컨트롤러 비활성화해서 못움직이게

        }
    }

    private void GhostOn()  // 같은 Ghost상태인 플레이어에게만 동기화
    {
        ghost.SetActive(true);      // 플레이어 유령상태 키고
    }

    private void Dead()
    {
        playerEnd = true;
        isDead = true;
        
        if (playOne)
        {
            animator.Play("Death");
            playOne = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Attack"))
        {
            if (!isDown)
            {/*
                PlayerController playercontroller = other.transform.parent.GetComponent<PlayerController>();
                //PlayerHealth playerHealth = transform.parent.GetComponent<PlayerHealth>();
                int getdamage = playercontroller.damage;
                */      // 멀티 적용시 주석 풀고 아래 정수형 지워야됨
                int getdamage = 10;
                Vector3 hitpoint = other.ClosestPoint(transform.position);
                Vector3 hitnormal = transform.position - other.transform.position;
                OnDamage(getdamage, hitpoint, hitnormal);
            }
            else
            {
                PlayerController playercontroller = other.transform.parent.GetComponent<PlayerController>();
                //PlayerHealth playerHealth = transform.parent.GetComponent<PlayerHealth>();
                int getdamage = playercontroller.damage;

                playerDown -= getdamage;
            }
        }
        if (other.CompareTag("Building"))
        {
            if (other.name == "MountainVilla")
            {
                isInside = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Building"))
        {
            if (other.name == "MountainVilla")
            {
                 isInside = false;                
            }
        }
    }
}