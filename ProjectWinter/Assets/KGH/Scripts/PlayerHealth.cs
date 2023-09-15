using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    private Animator animator;

    // 체력관련
    private float playerDown = 100;
    public bool isDown = false;

    private bool playerEnd;

    //private bool isDead = false;
    // 체력관련

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
        if(isDown)
        {
            onDeath();
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
        animator.SetBool("Dead", playerEnd);
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
    }
}