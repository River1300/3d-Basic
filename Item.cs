using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{   
    // [9]. 필요 속성 : 아이템 타입, 아이템 속성
    public enum Type { Ammo, Coin, Grenade, Heart, Weapon };
    public Type type;
    public int value;

    void Update()
    {
        // [9]. 1) 아이템이 프레임마다 회전한다. -> Player
        transform.Rotate(Vector3.up * 20 * Time.deltaTime);
    }
}
