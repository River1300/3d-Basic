using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
// 속성 : 무기 타입(근접, 원거리) | 공격력 | 연사력 | 탄창 | 충돌 판정 범위 | 이펙트 | 발사 지점
// 행동 : 무기 사용 | 근접 공격(Co) | 원거리 공격(Co)
    public Transform bulletPos;
    public GameObject bullet;
    public Transform bulletCasePos;
    public GameObject bulletCase;

    public enum Type { Melee, Range };
    public Type type;

    public int damage;
    public int maxAmmo;
    public int curAmmo;
    public float rate;

    public BoxCollider meleeArea;
    public TrailRenderer trailEffect;

    public void Use()
    {
        // [17]. 1) 무기의 타입을 확인하고 해당 타입의 코루틴 함수를 호출한다.
        if(type == Type.Melee)
        {
            // [17]. 2) 연속되는 공격 속에서 진행되고 있는 스윙 코루틴을 멈추고 새로 시작
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

    IEnumerator Swing()
    {
        yield return new WaitForSeconds(0.1f);
        // [17]. 3) 만들어둔 망치의 콜라이더와 이펙트를 활성화 시킨다.
        meleeArea.enabled = true;
        trailEffect.enabled = true;

        yield return new WaitForSeconds(0.8f);
        // [17]. 4) 일정 시간이 지나고 나서 충돌 콜라이더를 비활성화 시킨다.
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f);
        // [17]. 5) 일정 시간이 지나고 나서 이펙트를 비활성화 시킨다.
        trailEffect.enabled = false;
    }

    IEnumerator Shot()
    {
        // [19]. 1) 총알을 인스턴스화 한 뒤에 물리적 힘을 가하기 위해 리지드 바디를 받는다.
        GameObject instantBullet = Instantiate(bullet, bulletPos.position, bulletPos.rotation);
        Rigidbody bulletRigid = instantBullet.GetComponent<Rigidbody>();
        // [19]. 2) 전방을 향해 발사(만들어 두었던 bulletPos의 위치상 전방, z는 forward, x는 right, y는 up)
        bulletRigid.velocity = bulletPos.forward * 50;

        yield return null;

        // [19]. 3) 탄피 배출을 위해 인스턴스화
        GameObject instantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRigid = instantCase.GetComponent<Rigidbody>();
        // [19]. 4) 탄피가 튕겨져 나가듯이 배출되도록 방향을 구한다.
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2) + Vector3.up * Random.Range(2, 3);
        caseRigid.AddForce(caseVec, ForceMode.Impulse);
        caseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);
    }
}
