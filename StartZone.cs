using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartZone : MonoBehaviour
{
    // [40]. 필요 속성 : 게임 매니저
    public GameManager gameManager;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            // [40]. 1) 트리거에 플레이어가 들어오면 매니저의 스테이지 시작 함수를 호출
            gameManager.StageStart();
        }
    }
}
