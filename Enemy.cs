using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// [25]. 2) 네비게이션을 받기 위한 전처리기
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    // [23]. 필요 속성 : 피격 대상의 최대 체력과 현재 체력, 리지드바디, 콜라이더, 마테리얼
    public int maxHealth;
    public int curHealth;
    Rigidbody rigid;
    BoxCollider boxCollider;
    
    // [32]. 보스의 모든 파츠를 담기 위해 배열로 변경
    MeshRenderer[] meshs;

    // [25]. 필요 속성 : 네비게이션, 플레이어 위치
    public Transform target;
    NavMeshAgent nav;
    Animator anim;
    public bool isChase;
    // [27]. 필요 속성 : 콜라이더, 현재 공격 중 플래그
    public BoxCollider meleeArea;
    public bool isAttack;

    // [28]. 필요 속성 : 몬스터 타입
    // [32]. 보스 타입 추가
    public enum Type { A, B, C, D };

    public Type enemyType;
    // [29]. 필요 속성 : 미사일 프리팹
    public GameObject bullet;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        // [23]. 4) 마테리얼은 메쉬랜더러가 가지고 있다.
        // [25]. 1) 적 객체는 MeshRenderer컴포넌트를 자식이 가지고 있다.
        meshs = GetComponentsInChildren<MeshRenderer>();

        nav = GetComponent<NavMeshAgent>();
        // [25]. 5) 자식 오브젝트가 가지고 있는 애니메이터 컨포넌트를 받아온다.
        anim = GetComponentInChildren<Animator>();

        // [32]. 1) 보스는 플레이어를 추적하여 이동하지 않도록 함수 호출을 맊는다.
        if(enemyType != Type.D)
            Invoke("ChaseStart", 2f);
    }

    void ChaseStart()
    {
        isChase = true;
        anim.SetBool("isWalk", true);
    }

    void Update()
    {
        // [25]. 6) 추적 중일 때만 이동한다.
        // [26]. 6) 플래그를 사용해도 추적만 놓칠뿐 계속 이동하므로 활성화 시에만 움직이도록 한다.
        // [32]. 2) 보스는 추적하지 않는다.
        if(nav.enabled && enemyType != Type.D)
        {
            // [25]. 3) 매 프레임 마다 플레이어를 향해 이동한다.
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }
    }

    // [25]. 4) 물리적 회전을 막는다.
    void FixedUpdate()
    {
        // [27]. 1) 플레이어를 타겟팅 한다.
        Targeting();
        FreezeVelocity();
    }

    void Targeting()
    {
        // [32]. 3) 보스는 일반 몹과 동일한 방식의 타겟팅을 하지 않는다.
        if(enemyType != Type.D)
        {
            float targetRadius = 1.5f;
            float targetRange = 3f;

            // [28]. 1) 몬스터 타입에 따라서 공격 범위와 타겟팅 구역의 사이즈가 다른다.
            switch (enemyType)
            {
                case Type.A:
                    targetRadius = 1.5f;
                    targetRange = 3f;
                    break;
                case Type.B:
                    targetRadius = 1f;
                    targetRange = 12f;
                    break;
                case Type.C:
                    targetRadius = 0.5f;
                    targetRange = 25f;
                    break;
            }

            // [27]. 2) 적이 앞을 향해 스피어 캐스트를 발사
            RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, targetRadius, transform.forward, targetRange, LayerMask.GetMask("Player"));
            // [27]. 3) 범위 안에 플레이어가 감지되어다면 공격 함수 호출
            if(rayHits.Length > 0 && !isAttack)
            {
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        // [27]. 4) 일단 멈추고 공격
        isChase = false;
        isAttack = true;
        anim.SetBool("isAttack", true);

        // [28]. 2) 몬스터 타입에 따라 각기 다른 공격을 한다.
        switch(enemyType)
        {
            case Type.A:
                // [27]. 5) 공격 범위 활성화
                yield return new WaitForSeconds(0.7f);
                meleeArea.enabled = true;
                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;
                yield return new WaitForSeconds(1f);
                break;
            case Type.B:
                // [28]. 3) 잠깐 멈추었다가 돌격
                yield return new WaitForSeconds(0.1f);
                rigid.AddForce(transform.forward * 20, ForceMode.Impulse);
                meleeArea.enabled = true;
                // [28]. 4) 빠른 속도로 돌진하다가 정지
                yield return new WaitForSeconds(0.5f);
                rigid.velocity = Vector3.zero;
                meleeArea.enabled = false;

                yield return new WaitForSeconds(2.5f);
                break;
            case Type.C:
                yield return new WaitForSeconds(0.5f);
                // [29]. 2) 총알을 인스턴트화
                GameObject instantBullet = Instantiate(bullet, transform.position, transform.rotation);
                // [29]. 3) 총알의 리지드바디를 받아와서 속도를 지정
                Rigidbody rigidBullet = instantBullet.GetComponent<Rigidbody>();
                rigidBullet.velocity = transform.forward * 20f;

                yield return new WaitForSeconds(2.5f);
                break;
        }

        
        // [27]. 6) 공격이 끝났다면 다시 추적
        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack", false);
    }

    void FreezeVelocity()
    {
        // [25]. 8) 추적 중일 때만 물리적 계산을 중지한다.
        if(isChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // [23]. 1) 근접 공격 대미지와 원거리 공격 대미지를 나누어 받는다.
        if(other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            // [23]. 7) 넉백 위치는 자신의 위치에서 피격 위치를 뺀 값이다.
            Vector3 reactVec = transform.position - other.transform.position;
            // [23]. 3) 피격 코루틴 함수 호출
            StartCoroutine(OnDamage(reactVec, false));
        }
        else if(other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;

            Vector3 reactVec = transform.position - other.transform.position;
            // [23]. 9) 피격 이후 총알은 제거된다.
            Destroy(other.gameObject);

            StartCoroutine(OnDamage(reactVec, false));
        }
    }

    // [24]. 12) 폭탄에 맞아 체력에 달고 넉백된다.
    public void HitByGrenade(Vector3 explosionPos)
    {
        curHealth -= 100;
        Vector3 reactVec = transform.position - explosionPos;
        StartCoroutine(OnDamage(reactVec, true));
    }

    // [24]. 13) 폭탄에 맞아 사망한 적은 좀더 격정적이게 죽도록 플래그를 만든다.
    IEnumerator OnDamage(Vector3 reactVec, bool isGrenade)
    {
        // [23]. 5) 피격될 때 색을 바꾸고 일정 시간 뒤에 색을 되돌린다.
        // [32]. 5) 모든 파츠를 순회하며 색을 바꾼다.
        foreach(MeshRenderer mesh in meshs)
            mesh.material.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        if(curHealth > 0){
            foreach(MeshRenderer mesh in meshs)
                mesh.material.color = Color.white;
        }
        else{
            // [23]. 6) 체력이 0이라면 레이어를 바꾸고 피격되지 않도록 한다.
            foreach(MeshRenderer mesh in meshs)
                mesh.material.color = Color.gray;

            gameObject.layer = 12;
            // [25]. 7) 죽을 때 죽는 애니매이션 출력
            isChase = false;
            nav.enabled = false;
            anim.SetTrigger("doDie");

            if(isGrenade)
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up * 3;

                // [24). 14) 더 격정적인 죽음을 위해 회전을 주며 튕겨저 나가게 한다.
                rigid.freezeRotation = false;
                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
                rigid.AddTorque(reactVec * 15, ForceMode.Impulse);
            }
            else
            {
                // [23]. 8) 방향 값을 일반화 시키고 넉백 값에 약간의 높이를 추가한다.
                reactVec = reactVec.normalized;
                reactVec += Vector3.up;

                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            }
            // [32]. 4) 보스는 시체를 남기지 않는다.
            if(enemyType != Type.D)
                Destroy(gameObject, 4);
        }
    }
}
