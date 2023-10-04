using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class Plant : LivingEntity
{
    public GameObject dropFruit;
    void Update()
    {

    }

    public override void Die()
    {
        isDead = true;
        Debug.Log("죽었나?");
        photonView.RPC("DisableAllChildren", RpcTarget.All);        
    }

    [PunRPC]
    public void DisableAllChildren()
    {
        // 모든 자식 오브젝트를 가져옴
        Transform[] children = GetComponentsInChildren<Transform>();

        // 부모 자체를 제외하고 모든 자식 오브젝트를 반복
        foreach (Transform child in children)
        {
            if (child != transform) // 부모 자체를 제외
            {
                // 기존 자식 오브젝트를 비활성화
                child.gameObject.SetActive(false);
            }
        }

        Debug.Log("열매가 떨어졌다!");

        if (PhotonNetwork.IsMasterClient)
        {
            // 부모 오브젝트의 위치와 회전값을 가져옴
            Vector3 parentPosition = new Vector3(transform.position.x + 1, transform.position.y + 1, transform.position.z - 2);
            Quaternion parentRotation = transform.rotation;

            // 대체할 스프라이트를 인스턴스화하고 위치 및 회전값을 설정
            GameObject replacementSprite = PhotonNetwork.Instantiate(dropFruit.name, parentPosition, parentRotation);

            // 새로운 스프라이트를 부모 오브젝트의 자식으로 만듬
            replacementSprite.transform.parent = transform;

            // 스프라이트의 크기를 조절
            Vector3 newScale = new Vector3(1.0f, 1.0f, 1.0f); // X, Y, Z 축의 크기를 조절
            replacementSprite.transform.localScale = newScale;
        }
    }
}
