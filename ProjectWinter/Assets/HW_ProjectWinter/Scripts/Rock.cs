using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Rock : LivingEntity
{
    void Update()
    {

    }

    public override void Die()
    {
        isDead = true;
        Debug.Log("�׾���?");
        photonView.RPC("DestroyObjectAndInstantiatePrefab", RpcTarget.All);
    }

    [PunRPC]
    public void DestroyObjectAndInstantiatePrefab()
    {

        Debug.Log("������ �ı�");

        Vector3 rockPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        Quaternion rockRotation = transform.rotation;

        // ���� ���� ������Ʈ�� �ı��մϴ�.
        Destroy(gameObject);

        // �ı��� ��ġ�� �������� �ν��Ͻ�ȭ�մϴ�.
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject changeRock = PhotonNetwork.Instantiate("RockPiece", rockPosition, rockRotation);
            // ��������Ʈ�� ũ�⸦ �����մϴ�.
            Vector3 newScale = new Vector3(0.5f, 0.5f, 0.5f); // X, Y, Z ���� ũ�⸦ �����մϴ�.
            changeRock.transform.localScale = newScale;
        }

    }
}