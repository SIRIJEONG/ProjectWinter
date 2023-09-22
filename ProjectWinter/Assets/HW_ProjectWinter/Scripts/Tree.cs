using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Tree : LivingEntity
{
    public string targetChildName; // ��Ȱ��ȭ�� Ư�� �ڽ� ������Ʈ�� �̸�    

    void Start()
    {

    }

    void Update()
    {

    }

    public override void Die()
    {
        isDead = true;
        Debug.Log("�׾���?");
        photonView.RPC("TreeDestroy", RpcTarget.All);
    }

    [PunRPC]
    public void TreeDestroy()
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
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject changeTrees = PhotonNetwork.Instantiate("WoodPiece", treePosition, treeRotation);
            // ��������Ʈ�� ũ�⸦ �����մϴ�.
            Vector3 newScale = new Vector3(0.5f, 0.5f, 0.5f); // X, Y, Z ���� ũ�⸦ �����մϴ�.
            changeTrees.transform.localScale = newScale;
        }      
    }
}
