using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    // [15]. 필요 속성 : 타겟 위치, 공전 속도, 오프셋
    public Transform target;
    public float orbitSpeed;
    Vector3 offset;

    void Start()
    {
        // [15]. 2) 폭탄과 플레이어간의 거리를 저장한다.
        offset = transform.position - target.position;
    }

    void Update()
    {
        // [15]. 3) 폭타의 위치를 매 프레임 마다 플레이어의 위치에 맞추어 저장해 준다.
        transform.position = target.position + offset;
        // [15]. 1) target을 중심으로 회전한다.
        transform.RotateAround(target.position, Vector3.up, orbitSpeed * Time.deltaTime);
        // [15]. 4) 폭탄이 회전하면 플레이어와의 거리를 재계산 한다. -> Player
        offset = transform.position - target.position;
    }
}
