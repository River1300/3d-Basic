using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
// 속성 : 플레이어 위치 | 간격
// 행동 : 플레이어 따라다니기
    public Transform target;
    public Vector3 offset;

    void Update()
    {
        // [4]. 1) 카메라는 프레임마다 플레이어를 따라서 움직인다.
        transform.position = target.position + offset;
    }
}
