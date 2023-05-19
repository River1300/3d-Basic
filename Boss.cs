using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemy
{
    // [32]. 필요 속성 : 미사일 프리팹과 발사 위치들, 플레이어 예상 위치, 착지 위치, 플래그
    public GameObject missile;
    public Transform missilePortA;
    public Transform missilePortB;
    Vector3 lookVec;
    Vector3 tauntVec;
    public bool isLook;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        // [33]. 7) 보스는 추적을 하지 않는다.
        nav.isStopped = true;
        StartCoroutine(Think());
    }

    void Start()
    {
        isLook = true;
    }

    void Update()
    {
        // [32]. 1) 플레이어를 바라보고 있는 상황에서 타겟 추적
        if(isLook)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            // [32]. 2) 플레이어 입력 값을 바탕으로 플레이어 예상 위치를 바라본다.
            lookVec = new Vector3(h, 0, v) * 5f;
            transform.LookAt(target.position + lookVec);
        }
        else
        {
            // [33]. 8) 점프공격을 하기 위해 움직여 준다.
            nav.SetDestination(tauntVec);
        }
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);
        // [33]. 1) 보스의 패턴 번호를 랜덤하게 받는다.
        int ranType = Random.Range(0, 5);
        switch(ranType)
        {
            case 0:
            case 1:
                StartCoroutine(MissileShot());
                break;
            case 2:
            case 3:
                StartCoroutine(RockShot());
                break;
            case 4:
                StartCoroutine(Taunt());
                break;
        }
    }

    IEnumerator MissileShot()
    {
        // [33]. 2) 애니매이션 출력
        anim.SetTrigger("doShot");

        // [33]. 3) 미사일을 인스턴스화 한 뒤, 스크립트를 받아와서 미사일이 쫒아갈 플레이어 위치를 전달한다.
        yield return new WaitForSeconds(0.2f);
        GameObject instantMissileA = Instantiate(missile, missilePortA.position, missilePortA.rotation);
        BossMissile bossMissileA = instantMissileA.GetComponent<BossMissile>();
        bossMissileA.target = target;

        yield return new WaitForSeconds(0.2f);
        GameObject instantMissileB = Instantiate(missile, missilePortB.position, missilePortB.rotation);
        BossMissile bossMissileB = instantMissileB.GetComponent<BossMissile>();
        bossMissileB.target = target;

        if(isDead)
        {
            Destroy(instantMissileA);
            Destroy(instantMissileB);
        }

        yield return new WaitForSeconds(2.5f);

        StartCoroutine(Think());
    }

    IEnumerator RockShot()
    {
        // [33]. 4) 기를 모으는 중에는 고정된 한 방향을 바라보며 기를 모은다.
        isLook = false;
        anim.SetTrigger("doBigShot");
        Instantiate(bullet, transform.position, transform.rotation);
        yield return new WaitForSeconds(3.5f);

        isLook = true;
        StartCoroutine(Think());
    }

    IEnumerator Taunt()
    {
        // [33]. 5) 점프 뛸 위치를 저장한다.
        tauntVec = target.position + lookVec;

        // [33]. 6) 점프를 뛸 때는 방향 추적을 잠시 끈다.
        isLook = false;
        // [33]. 9) 이동을 위해 네비를 작동한다.
        nav.isStopped = false;
        boxCollider.enabled = false;
        anim.SetTrigger("doTaunt");

        yield return new WaitForSeconds(1.5f);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(1f);

        isLook = true;
        nav.isStopped = true;
        boxCollider.enabled = true;
        StartCoroutine(Think());
    }
}
