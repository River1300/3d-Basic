using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{   
// 속성 : 아이템 타입(총알, 수류탄, 돈, 체력, 무기) | 아이템 값 | 물리 | 충돌체
// 행동 : 회전
// 버그 : 물리적 문제 개선
    public enum Type { Ammo, Coin, Grenade, Heart, Weapon };
    public Type type;
    public int value;   // 무기는 무기 번호, 아이템은 갯수, 돈은 량

    Rigidbody rigid;
    SphereCollider sphereCollider;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    void Update()
    {
        // [9]. 1) 아이템이 매 프레임마다 y축으로 회전한다.
        transform.Rotate(Vector3.up * 20 * Time.deltaTime);
    }

    void OnCollisionEnter(Collision other)
    {
        // [22]. 6) 아이템이 바닥에 닿게되면 물리적인 계산을 고정 시키고, 바닥 콜라이더를 비활성화 시킨다.
        if(other.gameObject.tag == "Floor")
        {
            rigid.isKinematic = true;
            sphereCollider.enabled = false;
        }
    }
}
