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

    void Awake()
    {   
        // [2]. 1) 애니매이터를 Player 자식이 가지고 있으므로 자식으로 부터 컴포넌트를 받아와 초기화
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        // [1]. 1) 먼저 입력받은 방향 값을 저장한다.
        hAxisRaw = Input.GetAxisRaw("Horizontal");
        vAxisRaw = Input.GetAxisRaw("Vertical");

        // [2]. 2) 현재 걷기 입력키가 눌려지고 있는지를 플래그로 받는다.
        wDown = Input.GetButton("Walk");

        // [1]. 2) 방향을 일반화 하여 저장한다.
        dirVec = new Vector3(hAxisRaw, 0, vAxisRaw).normalized;

        // [1]. 3) 현재 위치 + 방향 * 속도 * 시간은 미래 위치
        // [2]. 4) 걷기 입력이 눌리고 있다면 속도를 낮춘다.
        transform.position += dirVec * speed * (wDown ? 0.3f : 1f) * Time.deltaTime;

        // [2]. 3) 애니매이터로 파라미터를 전달한다.
        anim.SetBool("isRun", dirVec != Vector3.zero);
        anim.SetBool("isWalk", wDown);

        // [3]. 1) 플레이어의 방향에 따라 오브젝트를 회전시킨다. -> Follow
        transform.LookAt(transform.position + dirVec);
    }
}
