using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // [37]. 게임을 컨트롤하는데 필요한 속성들
    public GameObject menuCam;
    public GameObject gameCam;
    public Player player;
    public Boss boss;
    public int stage;
    public float playTime;
    public bool isBattle;
    public int enemyCntA;
    public int enemyCntB;
    public int enemyCntC;
    public int enemyCntD;

    public GameObject menuPanel;
    public GameObject gamePanel;
    public Text maxScoreTxt;
    public Text scoreTxt;
    public Text stageTxt;
    public Text playTimeTxt;
    public Text playerHealthTxt;
    public Text playerAmmoTxt;
    public Text playerCoinTxt;
    public Image weapon1Img;
    public Image weapon2Img;
    public Image weapon3Img;
    public Image weaponRImg;
    public Text enemyATxt;
    public Text enemyBTxt;
    public Text enemyCTxt;

    public RectTransform bossHealthGroup;
    public RectTransform bossHealthBar;

    // [40]. 필요 속성 : 게임 준비 화면의 오브젝트를 담을 변수
    public GameObject itemShop;
    public GameObject weaponShop;
    public GameObject stageZone;

    // [41]. 필요 속성 : 몬스터 소환에 필요한 변수
    public Transform[] enemyZones;
    public GameObject[] enemies;
    public List<int> enemyList;
    
    // [42]. 필요 속성 : 게임 오버 오브젝트, 현재 점수, 최고 점수 텍스트
    public GameObject gameOverPanel;
    public Text curScoreText;
    public Text bestText;

    void Awake()
    {
        enemyList = new List<int>();
        // [38]. 1) 저장된 최고 점수를 출력한다.
        maxScoreTxt.text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore"));

        if(PlayerPrefs.HasKey("MaxScore"))
        {
            PlayerPrefs.SetInt("MaxScore", 0);
        }
    }

    public void GameStart()
    {
        // [38]. 2) 카메라와 UI판을 활성화, 비활성화 시킨다.
        menuCam.SetActive(false);
        gameCam.SetActive(true);

        menuPanel.SetActive(false);
        gamePanel.SetActive(true);

        player.gameObject.SetActive(true);
    }

    public void StageStart()
    {
        // [40]. 3) 스테이지 시작과 함께 상점, 스테이지 존은 비활성화
        itemShop.SetActive(false);
        weaponShop.SetActive(false);
        stageZone.SetActive(false);

        foreach(Transform zone in enemyZones)
            zone.gameObject.SetActive(true);

        // [40]. 2) 현재 전투 중임을 플래그에 체크한다.
        isBattle = true;
        StartCoroutine(InBattle());
    }

    public void StageEnd()
    {
        // [40]. 4) 스테이지가 끝나면 안전 지역으로 플레이어를 옮긴다.
        player.transform.position = new Vector3(-20, 1, -20);

        itemShop.SetActive(true);
        weaponShop.SetActive(true);
        stageZone.SetActive(true);

        foreach(Transform zone in enemyZones)
            zone.gameObject.SetActive(false);

        isBattle = false;
        stage++;
    }

    public void GameOver()
    {
        // [42]. 2) UI를 활성화 하고 최고 점수를 표시한다.
        gamePanel.SetActive(false);
        gameOverPanel.SetActive(true);

        curScoreText.text = scoreTxt.text;

        int maxScore = PlayerPrefs.GetInt("MaxScore");
        if(player.score > maxScore)
        {
            bestText.gameObject.SetActive(true);
            PlayerPrefs.GetInt("MaxScore", player.score);
        }
    }

    public void ReStart()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator InBattle()
    {
        if (stage % 5 == 0)
        {
            // [41]. 5) 보스 소환 스테이지와 일반 몹 스테이로 나눈다.
            enemyCntD++;
            GameObject instantEnemy = Instantiate(enemies[3], enemyZones[0].position, enemyZones[0].rotation);
            Enemy enemy = instantEnemy.GetComponent<Enemy>();
            enemy.target = player.transform;
            enemy.gameManager = this;
            boss = instantEnemy.GetComponent<Boss>();
        } 
        else
        {
            for(int index = 0; index < stage; index++)
            {
                // [41]. 1) 리스트에 적 타입 데이터를 저장 한다.
                int ran = Random.Range(0, 3);
                enemyList.Add(ran);

                // [41]. 4) 적 타입이 추가될 때마다 수를 증가 시킨다.
                switch(ran)
                {
                    case 0:
                        enemyCntA++;
                        break;
                    case 1:
                        enemyCntB++;
                        break;
                    case 2:
                        enemyCntC++;
                        break;
                }
            }

            while(enemyList.Count > 0)
            {
                // [41]. 2) 저장된 리스트의 데이터 만큼 적을 객체화 한다.
                int ran = Random.Range(0, 4);
                GameObject instantEnemy = Instantiate(enemies[enemyList[0]], enemyZones[ran].position, enemyZones[ran].rotation);
                // [41]. 3) 플레이어 위치를 넘긴다.
                Enemy enemy = instantEnemy.GetComponent<Enemy>();
                enemy.target = player.transform;
                enemy.gameManager = this;
                enemyList.RemoveAt(0);
                yield return new WaitForSeconds(4f);
            }
        }

        // [41]. 6) 적을 모두 제거할 때 까지 반복문에 갇혀 있는다.
        while(enemyCntA + enemyCntB + enemyCntC + enemyCntD > 0)
        {
            yield return null;
        }

        yield return new WaitForSeconds(4f);
        boss = null;
        StageEnd();
    }

    void Update()
    {
        if(isBattle)
            playTime += Time.deltaTime;
    }

    void LateUpdate()
    {
        // [39]. 1) 현재 플레이어의 점수
        scoreTxt.text = string.Format("{0:n0}", player.score);
        // [39]. 6) 현재 스테이지 번호
        stageTxt.text = "STAGE " + stage;
        // [39]. 7) 현재 플레이 타임
        int hour = (int)(playTime / 3600);
        int min = (int)((playTime - hour * 3600) / 60);
        int second = (int)(playTime % 60);
        playTimeTxt.text = string.Format("{0:00}", hour) + ":" + string.Format("{0:00}", min) + ":" + string.Format("{0:00}", second);
        // [39]. 2) 현재 플레이어의 체력과 돈
        playerHealthTxt.text = player.health + " / " + player.maxHealth;
        playerCoinTxt.text = string.Format("{0:n0}", player.coin);
        // [39]. 3) 들고 있는 무기가 없을 때 총알 갯수, 근접일 때, 총을 들고 있을 때 각기 다른 값을 출력한다.
        if(player.equipWeapon == null)
            playerAmmoTxt.text = " - / " + player.ammo;
        else if(player.equipWeapon.type == Weapon.Type.Melee)
            playerAmmoTxt.text = " - / " + player.ammo;
        else
            playerAmmoTxt.text = player.equipWeapon.curAmmo + " / " + player.ammo;
        // [39]. 4) 무기 보유 여부에 따라 이미지 알파값 조정
        weapon1Img.color = new Color(1, 1, 1, player.hasWeapons[0] ? 1 : 0);
        weapon2Img.color = new Color(1, 1, 1, player.hasWeapons[1] ? 1 : 0);
        weapon3Img.color = new Color(1, 1, 1, player.hasWeapons[2] ? 1 : 0);
        weaponRImg.color = new Color(1, 1, 1, player.hasGrenade > 0 ? 1 : 0);
        // [39]. 5) 현재 남은 적의 숫자
        enemyATxt.text = enemyCntA.ToString();
        enemyBTxt.text = enemyCntB.ToString();
        enemyCTxt.text = enemyCntC.ToString();
        // [39]. 8) 보스의 체력 바
        if(boss != null)
        {
            // [41]. 8) 보스가 등장할 때만 보스 체력 바를 표시하도록 한다.
            bossHealthGroup.anchoredPosition = Vector3.down * 30;
            bossHealthBar.localScale = new Vector3((float)boss.curHealth / boss.maxHealth, 1, 1);
        }
        else
        {
            bossHealthGroup.anchoredPosition = Vector3.up * 300;
        }
    }

}
