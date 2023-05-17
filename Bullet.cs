using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // [18]. 필요 속성 : 총알의 데미지
    public int damage;
    // [29]. 필요 속성 : 근접 공격인지
    public bool isMelee;

    // [18]. 1) 충돌하면 총알은 제거된다.
    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Floor")
        {
            Destroy(gameObject, 3);
        }
    }

    // [23]. 2) 발사되는 총알은 트리거로 제거한다.
    void OnTriggerEnter(Collider other)
    {
        // [29]. 5) 근접 공격이 아닐 경우에만 벽에 충돌했을 때 제거한다.
        if(!isMelee && other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }
}
