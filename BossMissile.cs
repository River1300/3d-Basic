using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMissile : Bullet
{
    // [30]. 필요 속성 : 플레이어 위치, 네비
    public Transform target;
    NavMeshAgent nav;

    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // [30]. 1) 매 프레임 마다 플레이어를 추적
        nav.SetDestination(target.position);
    }
}
