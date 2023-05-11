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
            StartCoroutine(OnDamage(reactVec));
        }
        else if(other.tag == "Bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curHealth -= bullet.damage;

            Vector3 reactVec = transform.position - other.transform.position;
            // [23]. 9) 피격 이후 총알은 제거된다.
            Destroy(other.gameObject);

            StartCoroutine(OnDamage(reactVec));
        }
    }

    IEnumerator OnDamage(Vector3 reactVec)
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
            // [23]. 8) 방향 값을 일반화 시키고 넉백 값에 약간의 높이를 추가한다.
            reactVec = reactVec.normalized;
            reactVec += Vector3.up;

            rigid.AddForce(reactVec * 5, ForceMode.Impulse);

            Destroy(gameObject, 4);
        }
    }
}
