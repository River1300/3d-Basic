using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
// 속성 : 데미지 | (근접 ? O : X) | (보스 주먹 ? O : X)
// 행동 : 탄피 제거 | 총탄 제거
// 버그 : 총알만 제거
    public int damage;
    // [29]. 필요 속성 : 근접 공격인지
    public bool isMelee;
    // [31]. 필요 속성 : 보스 주먹 플래그
    public bool isRock;

    void OnCollisionEnter(Collision other)
    {
        if(!isRock && other.gameObject.tag == "Floor")
        {
            // [18]. 1) 총탄이 바닥에 떨어지면 3초 뒤에 사라지도록 한다.
            Destroy(gameObject, 3);
        }
    }

    // [23]. 3) 발사되는 총알은 트리거로 제거한다.
    void OnTriggerEnter(Collider other)
    {
        // [29]. 5) 근접 공격이 아닐 경우에만 벽에 충돌했을 때 제거한다.
        if(!isMelee && other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
