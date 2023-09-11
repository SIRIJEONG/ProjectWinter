using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    private Animator animator;

    // 체력관련
    private float playerHP;
    private float playerDown = 100;
    private bool isDown = false;

    //private bool isDead = false;
    // 체력관련

    LivingEntity livingEntityClass;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        livingEntityClass = FindAnyObjectByType<LivingEntity>();
        livingEntityClass.onDeath += Down;

        playerHP = health;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Die()
    {
        isDown = true;
    }

    private void Down()
    {
        animator.SetBool("Down", isDown);
        // 시간당 playerDown을 줄여나감
        if(playerDown <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        animator.SetBool("Dead", isDead);

    }

    private void OnTriggerEnter(Collider other)
    {
        if( other.tag.Equals("hit"))
        { 

        }
    }

}