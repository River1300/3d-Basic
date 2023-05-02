using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    // [4]. 필요 속성 : 플레이어의 위치와 카메라의 고정 위치
    public Transform target;
    public Vector3 offset;

    void Update()
    {
        // [4]. 1) 카메라는 프레임마다 플레이어를 따라서 움직인다.
        transform.position = target.position + offset;
    }
}
