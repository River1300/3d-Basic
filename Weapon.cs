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
    
    public void Use()
    {
        // [17]. 1) 근접 무기일 경우 무기를 휘두른다.
        if(type == Type.Melee)
        {
            // [17]. 3) 진행되고 있는 코루틴을 멈추고 새로 시작
            StopCoroutine("Swing");
            StartCoroutine("Swing");
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

        yield return new WaitForSeconds(0.3f);
        // [17]. 6) 콜라이더를 비활성화 시킨다.
        meleeArea.enabled = false;

        yield return new WaitForSeconds(0.3f);
        // [17]. 7) 이펙트를 비활성화 시킨다. -> Player
        trailEffect.enabled = false;
    }
}
