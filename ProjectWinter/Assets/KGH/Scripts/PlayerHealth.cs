using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    private Animator animator;

    // 체력관련
    private float playerDown = 100;
    private bool isDown = false;

    //private bool isDead = false;
    // 체력관련

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        
       // onDeath += Down;

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void Die()
    {
        base.Die();
        isDown = true;
    }

    private void onDeath()
    {
        animator.SetBool("Down", isDown);
        PlayerController.speed = 2;
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
        if (other.CompareTag("hit"))
        {
            Debug.Log(health);
            OnDamage(10, other.ClosestPoint(transform.position), transform.position - other.transform.position);
        }
    }

}