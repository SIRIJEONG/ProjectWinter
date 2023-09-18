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

        if (hunger >= 0)
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
            Dead();
        }
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
                */
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