using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static SG_Item;

public class PlayerController : MonoBehaviourPun
{
    private Rigidbody rb;
    private Animator animator;
    private float aniSpeed = 0;

    private float horizontal;
    private float vertical;
    public int speed = 5;

    public bool doSomething = false;

    // 플레이어의 직업
    public static bool isSurvivor;
    public static bool isTrator;
    // 플레이어의 직업

    public GameObject weapon;       // 무기 들었을때 공격범위
    public GameObject fist;         // 주먹일때 공격범위
    public GameObject ui;           // 파워 게이지 ui
    private PlayerInventory playerInventory;
    public GameObject _playerInventory;
    public SG_PlayerActionControler playerActionControler;
    public SG_Item playerItem;
    //public GameObject cameraObject;

    public Transform itemInHand;
    public Transform inven;
    private bool itemSet = false;


    private RaycastHit hitInfo;
    private Vector3 moveVec;

    private float doingTime;    // 행동을 한 시간 체크
    private float startTime;
    private bool shouldStartTiming = false;
    private int doingCase;      // 뭘 하는 도중인지

    private UiFollowPlayer uiFollowPlayer;

    public PlayerHealth health;    // 본인의 PlayerHealth
    public PlayerHealth playerHealth;   // 남의 PlayerHealth

    // 공격관련
    private float attackPower;  // 마우스로 공격 차지
    public bool isAttack;
    public float damage;         // 줄 데미지
    public bool eat = false;       // 임시로 넣은것, 나중에 음식을 먹으면 on, 회복 후 off로 재활용
    private bool hand = false;      // 임시 : 손에 무기 들었는지
    // 공격관련

    private CameraFollow cameraFollow;

    public SG_Inventory inventoryClass;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        doingTime = 0;

        playerActionControler = transform.GetComponent<SG_PlayerActionControler>();

        playerInventory = _playerInventory.GetComponent<PlayerInventory>();

        uiFollowPlayer = ui.GetComponent<UiFollowPlayer>();

        health = transform.GetComponent<PlayerHealth>();

        //playerItem = GetComponent<SG_Item>();

        if (photonView.IsMine)
        {
            CinemachineVirtualCamera followCam = FindObjectOfType<CinemachineVirtualCamera>();
            followCam.LookAt = transform;           
            cameraFollow = transform.GetComponent<CameraFollow>();
            cameraFollow.playerController = this;
            cameraFollow.toFallow = transform;
            //followCam.LookAt = transform;
        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
        { return; }
        animator.SetBool("attack", isAttack);
        if (!doSomething && !health.isDead)
        {
            PlayerMove();
            if (!health.isDown)
            {
                PLayerIsClick();
            }
        }

        if(Input.GetKeyDown(KeyCode.Q))
        {
            if(playerInventory.handItemClone != null)
            {
                //Drop();
                playerInventory.MissItem();
                playerInventory.Drop();
            }
        }

        if (shouldStartTiming)
        {
            doingTime = Time.time - startTime;
        }

       // itemInHand.Item = playerItem.itemType

        Debug.DrawRay(transform.position, transform.forward * 1.0f, Color.magenta);

        if (Input.GetKeyDown(KeyCode.E) && !doSomething && !health.isDead && !health.isDown)
        {
            if (!doSomething && !Input.GetMouseButton(0))
            {
                // 플레이어의 앞에 있는 물체를 판별
                if (Physics.Raycast(transform.position, transform.forward, out hitInfo, 1.0f))
                {

                    if (hitInfo.collider.gameObject.tag == "Player")
                    {
                        playerHealth = hitInfo.collider.gameObject.GetComponent<PlayerHealth>();
                        bool isPlayerDown = playerHealth.isDown;
                        bool isPlayerDeath = playerHealth.isDead;
                        if (isPlayerDown && !isPlayerDeath)
                        {
                            StartCoroutine(PressE());
                        }
                    }
                    if (hitInfo.collider.gameObject.tag == "Warehouse")     // 아래 else와 합쳐도 될듯?
                    {
                        StartCoroutine(PressE());
                    }
                    else
                    {
                        StartCoroutine(PressE());
                    }
                }
                // 플레이어의 앞에 있는 물체를 판별
                else
                {   }
            }

        }
        else if (Input.GetKey(KeyCode.E) && doSomething && !health.isDead && !health.isDown)
        {
            uiFollowPlayer.Gauge(60);

        }
        else if (Input.GetKeyUp(KeyCode.E) && doSomething && !health.isDead && !health.isDown)  // E키를 떼면 작업을 멈추기
        {
            shouldStartTiming = false;
            doingTime = 0;
            doSomething = false;
            animator.SetBool("DoSomething", doSomething);
            animator.SetBool("Looting", false);
            uiFollowPlayer.currentValue = 0;
            uiFollowPlayer.LoadingBar.fillAmount = uiFollowPlayer.currentValue / 100;
        }

        if (isAttack && health.health <= 0)
        {
            isAttack = false;
            animator.SetBool("attack", isAttack);
        }

        if (doingTime > 2 && doSomething)       // 작업이 끝났을 때 행동을 멈추기
        {
            shouldStartTiming = false;
            Debug.Log("111");

            doingTime = 0;
            uiFollowPlayer.currentValue = 0;
            uiFollowPlayer.LoadingBar.fillAmount = uiFollowPlayer.currentValue / 100;
            doSomething = false;
            animator.SetBool("DoSomething", doSomething);

            DoSomething();
        }
        else if (doingTime > 2 && eat)       // 작업이 끝났을 때 행동을 멈추기
        {
            shouldStartTiming = false;
            Debug.Log("123");
            doingTime = 0;
            uiFollowPlayer.currentValue = 0;
            uiFollowPlayer.LoadingBar.fillAmount = uiFollowPlayer.currentValue / 100;
            doSomething = false;
            animator.SetBool("DoSomething", doSomething);

            DoSomething();
        }
    }

