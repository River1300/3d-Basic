using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{   
    // [9]. 필요 속성 : 아이템 타입, 아이템 속성
    public enum Type { Ammo, Coin, Grenade, Heart, Weapon };
    public Type type;
    public int value;
    // [22]. 필요 속성 : 아이템의 리지드바디와 콜라이더
    Rigidbody rigid;
    SphereCollider sphereCollider;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    void Update()
    {
        // [9]. 1) 아이템이 프레임마다 회전한다. -> Player
        transform.Rotate(Vector3.up * 20 * Time.deltaTime);
    }

    void OnCollisionEnter(Collision other)
    {
        // [22]. 6) 아이템이 바닥에 닿게되면 고정 시킨다.
        if(other.gameObject.tag == "Floor")
        {
            rigid.isKinematic = true;
            sphereCollider.enabled = false;
        }
    }
}
