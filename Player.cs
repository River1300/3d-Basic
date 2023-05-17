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
    Weapon equipWeapon;
    // [13]. 필요 속성 : 아이템 변수, 아이템 Max 변수
    public int maxAmmo;
    public int maxHealth;
    public int maxCoin;
    public int maxHasGrenade;
    public int ammo;
    public int health;
    public int coin;
    public int hasGrenade;
    // [15]. 필요 속성 : 공전하는 오브젝트를 관리하기 위한 변수 -> Orbit
    public GameObject[] grenade;
    // [17]. 필요 속성 : 키 플래그, 공격 상태 플래그, 공격 딜레이
    bool fDown;
    bool isFireReady = true;
    float fireDelay;
    // [20]. 필요 속성 : 재장전 키 플래그, 현재 장전중 플래그
    bool rDown;
    bool isReload;
    // [21]. 필요 속성 : 메인 카메라
    public Camera followCamera;
    // [22]. 필요 속성 : 플래그
    bool isBoarder;
    // [24]. 필요 속성 : 수류탄 프리팹, 수류탄 투척 버튼 플래그
    public GameObject grenadeObj;
    bool gDown;
    // [26]. 필요 속성 : 현재 피격 상태임을 체크할 플래그
    bool isDamage;
    MeshRenderer[] meshs;

    void Awake()
    {   
        rigid = GetComponent<Rigidbody>();
        // [2]. 1) 애니매이터를 Player 자식이 가지고 있으므로 자식으로 부터 컴포넌트를 받아와 초기화
        anim = GetComponentInChildren<Animator>();
        // [26]. 4) 복수형으로 모든 파츠의 컴포넌트를 받아온다.
        meshs = GetComponentsInChildren<MeshRenderer>();
    }

    void Update()
    {
        // [5]. 1) Update()함수의 로직들을 기능 별로 묶어서 함수를 만든다.
        GetInput();
        Move();
        Turn();
        Jump();
        Grenade();
        Attack();
        Reload();
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

        // [17]. 10) 발사 버튼을 입력 받는다.
        fDown = Input.GetButton("Fire1");

        // [20]. 2) 장전 버튼을 입력 받는다.
        rDown = Input.GetButtonDown("Reload");

        // [24]. 1) 수류탄 버튼을 입력 받는다.
        gDown = Input.GetButtonDown("Fire2");
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
        if(isSwap || isReload || !isFireReady)
        {
            dirVec = Vector3.zero;
        }

        // [1]. 3) 현재 위치 + 방향 * 속도 * 시간은 미래 위치
        // [2]. 4) 걷기 입력이 눌리고 있다면 속도를 낮춘다.
        // [22]. 5) 플레이어가 벽을 향해 더이상 갈 수 없도록 미래의 위치 값을 더하지 않는다.
        if(!isBoarder)
            transform.position += dirVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        // [2]. 3) 애니매이터로 파라미터를 전달한다.
        anim.SetBool("isRun", dirVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    void Turn()
    {
        // [3]. 1) 플레이어의 방향에 따라 오브젝트를 회전시킨다. -> Follow
        transform.LookAt(transform.position + dirVec);

        if(fDown)
        {
            // [21]. 1) 스크린에서 월드로 Ray를 쏘도록 한다.
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            // [21]. 2) 충돌한 오브젝트의 정보를 받는다.
            RaycastHit rayHit;
            // [21]. 3) ray로 부터 충돌한 지점의 정보를 rayHit가 반환 받았다면
            if(Physics.Raycast(ray, out rayHit, 100))
            {
                // [21]. 4) 포인트 지점을 향하는 방향 벡터를 구한다.
                Vector3 nextVec = rayHit.point - transform.position;
                // [21]. 5) 이때 y축 값은 0으로 고정한다.
                nextVec.y = 0;
                // [21]. 6) 자신의 위치에서 방향벡터 만큼 이동한 위치를 바라보도록 한다.
                transform.LookAt(transform.position + nextVec);
            }
        }
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

    void Grenade()
    {
       // [24]. 2) 수류탄을 던지지 못하는 경우를 제어문으로 만든다.
       if(hasGrenade == 0) return;
       if(gDown && !isReload && !isSwap)
       {
            // [24]. 3) 마우스 포인트 좌표로 수류탄을 던지도록 Ray를 쏜다.
            Ray ray = followCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayHit;
            if(Physics.Raycast(ray, out rayHit, 100))
            {
                Vector3 nextVec = rayHit.point - transform.position;
                nextVec.y = 12;

                // [24]. 4) 수류탄 객체 생성
                GameObject instantGrenade = Instantiate(grenadeObj, transform.position, transform.rotation);
                // [24]. 5) 수류탄 객체의 리지드 바디를 받아와서 던진다.
                Rigidbody rigidGrenade = instantGrenade.GetComponent<Rigidbody>();
                rigidGrenade.AddForce(nextVec, ForceMode.Impulse);
                // [24]. 6) 수류탄에 회전을 주도록 한다.
                rigidGrenade.AddTorque(Vector3.back * 10, ForceMode.Impulse);
                // [24]. 7) 수류탄 갯수와 공전하는 수류탄을 비활성화
                hasGrenade--;
                grenade[hasGrenade].SetActive(false);
            }
       }
    }

    void Attack()
    {
        // [17]. 11) 일단 현재 무기를 장착하고 있지 않다면 반환한다.
        if(equipWeapon == null) return;
        // [17]. 12) 공격 딜레이에 매 프레이마다 시간을 더해준다.
        fireDelay += Time.deltaTime;
        // [17]. 13) 현재 장착한 무기의 딜레이와 쌓인 공격 딜레이를 비교하여 공격 준비 플래그에 저장한다.
        isFireReady = equipWeapon.rate < fireDelay;
        // [17]. 14) 공격키가 눌렸고, 공격 준비가 된 상태일 경우 무기 사용 함수를 호출
        if(fDown && isFireReady && !isSwap && !isDodge)
        {
            equipWeapon.Use();
            // [17]. 15) 만들 예정인 애니매이션 파라미터를 전달
            anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
            fireDelay = 0;
        }
    }

    void Reload()
    {
        // [20]. 3) 무기가 없거나 장착한 무기가 근접 무기이거나 총알이 0개면 반환
        if(equipWeapon == null) return;
        if(equipWeapon.type == Weapon.Type.Melee) return;
        if(ammo == 0) return;

        if(rDown && !isJump && !isDodge && !isSwap && isFireReady)
        {
            // [20]. 4) 애니매이션에 파라미터를 전달하고 장전 상태로 전환
            anim.SetTrigger("doReload");
            isReload = true;
            Invoke("ReloadOut", 3f);
        }
    }

    void ReloadOut()
    {
        // [20]. 5) 장전이 끝날 때 탄창이 채워지는데 보유한 탄창을 확인하여 탄창에 넣을 총알 갯수를 지정
        int reAmmo = ammo < equipWeapon.maxAmmo ? ammo : equipWeapon.maxAmmo;
        equipWeapon.curAmmo = reAmmo;
        ammo -= reAmmo;
        isReload = false;
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
                // [17]. 8) 스크립트를 가지고 있는 게임 오브젝트로 수정
                equipWeapon.gameObject.SetActive(false);
            }
            // [12]. 10) 장착 중인 무기 번호를 저장한다.
            equipWeaponIndex = weaponIndex;
            // [12]. 4) 장착할 무기를 저장한다.
            // [17]. 9) 해당 무기가 보유하고 있는 Weapon 스크립트를 전달
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);
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

    void FreezeRotation()
    {
        // [22]. 2) 회전하는 속도를 0으로 맞춘다.
        rigid.angularVelocity = Vector3.zero;
    }

    void StopToWall()
    {
        // [22]. 3) 플레이어가 앞으로 Ray를 쏜다.
        Debug.DrawRay(transform.position, transform.forward * 5, Color.green);
        // [22]. 4) Raycast로 충돌한 오브젝트를 확인하고 Wall일 경우 true를 준다.
        isBoarder = Physics.Raycast(transform.position, transform.forward, 5, LayerMask.GetMask("Wall"));
    }

    // [22]. 1) 플레이어가 자동으로 회전하는 것을 맊는다.
    void FixedUpdate()
    {
        FreezeRotation();
        StopToWall();
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

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item")
        {
            Item item = other.GetComponent<Item>();
            switch(item.type)
            {
                case Item.Type.Ammo:
                    ammo += item.value;
                    if(ammo > maxAmmo) ammo = maxAmmo;
                    break;
                case Item.Type.Coin:
                    coin += item.value;
                    if(coin > maxCoin) coin = maxCoin;
                    break;
                case Item.Type.Heart:
                    health += item.value;
                    if(health > maxHealth) health = maxHealth;
                    break;
                case Item.Type.Grenade:
                    // [15]. 5) 0부터 수류탄을 활성화 시킨다. -> Weapon
                    grenade[hasGrenade].SetActive(true);
                    hasGrenade += item.value;
                    if(hasGrenade > maxHasGrenade) hasGrenade = maxHasGrenade;
                    break;
            }
            Destroy(other.gameObject);
        }
        else if(other.tag == "EnemyBullet")
        {
            if(!isDamage)
            {
                // [26]. 1) 적 총알에 피격되었을 경우 데미지 만큼 체력 손실
                Bullet enemyBullet = other.GetComponent<Bullet>();
                health -= enemyBullet.damage;
                // [29]. 4) 적 총알로 부터 리지드바디를 탐색한 뒤 있다면 제거
                if(other.GetComponent<Rigidbody>() != null)
                    Destroy(other.gameObject);
                // [26]. 2) 피격 이벤트 진행
                StartCoroutine(OnDamage());
            }
        }
    }

    IEnumerator OnDamage()
    {
        // [26]. 3) 현재 피격 상태이다.
        isDamage = true;
        // [26]. 5) 피격 상태일 때 색을 바꿔준다.
        foreach(MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.yellow;
        }

        yield return new WaitForSeconds(1f);

        foreach(MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.white;
        }
        isDamage = false;
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