    private void DoSomething()
    {
        if (playerInventory.foodInHand)
        {
            // 먹은 음식에 따른 회복
            //playerInventory.playerinventory.slots[(int)inven].item
            //playerHealth.health += playerInventory.hp;
            //playerHealth.cold += playerInventory.cold;
            //playerHealth.hunger += playerInventory.hunger;

            playerInventory.Eat();
            playerInventory.MissItem();
            health.RestoreHealth(playerInventory.hp);
            health.cold += playerInventory.cold;
            health.hunger += playerInventory.hunger;
            
            animator.SetBool("Eat", false);

            eat = false;

            playerInventory.foodInHand = false;
        }

        // 여기에 완료됐을때 상호작용을 실행할 코드를 추가해야됨
        if (doingCase == 1)
        {
            //아이템
            photonView.RPC("PickUp", RpcTarget.All);
            
            animator.SetBool("Looting", false);

            doingCase = 0;
        }
        else if (doingCase == 2)    // 다운된 플레이어일때 살림
        {
            //플레이어
            Collider hitCollider = hitInfo.collider;

            GameObject toRevive = hitCollider.gameObject;

            playerHealth = toRevive.GetComponent<PlayerHealth>();

            photonView.RPC("Revive", RpcTarget.All, playerHealth);

            doingCase = 0;
        }
        else if (doingCase == 3)
        {
            //상자

            doingCase = 0;
        }
    }

    [PunRPC]
    private void Revive(PlayerHealth playerHealth)
    {
        playerHealth.health = 20;
        playerHealth.playerDown = 100;
        playerHealth.isDown = false;
    }

    //private void duplicationChek()
    //{
    //    for (int i = 0; playerInventory.inventory.Count > i; i++)
    //    {
    //        SG_Item item = playerInventory.inventory[i].transform.GetChild(0).GetComponent<SG_Item>();
    //        SG_Item.ItemType itemType = item.itemType;

    //        if ( itemType == SG_Item.ItemType.Used ) // 소비탬일떄
    //        {
    //            if (playerInventory.inventory[i].transform.GetChild(0).name == hitInfo.transform.name) // 아이탬이 같을때
    //            {
    //                if (playerInventory.inventory[i].transform.childCount < 3) // 3개 미만일떄
    //                {
    //                    AddItem(playerInventory.inventory[i].transform);
    //                }
    //                else if(playerInventory.inventory[i].transform.childCount >= 3/*탬창이 비었을때*/)
    //                {

