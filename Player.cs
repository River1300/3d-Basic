using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // [1]. 필요 속성 : 플레이어가 이동하는데 필요한 방향, 속도값
    public float speed;
    float hAxisRaw;
    float vAxisRaw;
    Vector3 dirVec;
    // [2]. 필요 속성 : 걷기 플래그, 애니매이터
    bool wDown;
    Animator anim;
    // [6]. 필요 속성 : 점프 키, 점프 플래그, 리지드바디
    bool jDown;
    bool isJump;
    Rigidbody rigid;
    // [8]. 필요 속성 : 회피 플래그, 회피 방향값
    bool isDodge;
    Vector3 dodgeVec;
    // [10]. 필요 속성 : 접촉한 오브젝트를 저장할 변수
    GameObject nearObject;
    // [11]. 필요 속성 : 상호 작용 플래그, 무기 오브젝트 배열, 무기 오브젝트 활성화 배열
    bool iDown;
    public GameObject[] weapons;
    public bool[] hasWeapons;
    // [12]. 필요 속성 : 무기 스왑 플래그, 장착 무기, 교체 중 플래그, 무기 번호
    int equipWeaponIndex = -1;
    bool sDown1;
    bool sDown2;
    bool sDown3;
    bool isSwap;
    GameObject equipWeapon;

    void Awake()
    {   
        rigid = GetComponent<Rigidbody>();
        // [2]. 1) 애니매이터를 Player 자식이 가지고 있으므로 자식으로 부터 컴포넌트를 받아와 초기화
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // [5]. 1) Update()함수의 로직들을 기능 별로 묶어서 함수를 만든다.
        GetInput();
        Move();
        Turn();
        Jump();
        Dodge();
        Swap();
        Interaction();
    }

    void GetInput()
    {
        // [1]. 1) 먼저 입력받은 방향 값을 저장한다.
        hAxisRaw = Input.GetAxisRaw("Horizontal");
        vAxisRaw = Input.GetAxisRaw("Vertical");

        // [2]. 2) 현재 걷기 입력키가 눌려지고 있는지를 플래그로 받는다.
        wDown = Input.GetButton("Walk");

        // [6]. 1) 점프 키 입력을 저장 받는다.
        jDown = Input.GetButtonDown("Jump");

        // [11]. 1) 상호 작용 키가 눌렸는지 변수에 저장
        iDown = Input.GetButtonDown("Interaction");

        // [12]. 1) 무기 교체 버튼을 입력 받는다.
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
    }

    void Move()
    {
        // [1]. 2) 방향을 일반화 하여 저장한다.
        dirVec = new Vector3(hAxisRaw, 0, vAxisRaw).normalized;

        // [8]. 8) 현재 회피 중 이라면 방향을 바꾸지 못하도록 현재 방향에 회피 방향값을 배정한다.
        if(isDodge)
        {
            dirVec = dodgeVec;
        }

        // [12]. 8) 무기를 교체 중일 떄는 움직임, 점프, 회피 모두 못한다.
        if(isSwap)
        {
            dirVec = Vector3.zero;
        }

        // [1]. 3) 현재 위치 + 방향 * 속도 * 시간은 미래 위치
        // [2]. 4) 걷기 입력이 눌리고 있다면 속도를 낮춘다.
        transform.position += dirVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        // [2]. 3) 애니매이터로 파라미터를 전달한다.
        anim.SetBool("isRun", dirVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    void Turn()
    {
        // [3]. 1) 플레이어의 방향에 따라 오브젝트를 회전시킨다. -> Follow
        transform.LookAt(transform.position + dirVec);
    }

    void Jump()
    {
        // [8]. 2) 회피 중에 점프키를 누르면 점프를 한다.
        if(jDown && !isJump && dirVec == Vector3.zero && !isDodge && !isSwap)
        {
            // [6]. 2) 점프 키가 눌렸다면 y축으로 힘을 가한다.
            rigid.AddForce(Vector3.up * 25, ForceMode.Impulse);

            // [7]. 1) 점프를 할 때 파라미터를 전달한다.
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");

            // [6]. 3) 현재 점프 중 임을 플래그에 저장한다.
            isJump = true;
        }
    }

    void Dodge()
    {
        // [8]. 1) 이동 중의 점프키를 누르면 회피를 한다.
        if(jDown && !isJump && dirVec != Vector3.zero && !isDodge && !isSwap)
        {
            // [8]. 7) 회피 방향 변수에 현재 방향 변수를 배정한다. -> Item
            dodgeVec = dirVec;
            // [8]. 3) 회피 중에는 짧은 시간 동안 더 빠르게 움직인다.
            speed *= 2;
            // [8]. 4) 회피 파라미터를 전달하고 회피 중 임을 플래그에 저장한다.
            anim.SetTrigger("doDodge");
            isDodge = true;
            // [8]. 6) Invoke로 회피 종료를 예약한다.
            Invoke("DodgeOut", 0.5f);
        }
    }

    void DodgeOut()
    {
        // [8]. 5) 회피가 종료되면 속도가 원상태로 돌아가고 회피 플래그는 false가 된다.
        speed *= 0.5f;
        isDodge = false;
    }

    void Swap()
    {
        // [12]. 9) 보유하고 있지 않는 무기는 장착할 수 없고, 이미 장착한 무기를 교체하지 않는다.
        if(sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0)) return;
        if(sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1)) return;
        if(sDown3 && (!hasWeapons[2] || equipWeaponIndex == 2)) return;

        int weaponIndex = -1;
        if(sDown1) weaponIndex = 0;
        if(sDown2) weaponIndex = 1;
        if(sDown3) weaponIndex = 2;

        // [12]. 2) 무기 교체 버튼이 눌렸다면 해당 무기를 활성화
        if((sDown1 || sDown2 || sDown3) && !isJump && !isDodge)
        {
            // [12]. 3) 무기를 교체하면 기존에 장착한 무기는 빼야 한다.
            if(equipWeapon != null)
            {
                equipWeapon.SetActive(false);
            }
            // [12]. 10) 장착 중인 무기 번호를 저장한다.
            equipWeaponIndex = weaponIndex;
            // [12]. 4) 장착할 무기를 저장한다.
            equipWeapon = weapons[weaponIndex];
            equipWeapon.SetActive(true);
            // [12]. 5) 무기 교체시 트리거 파라미터를 전달한다.
            anim.SetTrigger("doSwap");
            // [12]. 6) 현재 무기를 교체 중이라 한다.
            isSwap = true;

            Invoke("SwapOut", 0.4f);
        }
    }

    void SwapOut()
    {
        // [12]. 7) 무기 교체가 끝났음
        isSwap = false;
    }

    void Interaction()
    {
        // [11]. 2) 무기 근처에서 버튼이 눌렸다면 해당 무기 활성화
        if(iDown && nearObject != null && !isJump && !isDodge)
        {
            if(nearObject.tag == "Weapon")
            {
                // [11]. 3) 무기 번호를 받아와서 활성화
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;
                // [11]. 4) 무기를 활성화 한 뒤에는 씬에 배치된 오브젝트 제거
                Destroy(nearObject);
            }
        }
    }

    void OnCollisionEnter(Collision other)
    {
        // [6]. 4) 점프를 한 플레이어가 바닥에 찾지 했다면 점프 중이 아님을 플래그에 저장한다.
        if(other.gameObject.tag == "Floor")
        {
            // [7]. 2) 착지를 할 때 파라미터를 전달한다.
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        // [10]. 1) 가까이에 있는 오브젝트를 감지하여 변수에 저장한다.
        if(other.tag == "Weapon")
        {
            nearObject = other.gameObject;
        }
        Debug.Log(nearObject);
    }
    
    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Weapon")
        {
            nearObject = null;
        }
    }
}
