using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject[] weapons;
    public GameObject[] grenade;

    public Weapon equipWeapon;

    public Camera followCamera;

    public int maxAmmo;
    public int maxHealth;
    public int maxCoin;
    public int maxHasGrenade;
    public int ammo;
    public int health;
    public int coin;
    public int hasGrenade;

    public float speed;

    public bool[] hasWeapons;

    GameObject nearObject;

    int equipWeaponIndex = -1;

    float hAxisRaw;
    float vAxisRaw;
    float fireDelay;

    bool wDown;
    bool jDown;
    bool isJump;
    bool isDodge;
    bool iDown;
    bool sDown1;
    bool sDown2;
    bool sDown3;
    bool isSwap;
    bool fDown;
    bool isFireReady = true;
    bool rDown;
    bool isReload;
    bool isBoarder;

    Vector3 dirVec;
    Vector3 dodgeVec;   // 회피 방향을 고정할 방향 변수

    Animator anim;

    Rigidbody rigid; 
    // [24]. 필요 속성 : 수류탄 프리팹, 수류탄 투척 버튼 플래그
    public GameObject grenadeObj;
    bool gDown;
    // [26]. 필요 속성 : 현재 피격 상태임을 체크할 플래그
    bool isDamage;
    MeshRenderer[] meshs;
    // [35]. 필요 속성 : 현재 쇼핑 중
    bool isShop;
    // [39]. 필요 속성 : 플레이어 점수
    public int score;
    // [42]. 필요 속성 : 죽었다는 플래그, 게임 매니저
    bool isDead;
    public GameManager gameManager;
    // [43]. 필요 속성 : 오디오 소스
    public AudioSource jumpSound;

    void Awake()
    {   
        rigid = GetComponent<Rigidbody>();
        // [2]. 1) 애니매이터를 Player 자식(MeshObject)이 가지고 있으므로 자식으로 부터 컴포넌트를 받아와 초기화
        anim = GetComponentInChildren<Animator>();
        // [26]. 4) 복수형으로 모든 파츠의 컴포넌트를 받아온다.
        meshs = GetComponentsInChildren<MeshRenderer>();

        //Debug.Log(PlayerPrefs.GetInt("MaxScore"));
        // [36]. 1) 플레이어 최고 점수를 기록
        //PlayerPrefs.SetInt("MaxScore", 11650);
    }

    void Update()
    {
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
        hAxisRaw = Input.GetAxisRaw("Horizontal");
        vAxisRaw = Input.GetAxisRaw("Vertical");

        wDown = Input.GetButton("Walk"); // 누른 순간이 아닌 누르고 있는 중
        jDown = Input.GetButtonDown("Jump");

        iDown = Input.GetButtonDown("Interaction");

        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");

        fDown = Input.GetButton("Fire1");
        rDown = Input.GetButtonDown("Reload");
        gDown = Input.GetButtonDown("Fire2");
    }

    void Move()
    {
        // [1]. 1) 입력받은 Vector의 방향만 사용하기 위해 크기를 정규화 시킨다.
        dirVec = new Vector3(hAxisRaw, 0, vAxisRaw).normalized;

        // [8]. 6) 회피 중 이라면 방향을 바꾸지 못하도록 이동 방향에 회피 방향값을 지정한다.
        if(isDodge)
            dirVec = dodgeVec;

        // [12]. 8) 무기를 교체 중일 떄는 움직임, 점프, 회피 모두 못하도록 이동 값을 Vector3.zero
        if(isSwap || isReload || !isFireReady || isDead)
            dirVec = Vector3.zero;

        // [22]. 5) 플레이어가 벽을 향해 더이상 갈 수 없도록 미래의 위치 값을 더하지 않는다.
        if(!isBoarder)
            // [1]. 2) 미래의 위치 = 현재 위치 + 방향 * 속도 * 시간
            // [2]. 3) 걷기 입력(wDown)이 눌리고 있다면 속도를 낮춘다.
            transform.position += dirVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        // [2]. 2) 입력된 움직임 값이 있다면 애니매이터로 파라미터를 전달한다.
        anim.SetBool("isRun", dirVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);
    }

    void Turn()
    {
        // [3]. 1) 유저가 바라보는 방향에 따라 플레이어 오브젝트를 회전시킨다.
        transform.LookAt(transform.position + dirVec);

        if(fDown && !isDead)
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
        if(jDown && !isJump && dirVec == Vector3.zero && !isDodge && !isSwap && !isDead)
        {
            // [6]. 1) 점프 키가 눌렸다면 y축으로 힘을 가한다.
            rigid.AddForce(Vector3.up * 25, ForceMode.Impulse);
            isJump = true;

            // [7]. 1) 점프를 할 때 파라미터를 전달한다.
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");

            jumpSound.Play();
        }
    }

    void Grenade()
    {
       // [24]. 2) 수류탄을 던지지 못하는 경우를 제어문으로 만든다.
       if(hasGrenade == 0) return;
       if(gDown && !isReload && !isSwap && !isDead)
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
        // [17]. 6) 일단 현재 무기를 장착하고 있지 않다면 반환한다.
        if(equipWeapon == null) return;
        // [17]. 7) 각각의 공격 타입마다 연사 속도가 있고 이 연사 속도를 기준으로 공격한다.
        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;

        if(fDown && isFireReady && !isSwap && !isDodge && !isShop && !isDead)
        {
            // [17]. 8) 무기 스크립트의 사용 함수를 호출하고 애니매이션을 출력한다.
            equipWeapon.Use();
            anim.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
            fireDelay = 0;
        }
    }

    void Reload()
    {
        // [20]. 2) 장착한 무기가 없거나, 근접 무기이거나 총알이 0개면 반환
        if(equipWeapon == null) return;
        if(equipWeapon.type == Weapon.Type.Melee) return;
        if(ammo == 0) return;

        if(rDown && !isJump && !isDodge && !isSwap && isFireReady && !isDead)
        {
            // [20]. 3) 애니매이션에 파라미터를 전달하고 장전 시간 중 다른 행동을 맊기 위해 플래그에 true
            anim.SetTrigger("doReload");
            isReload = true;
            Invoke("ReloadOut", 3f);
        }
    }

    void ReloadOut()
    {
        // [20]. 4) 탄창을 채울 때 해당 무기 최대 탄창과 플레이어 보유 탄약을 비교하여 장전
        int reAmmo = ammo < equipWeapon.maxAmmo ? ammo : equipWeapon.maxAmmo;
        equipWeapon.curAmmo = reAmmo;
        ammo -= reAmmo;
        isReload = false;
    }

    void Dodge()
    {
        if(jDown && !isJump && dirVec != Vector3.zero && !isDodge && !isSwap && !isDead)
        {
            // [8]. 5) 회피 방향을 현재 이동 중인 방향으로 지정한다.
            dodgeVec = dirVec;
            // [8]. 1) 회피 중에는 짧은 시간 동안 더 빠르게 움직인다.
            speed *= 2;
            // [8]. 4) 회피 파라미터를 전달하고 회피 중 임을 플래그에 저장한다.
            anim.SetTrigger("doDodge");
            isDodge = true;
            // [8]. 3) Invoke로 회피 종료를 예약한다.
            Invoke("DodgeOut", 0.5f);
        }
    }

    void DodgeOut()
    {
        // [8]. 2) 회피가 종료되면 속도가 원상태로 돌아가고 회피 플래그는 false가 된다.
        speed *= 0.5f;
        isDodge = false;
    }

    void Swap()
    {
        // [12]. 9) 보유하고 있지 않는 무기는 장착할 수 없고, 이미 장착한 무기를 교체하지 않는다.
        if(sDown1 && (!hasWeapons[0] || equipWeaponIndex == 0)) return;
        if(sDown2 && (!hasWeapons[1] || equipWeaponIndex == 1)) return;
        if(sDown3 && (!hasWeapons[2] || equipWeaponIndex == 2)) return;

        // [12]. 1) 입력 받은 무기 키를 바탕으로 무기 인덱스를 지정한다.
        int weaponIndex = -1;
        if(sDown1) weaponIndex = 0;
        if(sDown2) weaponIndex = 1;
        if(sDown3) weaponIndex = 2;

        // [12]. 2) 무기 교체 버튼이 눌려야만 교체를 실시 한다.
        if((sDown1 || sDown2 || sDown3) && !isJump && !isDodge && !isDead)
        {
            // [12]. 5) 처음 게임을 시작하면 보유한 무기가 없는 상태로 진행이 됨으로 아무것도 없는 상태에서 비활성화 할 경우 null 에러가 발생한다.
            //      => 그러니 null이 아닐 때만 비활성화 로직을 실행하도록 한다.
            if(equipWeapon != null)
                // [12]. 4) 기존에 장착하고 있던 무기는 비활성화 시키고 그 이후에 입력 받은 무기를 활성화 한다.
                equipWeapon.gameObject.SetActive(false);

            equipWeaponIndex = weaponIndex;
            // [12]. 3) 입력 받은 무기 인덱스로 게임 오브젝트 배열을 활성화 한다.
            // [17]. 9) 해당 무기가 보유하고 있는 Weapon 스크립트를 전달
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);

            // [12]. 6) 무기 교체 애니매이션을 출력하고 현재 무기 교체 중이니 다른 행동을 하지 못하도록 플래그에 true
            anim.SetTrigger("doSwap");
            isSwap = true;
            // [12]. 7) 일정 시간이 지난 뒤 플래그에 false
            Invoke("SwapOut", 0.4f);
        }
    }

    void SwapOut()
    {
        isSwap = false;
    }

    void Interaction()
    {
        // [11]. 1) 상호작용 오브젝트가 비어있지 않다면!
        if(iDown && nearObject != null && !isJump && !isDodge && !isDead)
        {
            if(nearObject.tag == "Weapon")
            {
                // [11]. 2) 무기 번호(0, 1, 2)를 받아와서 현재 해당 무기를 가지고 있다고 체크
                Item item = nearObject.GetComponent<Item>();
                int weaponIndex = item.value;
                hasWeapons[weaponIndex] = true;
                // [11]. 3) 씬에 배치된 오브젝트 제거
                Destroy(nearObject);
            }
            else if(nearObject.tag == "Shop")
            {
                // [34]. 4) 상점 트리거에 진입할 때 상점 정보를 받아오고 함수를 호출한다.
                Shop shop = nearObject.GetComponent<Shop>();
                shop.Enter(this);
                // [35]. 7) 현재 쇼핑 중임을 상태 표시
                isShop = true;
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

    void FixedUpdate()
    {
        // [22]. 1) 플레이어가 다른 오브젝트와 충돌때 리지드바디로 인한 물리적인 움직임을 맊는다.
        FreezeRotation();
        StopToWall();
    }

    void OnCollisionEnter(Collision other)
    {
        // [6]. 2) 점프를 한 플레이어가 바닥에 착지 했다면 플래그에 false
        if(other.gameObject.tag == "Floor")
        {
            // [7]. 2) 착지하는 순간 bool 파라미터를 전달 함으로써 착지 애니매이션이 출력되도록 한다.
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Item")
        {
            // [14]. 1) 아이템의 타입을 통해 해당 아이템의 값을 증가 시킨다.
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
                    // [15]. 5) 보유하고 있는 수류탄 갯수만큼 0부터 수류탄을 활성화 시킨다.
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
                
                // [33]. 13) 플레이어가 보스의 내려찍기를 당할 때 보스의 콜라이더로 인해 맵 밖으로 튕겨 나가는 경우가 있다.
                //      => 보스의 점프 공격에 맞으면 OnDamage 함수에 전달한다.
                bool isBossAtk = other.name == "Boss Melee Area";

                // [26]. 2) 피격 이벤트 진행
                StartCoroutine(OnDamage(isBossAtk));
            }
            // [29]. 4) 적 총알로 부터 리지드바디를 탐색한 뒤 있다면 제거
            // [33]. 12) 플레이어 무적과 상관 없이 충돌한 적 총알은 제거된다.
            if(other.GetComponent<Rigidbody>() != null)
                Destroy(other.gameObject);
        }
    }

    IEnumerator OnDamage(bool isBossAtk)
    {
        // [26]. 3) 현재 피격 상태이다.
        isDamage = true;
        // [26]. 5) 피격 상태일 때 색을 바꿔준다.
        foreach(MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.yellow;
        }

        if(isBossAtk)
        {
            // [33]. 14) 내력찍기를 당했으면 뒤로 넉백 당한다.
            rigid.AddForce(transform.forward * -25, ForceMode.Impulse);
        }

        // [42]. 1) 죽었다면 애니매이션을 출력하고 게임 매니저에 알린다.
        if(health < 0 && !isDead)
        {
            OnDie();
        }

        yield return new WaitForSeconds(1f);

        foreach(MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.white;
        }
        isDamage = false;

        if(isBossAtk)
        {
            rigid.velocity = Vector3.zero;
        }
    }

    void OnDie()
    {
        anim.SetTrigger("doDie");
        isDead = true;
        gameManager.GameOver();
    }

    void OnTriggerStay(Collider other)
    {
        // [10]. 1) 상호 작용을 위해 가까이에 있는 오브젝트를 감지하여 변수에 저장한다.
        // [34]. 3) 플레이어가 상점 오브젝트를 저장한다.
        if(other.tag == "Weapon" || other.tag == "Shop")
        {
            nearObject = other.gameObject;
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Weapon")
        {
            // [10]. 2) 무기와 멀어지면 오브젝트에 널을 넣어 둔다.
            nearObject = null;
        }
        else if(other.tag == "Shop")
        {
            // [34]. 5) 나갈 때는 나가는 함수를 호출하도록 한다.
            Shop shop = nearObject.GetComponent<Shop>();
            shop.Exit();
            // [35]. 8) 쇼핑이 끝났다.
            isShop = false;
            nearObject = null;
        }
    }
}
