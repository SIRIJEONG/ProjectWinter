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
        if(other.CompareTag("attack"))
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            int getDamage = playerController.damage;
            Vector3 hitPoint = other.ClosestPoint(transform.position);
            Vector3 hitNormal = transform.position - other.transform.position;
            OnDamage(getDamage, hitPoint, hitNormal);
        }
    }
}