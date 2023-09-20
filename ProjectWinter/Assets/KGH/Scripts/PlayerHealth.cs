using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : LivingEntity
{
    private Animator animator;

    // ü�°���
    public float playerDown = 100;
    public float hunger = 100;
    public float cold = 100;
    public bool isDown = false;
    public float maxHP = 100;

    private bool playerEnd;

    public GameObject ghost;        // �׾����� �ҷ��� ������Ʈ
    public GameObject powerGauge;
    public GameObject downGauge;
    //private bool isDead = false;
    // ü�°���

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
        if (health > maxHP)     //�ִ�ġ�� �ѱ�� �ִ�ġ�� �ʱ�ȭ
        { health = maxHP;}
        if ( hunger > 100)
        { hunger = 100;}
        if ( cold > 100)
        { cold = 100;}

        if ( health < 0) //�ּ�ġ�� �ѱ�� �ּ�ġ�� �ʱ�ȭ
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
            hunger -= Time.deltaTime / 5;       // �� �����Ӹ��� ��� ����
        }

        if (!isInside && cold > 0)         // ���忡 ���� �� �µ� ����, ���� �� ����
        {
            cold -= Time.deltaTime / 2.5f;
        }
        else if(isInside && cold <= 100)
        {
            cold += Time.deltaTime * 5;
        }

        if(hunger < 25)                     // ��Ⱑ ������ġ �̸��϶� ü�� ����
        {
            health -= Time.deltaTime;
            if ( health <= 0 )
            {
                Die();
            }
        }

        if (cold <= 25 && maxHP > 25)       // ������ ������ġ �����϶� �ִ�ü�� ����
        {
            maxHP -= Time.deltaTime * 5;
        }
        else if (cold > 25 && maxHP <= 100) // ����
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

        // �ð��� playerDown�� �ٿ�����
        playerDown -= Time.deltaTime * 3;

        if (playerDown <= 0)
        {
            Dead();     // �װ�

            GhostOn();

            playerController.enabled = false;       // ��Ʈ�ѷ� ��Ȱ��ȭ�ؼ� �������̰�

        }
    }

    private void GhostOn()  // ���� Ghost������ �÷��̾�Ը� ����ȭ
    {
        ghost.SetActive(true);      // �÷��̾� ���ɻ��� Ű��
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
                */      // ��Ƽ ����� �ּ� Ǯ�� �Ʒ� ������ �����ߵ�
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