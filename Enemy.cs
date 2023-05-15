using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // [23]. 필요 속성 : 피격 대상의 최대 체력과 현재 체력, 리지드바디, 콜라이더, 마테리얼
    public int maxHealth;
    public int curHealth;
    Rigidbody rigid;
    BoxCollider boxCollider;
    Material mat;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        // [23]. 4) 마테리얼은 메쉬랜더러가 가지고 있다.
        mat = GetComponent<MeshRenderer>().material;
    }

    void OnTriggerEnter(Collider other)
    {
        // [23]. 1) 근접 공격 대미지와 원거리 공격 대미지를 나누어 받는다.
        if(other.tag == "Melee")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            // [23]. 7) 넉백 위치는 자신의 위치에서 피격 위치를 뺀 값이다.
            Vector3 reactVec = transform.position - other.transform.position;
            // [23]. 3) 피격 코루틴 함수 호출
            StartCoroutine(OnDamage(reactVec, false));
        }
        else if(other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;

            Vector3 reactVec = transform.position - other.transform.position;
            // [23]. 9) 피격 이후 총알은 제거된다.
            Destroy(other.gameObject);

            StartCoroutine(OnDamage(reactVec, false));
        }
    }

    // [24]. 12) 폭탄에 맞아 체력에 달고 넉백된다.
    public void HitByGrenade(Vector3 explosionPos)
    {
        curHealth -= 100;
        Vector3 reactVec = transform.position - explosionPos;
        StartCoroutine(OnDamage(reactVec, true));
    }

    // [24]. 13) 폭탄에 맞아 사망한 적은 좀더 격정적이게 죽도록 플래그를 만든다.
    IEnumerator OnDamage(Vector3 reactVec, bool isGrenade)
    {
        // [23]. 5) 피격될 때 색을 바꾸고 일정 시간 뒤에 색을 되돌린다.
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        if(curHealth > 0){
            mat.color = Color.white;
        }
        else{
            // [23]. 6) 체력이 0이라면 레이어를 바꾸고 피격되지 않도록 한다.
            mat.color = Color.gray;
            gameObject.layer = 12;

            if(isGrenade)
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up * 3;

                // [24). 14) 더 격정적인 죽음을 위해 회전을 주며 튕겨저 나가게 한다.
                rigid.freezeRotation = false;
                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
                rigid.AddTorque(reactVec * 15, ForceMode.Impulse);
            }
            else
            {
                // [23]. 8) 방향 값을 일반화 시키고 넉백 값에 약간의 높이를 추가한다.
                reactVec = reactVec.normalized;
                reactVec += Vector3.up;

                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            }
            Destroy(gameObject, 4);
        }
    }
}