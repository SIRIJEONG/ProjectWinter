using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : LivingEntity
{
    public GameObject destroyPrefab; // 파괴된 자리에 인스턴스화할 프리팹

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

        Debug.Log("죽었나?");
        DestroyObjectAndInstantiatePrefab();
    }

    void DestroyObjectAndInstantiatePrefab()
    {

        Debug.Log("꼬마돌 파괴");


        Vector3 rockPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        Quaternion rockRotation = transform.rotation;

        // 현재 게임 오브젝트를 파괴합니다.
        Destroy(gameObject);

        // 파괴된 위치에 프리팹을 인스턴스화합니다.
        if (destroyPrefab != null)
        {
            GameObject changeRock = Instantiate(destroyPrefab, rockPosition, rockRotation);

            // 스프라이트의 크기를 조절합니다.
            Vector3 newScale = new Vector3(0.5f, 0.5f, 0.5f); // X, Y, Z 축의 크기를 조절합니다.
            changeRock.transform.localScale = newScale;

        }
    }
}