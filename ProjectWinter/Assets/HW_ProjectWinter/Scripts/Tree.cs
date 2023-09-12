using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : LivingEntity
{
    public string targetChildName; // ��Ȱ��ȭ�� Ư�� �ڽ� ������Ʈ�� �̸�
    public GameObject changePrefab; // �ı��� �ڸ��� �ν��Ͻ�ȭ�� ������

    void Start()
    {

    }

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
        TreeDestroy();
    }


    void TreeDestroy()
    {

        // Ư�� �ڽ� ������Ʈ�� ã�Ƽ� ��Ȱ��ȭ�մϴ�.
        Transform targetChild = transform.Find(targetChildName);

        if (targetChild != null)
        {
            targetChild.gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("ã������ �ڽ� ������Ʈ�� ã�� ���߽��ϴ�: " + targetChildName);
        }


        Vector3 treePosition = new Vector3(targetChild.position.x + 3, targetChild.position.y + 3, targetChild.position.z - 2);
        Quaternion treeRotation = targetChild.rotation;


        // �ı��� ��ġ�� �������� �ν��Ͻ�ȭ�մϴ�.
        if (changePrefab != null)
        {
            GameObject changeTrees = Instantiate(changePrefab, treePosition, treeRotation);

            // ��������Ʈ�� ũ�⸦ �����մϴ�.
            Vector3 newScale = new Vector3(0.5f, 0.5f, 0.5f); // X, Y, Z ���� ũ�⸦ �����մϴ�.
            changeTrees.transform.localScale = newScale;

        }
    }
}
