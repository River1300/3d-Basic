using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // [16]. 필요 속성 : 무기의 속성
    public enum Type { Melee, Range };
    public Type type;
    public int damage;
    public float rate;
    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;
    // [19]. 총알, 탄피 배출 위치
    public Transform bulletPos;
    public GameObject bullet;
    public Transform bulletCasePos;
    public GameObject bulletCase;
    // [20]. 최대 탄창에 넣을 수 있는 총알, 현재 탄창에 있는 총알
    public int maxAmmo;
    public int curAmmo;
    
    public void Use()
    {
        // [17]. 1) 근접 무기일 경우 무기를 휘두른다.
        if(type == Type.Melee)
        {
            // [17]. 3) 진행되고 있는 코루틴을 멈추고 새로 시작
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
        else if(type == Type.Range && curAmmo > 0)
        {
            // [20]. 1) 현재 보유 총알이 없으면 총을 쏘지 못하게 맊고, 총을 쏠때마다 총알 갯수를 감소 시킨다.
            curAmmo--;
            StartCoroutine("Shot");
        }
    }

    // [17]. 2) 코루틴 함수로 무기를 휘두른다.
    IEnumerator Swing()
    {
        // [17]. 4) 잠깐의 대기를 작성한다.
        yield return new WaitForSeconds(0.1f);
        // [17]. 5) 콜라이더와 이펙트를 활성화 시킨다.
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.8f);
        // [17]. 6) 콜라이더를 비활성화 시킨다.
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f);
        // [17]. 7) 이펙트를 비활성화 시킨다. -> Player
        trailEffect.enabled = false;
    }

    IEnumerator Shot()
    {
        // [19]. 1) 총알 발사위해 인스턴스화
        GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        // [19]. 2) 총알에 물리적 힘을 가하기 위해 리지드바디 받아오기
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
        // [19]. 3) 전방을 향해 발사(만들어 두었던 bulletPos의 위치상 전방, z는 forward, x는 right, y는 up)
        bulletRigid.velocity = bulletPos.forward * 50;
        yield return null;
        // [19]. 4) 탄피 배출을 위해 인스턴스화
        GameObject instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        // [19]. 5) 탄피에 물리적 힘을 가하기 위해 리지드바디 받아오기
        Rigidbody caseRigid = instantCase.GetComponent<Rigidbody>();
        // [19]. 6) 탄피가 배출되는 방향 구하기
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        caseRigid.AddForce(caseVec, ForceMode.Impulse);
        caseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);
    }
}
