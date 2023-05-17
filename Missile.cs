using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    void Update()
    {
        // [29]. 1) 매 프래임 마다 x축 기준으로 회전한다.
        transform.Rotate(Vector3.right * 30 * Time.deltaTime);
    }
}
