using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SG_PowerStationGridScript : MonoBehaviourPun

{
    [SerializeField]
    private GameObject slot;

    private GameObject slotClone;
    private GameObject slotClone_;

    private GameObject makeClone;

    private Transform topParentTrans;

    public bool isMake = false;

    private void Awake()
    {
        //Debug.Log(makeSlotCount);
    }
    private void Start()
    {
        SerchTopParentTrans();
        if (!isMake)
        {
            MakeSlot(); // 슬롯을 랜덤하게 만들어서 자식오브젝트로 넣는 함수
        }
    }

    private void SerchTopParentTrans() // 최상위 부모 오브젝트 찾는 로직
    {
        topParentTrans = transform;

        while (topParentTrans.parent != null)
        {
            topParentTrans = topParentTrans.parent;
        }
    }

    // Photon으로 해야할듯
    private void MakeSlot() // 슬롯을 랜덤하게 만들어서 자식오브젝트로 넣는 함수
    {

        if (topParentTrans.CompareTag("PowerStation"))  // 발전소일때 Instance Slot
        {
            slotClone = PhotonNetwork.Instantiate(slot.name, transform.position, Quaternion.identity);
            slotClone_ = PhotonNetwork.Instantiate(slot.name, transform.position, Quaternion.identity);
            photonView.RPC("ChangePositionItem", RpcTarget.All, topParentTrans);
        }
        else if (topParentTrans.CompareTag("HeliPad"))   // 헬리패드일때 Instance Slot
        {
            slotClone = PhotonNetwork.Instantiate(slot.name, transform.position, Quaternion.identity);
            photonView.RPC("ChangePositionItem", RpcTarget.All, topParentTrans);
        }
        else { /*PASS*/ }

    }   // MakeSlot()

#if PHOTON_NETWORK_ENABLE
    [PunRPC]
    public void ChangePositionItem(Transform topParentTrans_)
    {
        isMake = true;
        topParentTrans = topParentTrans_;
        slotClone.transform.SetParent(transform);
        slotClone_.transform.SetParent(transform);
        // 위 SetParent 오류가 생길수도 있는 것임 23.09.27 없으면 주석삭제할거임
    }
#else
    public void ChangePositionItem(Transform topParentTrans_)
    {
        topParentTrans = topParentTrans_;
        slotClone.transform.SetParent(transform);
        slotClone_.transform.SetParent(transform);
        // 위 SetParent 오류가 생길수도 있는 것임 23.09.27 없으면 주석삭제할거임
    }
#endif      // PHOTON_NETWORK_ENABLE


}   //NameSpace
