using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    // [34]. 필요 속성 : UI, 애니매이터, 플레이어
    public RectTransform uiGroup;
    public Animator anim;
    Player enterPlayer;
    // [35]. 필요 속성 : 아이템 공장 배열, 가격 배열, 아이템 스폰 위치 배열, 상인 대사
    public GameObject[] itemObj;
    public int[] itemPrice;
    public Transform[] itemPos;
    public Text talkText;
    public string[] talkData;

    public void Enter(Player player)
    {
        // [34]. 1) 플레이어가 상점에 진입할 경우 상점은 플레이어 정보를 받는다.
        enterPlayer = player;
        uiGroup.anchoredPosition = Vector3.zero;
    }

    public void Exit()
    {
        // [34]. 2) 플레이어가 떠날 경우 상인의 애니매이션을 출력한다.
        anim.SetTrigger("doHello");
        uiGroup.anchoredPosition = Vector3.down * 1000;
    }

    public void Buy(int index)
    {
        // [35]. 1) 매개변수로 인덱스를 받아서 가격을 변수에 저장한다.
        int price = itemPrice[index];
        if(price > enterPlayer.coin)
        {   
            StopCoroutine(Talk());
            // [35]. 2) 돈이 부족할 경우 반환한다.
            StartCoroutine(Talk());
            return;
        }

        // [35]. 4) 돈이 충분할 경우 플레이어의 돈을 차감한다.
        enterPlayer.coin -= price;
        // [35]. 5) 아이템 스폰 위치에 약간의 변화를 주기 위해 랜덤 위치를 Vector3로 받는다.
        Vector3 ranVec = Vector3.right * Random.Range(-3, 3) + Vector3.forward * Random.Range(-3, 3);
    
        // [35]. 6) 인스턴스화를 진행한다.
        Instantiate(itemObj[index], itemPos[index].position + ranVec, itemPos[index].rotation);
    }

    IEnumerator Talk()
    {
        // [35]. 3) 돈이 부족하다는 것을 NPC대사로 출력한다.
        talkText.text = talkData[1];

        yield return new WaitForSeconds(2f);

        talkText.text = talkData[0];
    }
}