    //                }
    //            }
    //            else if(playerInventory.inventory[i].transform.GetChild(0).name != hitInfo.transform.name/*아이탬이 다를때*/)
    //            {
    //                for(int j = 0; j < playerInventory.inventory.Count; j++)
    //                {

    //                }
    //            }
    //        }
    //        else
    //        {

    //        }

    //        // 인벤이 꽉 차 있을때

    //    }
    //}

    [PunRPC]
    private void PickUp()
    {
        /*  LEGACY
        //itemInHand = hitInfo.transform;
        //itemInHand.transform.SetParent(inven); //(_inven);

        //Collider collider = hitInfo.collider;
        //Rigidbody itemRb = itemInHand.transform.GetComponent<Rigidbody>();
        //collider.enabled = false;               // 콜라이더 컴포넌트 끄고
        //Destroy(itemRb);                        // 리지드바디 없앰 ( 손 따라오게 하기 위해)
        //itemInHand.transform.localPosition = Vector3.zero;
        //itemInHand.transform.localRotation = Quaternion.identity;
        //itemInHand.transform.localScale = Vector3.one;
        LEGACY  */
        playerActionControler.TryAction();
    }
    #region
    private void AddItem(Transform _inven)
    {
        itemInHand = hitInfo.transform;
        itemInHand.transform.SetParent(_inven);

        Collider collider = hitInfo.collider;
        Rigidbody itemRb = itemInHand.transform.GetComponent<Rigidbody>();
        collider.enabled = false;               // 콜라이더 컴포넌트 끄고
        Destroy(itemRb);                        // 리지드바디 없앰 ( 손 따라오게 하기 위해)
        itemInHand.transform.localPosition = Vector3.zero;
        itemInHand.transform.localRotation = Quaternion.identity;
        itemInHand.transform.localScale = Vector3.one;
    }

    private void Drop()
    {
        //Collider collider = hitInfo.collider;
        itemInHand.GetComponent<Collider>().enabled = true;

        Rigidbody newRigidbody = itemInHand.transform.AddComponent<Rigidbody>();        // 리지드바디 새로 달기
       
        itemInHand.SetParent(null);         //손 오브잭트와 분리
        itemInHand = null; 
    }
    #endregion
    // 아이템 관련 (이제 사용 X)

    #region
    private void PlayerMove()          
    {
        // ��ǥ�̵�
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        moveVec = new Vector3(horizontal, 0, vertical).normalized;

        transform.position += moveVec * speed * Time.deltaTime;

        transform.LookAt(transform.position + moveVec);
        // ��ǥ�̵�

        // �ִϸ��̼�
        if (Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0)
        {
            aniSpeed = Mathf.MoveTowards(aniSpeed, 1, Time.deltaTime * 3);
        }
        else
        {
            aniSpeed = Mathf.MoveTowards(aniSpeed, 0, Time.deltaTime * 3);
        }
        animator.SetFloat("Speed", aniSpeed);
        // �ִϸ��̼�
    }
    #endregion
    // 플레이어 움직임

    // 공격
    #region
    private void PLayerIsClick()
    {
        if (Input.GetMouseButton(0) && !isAttack && !health.isInside && !playerInventory.foodInHand)   // �߰����� : �տ� ������ ������
        {
            Attack();
            uiFollowPlayer.Gauge(120);
        }       
        else if (Input.GetMouseButtonUp(0) && !isAttack && !health.isInside)    // �߰����� : �տ� ������ ������
        {
            if (!playerInventory.foodInHand)
            {
                isAttack = true;
                uiFollowPlayer.currentValue = 0;
                uiFollowPlayer.LoadingBar.fillAmount = uiFollowPlayer.currentValue / 100;
                StartCoroutine(EndAttack());
            }
            else if (playerInventory.foodInHand && eat)
            {
                animator.SetBool("Eat", false);
                eat = false;
                doSomething = false;
                shouldStartTiming = false;
                uiFollowPlayer.currentValue = 0;
                uiFollowPlayer.LoadingBar.fillAmount = uiFollowPlayer.currentValue / 100;
            }
        }
        else if (Input.GetMouseButton(0) && !isAttack && playerInventory.foodInHand)//(������ �տ� ������)
        {
            uiFollowPlayer.Gauge(120);
            StartCoroutine(Eat());
        }
    }

