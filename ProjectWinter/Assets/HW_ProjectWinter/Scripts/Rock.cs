using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : LivingEntity
{
    public GameObject destroyPrefab; // �ı��� �ڸ��� �ν��Ͻ�ȭ�� ������

    void Update()
    {
        if (health == 0 && isDead == false)
        {
            isDead = true;
            Die();
        }
    }

    public override void Die()
    {

        Debug.Log("�׾���?");
        DestroyObjectAndInstantiatePrefab();
    }

    void DestroyObjectAndInstantiatePrefab()
    {

        Debug.Log("������ �ı�");


        Vector3 rockPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        Quaternion rockRotation = transform.rotation;

        // ���� ���� ������Ʈ�� �ı��մϴ�.
        Destroy(gameObject);

        // �ı��� ��ġ�� �������� �ν��Ͻ�ȭ�մϴ�.
        if (destroyPrefab != null)
        {
            GameObject changeRock = Instantiate(destroyPrefab, rockPosition, rockRotation);

            // ��������Ʈ�� ũ�⸦ �����մϴ�.
            Vector3 newScale = new Vector3(0.5f, 0.5f, 0.5f); // X, Y, Z ���� ũ�⸦ �����մϴ�.
            changeRock.transform.localScale = newScale;

        }
    }
}