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
        if(jDown && !isJump && dirVec == Vector3.zero && !isDodge)
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
        if(jDown && !isJump && dirVec != Vector3.zero && !isDodge)
        {
            // [8]. 7) 회피 방향 변수에 현재 방향 변수를 배정한다.
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
}
