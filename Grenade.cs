using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    // [24]. 필요 속성 : 폭발 오브젝트를 받아올 게임 오브젝트와 리지드바디
    public GameObject meshObj;
    public GameObject EffectObj;
    public Rigidbody rigid;

    void Start()
    {
        StartCoroutine(Explosion());
    }

    IEnumerator Explosion()
    {
        // [24]. 8) 3초 뒤에 폭발한다.
        yield return new WaitForSeconds(3f);
        // [24]. 9) 이동속도와 회전속도를 0으로
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        // [24]. 10) 폭발 이펙트 활성화
        meshObj.SetActive(false);
        EffectObj.SetActive(true);
        // [24]. 11) 폭발 범위에 닿은 모든 적 오브젝트를 가져온다.
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 15, Vector3.up, 0f, LayerMask.GetMask("Enemy"));
        // [24]. 12) 배열을 순회 하며 적 오브젝트에게 폭발 피격을 준다.
        foreach(RaycastHit hitObj in rayHits)
        {
            hitObj.transform.GetComponent<Enemy>().HitByGrenade(transform.position);
        }

        Destroy(gameObject, 5);
    }
}