    private void Attack()
    {
        if(!playerInventory.weapomInHand)
        {       //������ �ȵ������
            animator.SetBool("Weapon", false);
            animator.SetBool("Charge", true);

            attackPower += Time.deltaTime;
        }
        else
        {       //������ �������
            animator.SetBool("Weapon", true);
            animator.SetBool("Charge", true);

            attackPower += Time.deltaTime;
        }
    }

    private IEnumerator EndAttack()
    {        
        if (!playerInventory.weapomInHand)
        {       //������ �ȵ������
            if (attackPower >= 1f)           // ������ ��ġ ���� �ʿ�
            {
                attackPower = 1f;
            }
            damage = (1.0f + attackPower * (4 - 1)); 
           

            animator.SetBool("Charge", false);

            animator.SetBool("attack", isAttack);
            yield return new WaitForSeconds(0.2f);
            fist.SetActive(true);
            yield return new WaitForSeconds(0.3f);
            fist.SetActive(false);

            isAttack = false;
            attackPower = 0;
            Debug.Log(damage);

        }
        else
        {       //������ �������
            if (attackPower >= 1f)           // ������ ��ġ ���� �ʿ�
            {
                attackPower = 1f;
            }
            damage = playerInventory.damage * (1.0f + attackPower * (4 - 1));

            animator.SetBool("Charge", false);

            animator.SetBool("attack", isAttack);
            yield return new WaitForSeconds(0.1f);
            weapon.SetActive(true);
            yield return new WaitForSeconds(0.6f);
            weapon.SetActive(false);

            isAttack = false;
            attackPower = 0;
            Debug.Log(damage);
        }
    }

    private IEnumerator Eat()
    {
        doSomething = true;
        shouldStartTiming = true;
        startTime = Time.time;
        animator.SetBool("Eat", true);
        eat = true;
        yield return null;

    }

    #endregion
    // 공격

    private IEnumerator PressE()
    {
        shouldStartTiming = true;
        startTime = Time.time;
        if (hitInfo.collider.gameObject.tag == "Item") // 루팅모션
        {
            doSomething = true;
            animator.SetBool("DoSomething", doSomething);
            animator.SetBool("Looting", true);
            yield return new WaitForSeconds(1.2f);

            doingCase = 1;
        }
        else if(hitInfo.collider.gameObject.tag == "Player")
        {
            animator.SetFloat("Speed", 0);
            doSomething = true;
            animator.SetBool("DoSomething", doSomething);
            yield return new WaitForSeconds(1.5f);

            doingCase = 2;
        }
        else if (hitInfo.collider.gameObject.tag == "Warehouse") // 작업모션
        {
            animator.SetFloat("Speed", 0);
            doSomething = true;
            animator.SetBool("DoSomething", doSomething);
            yield return new WaitForSeconds(1.5f);

            doingCase = 3;
        }
    }
       
    // 실내 여부
    #region
    // ###########################
    // isMine일때만 실행
    // ###########################

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.IsMine)
        { return; }
        if (other.CompareTag("Building"))           // 플레이어가 건물 안으로 들어갔으면
        {
            //cameraObject = GameObject.Find("CM vcam1");
            //CameraFollow cameraFollow = cameraObject.gameObject.GetComponent<CameraFollow>(); // 카메라를 둘 오브잭트를 찾아 카메라를 둠
            cameraFollow.inside = other.gameObject;
            cameraFollow.isInside = true;           
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (!photonView.IsMine)
        { return; }
        if (other.CompareTag("Building"))
        {
            //cameraObject = GameObject.Find("CM vcam1");
            //CameraFollow cameraFollow = cameraObject.gameObject.GetComponent<CameraFollow>(); // 카메라를 둘 오브잭트를 찾아 카메라를 둠
            //bool isInside = cameraFollow.isInside;
            cameraFollow.isInside =  false;
            
            //if (photonView.isMine)
            //{
            //    CinemachineVirtualCamera followCam = FindObjectOfType<CinemachineVirtualCamera>();
            //    followCam.LookAt = transform;
            //}
        }
    }
    #endregion
    // 실내 여부

}
//animator.SetLayerWeight(1, 0.0f);       // 두번째(1) 레이어의 애니메이션을 멈춤