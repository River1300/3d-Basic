using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRock : Bullet
{
    // [31]. 필요 속성 : 굴리기 위한 리지드바디, 주먹 회전 속도와 크기, 기모으는 줄 플래그
    float angularPower = 2;
    float scaleValue = 0.1f;
    bool isShoot;
    Rigidbody rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        StartCoroutine(GainPowerTime());
        StartCoroutine(GainPower());
    }

    IEnumerator GainPowerTime()
    {
        // [31]. 1) 2.5초 동안 기를 모은 뒤에 발사 버튼을 누른다.
        yield return new WaitForSeconds(2.5f);
        isShoot = true;
    }

    IEnumerator GainPower()
    {
        // [31]. 2) 발사 버튼이 눌리기 전까지 주먹 크기를 계속 키운다.
        while(!isShoot)
        {
            angularPower += 0.02f;
            scaleValue += 0.005f;
            transform.localScale  = Vector3.one * scaleValue;
            rigid.AddTorque(transform.right * angularPower, ForceMode.Acceleration);
            yield return null;
        }
    }
}
