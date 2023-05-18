using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    // [32]. 필요 속성 : 미사일 프리팹과 발사 위치들, 플레이어 예상 위치, 착지 위치, 플래그
    public GameObject missile;
    public Transform missilePortA;
    public Transform missilePortB;
    Vector3 lookVec;
    Vector3 tauntVec;
    bool isLook;

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
    }
}
