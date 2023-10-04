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
        Debug.Log("�׾���?");
        photonView.RPC("DisableAllChildren", RpcTarget.All);        
    }

    [PunRPC]
    public void DisableAllChildren()
    {
        // ��� �ڽ� ������Ʈ�� ������
        Transform[] children = GetComponentsInChildren<Transform>();

        // �θ� ��ü�� �����ϰ� ��� �ڽ� ������Ʈ�� �ݺ�
        foreach (Transform child in children)
        {
            if (child != transform) // �θ� ��ü�� ����
            {
                // ���� �ڽ� ������Ʈ�� ��Ȱ��ȭ
                child.gameObject.SetActive(false);
            }
        }

        Debug.Log("���Ű� ��������!");

        if (PhotonNetwork.IsMasterClient)
        {
            // �θ� ������Ʈ�� ��ġ�� ȸ������ ������
            Vector3 parentPosition = new Vector3(transform.position.x + 1, transform.position.y + 1, transform.position.z - 2);
            Quaternion parentRotation = transform.rotation;

            // ��ü�� ��������Ʈ�� �ν��Ͻ�ȭ�ϰ� ��ġ �� ȸ������ ����
            GameObject replacementSprite = PhotonNetwork.Instantiate(dropFruit.name, parentPosition, parentRotation);

            // ���ο� ��������Ʈ�� �θ� ������Ʈ�� �ڽ����� ����
            replacementSprite.transform.parent = transform;

            // ��������Ʈ�� ũ�⸦ ����
            Vector3 newScale = new Vector3(1.0f, 1.0f, 1.0f); // X, Y, Z ���� ũ�⸦ ����
            replacementSprite.transform.localScale = newScale;
        }
    }
}
