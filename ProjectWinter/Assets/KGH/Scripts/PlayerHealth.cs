using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    private Animator animator;

    // ü�°���
    private float playerDown = 100;
    public bool isDown = false;

    private bool playerEnd;

    //private bool isDead = false;
    // ü�°���

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
        base.Die();
    }

    private void onDeath()
    {
        isDown = true;
        animator.SetBool("Down", isDown);
        PlayerController.speed = 2;
        // �ð��� playerDown�� �ٿ�����
        if(playerDown <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        playerEnd = true;
        animator.SetBool("Dead", playerEnd);

    }
}