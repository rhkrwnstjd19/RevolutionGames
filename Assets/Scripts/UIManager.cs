using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    float longitude = 0;
    float latitude = 0;

    public GameObject dungeon_menu;
    public GameObject exit_menu;
    public GameObject obj;
    public SpawnMode spawnMode;

    public TMP_Text result1;
    public TMP_Text result;

        bool inGame = false;
    [Header("EnemyState")]
    public TMP_Text totalEnemyCount;
    public TMP_Text currentKilledEnemyCount;

    [Header("HPSet")]
    public Slider hpBar;
    public TMP_Text hpText;

    [Header("PlayerInfo")]
    public Slider expBar;
    public TMP_Text playerLevel;
    public TMP_Text expPercentage;
    public TMP_Text playerMoney;

    DungeonInfo dungeonInfo;
    Player player;
        
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (obj != null)
        {
            obj.SetActive(false);
        }

        inGame = SceneManager.GetActiveScene().name.Equals("InDungeon");
        Debug.Log(inGame);
        // Example: Access dungeon information for index 0

    }

    void Update()
    {
        SetPlayerInfo();
        if (inGame)
        {
            SetEnemyCount();
            SetHp();
        }
            
        try
        {
            longitude = GPS_Manager.Instance.longitude;
            latitude = GPS_Manager.Instance.latitude;
        }
        catch (Exception ex)
        {
            latitude = 37.791231f;
            longitude = 127.123242f;
            return;
            // Optionally, you can log additional information from the exception, such as ex.StackTrace
        }
            

    }
        

    public void Menu_button()
    {
        dungeon_menu.SetActive(!dungeon_menu.activeSelf);
    }
    private void SetPlayerInfo()
    {
        if (DataManager.Instance != null) {
            playerLevel.text = "LV: " + DataManager.Instance.PlayerLevel.ToString();
            expBar.minValue = 0;
            expBar.maxValue = DataManager.Instance.GetExpMax();
            expBar.value = DataManager.Instance.PlayerExp;
            expPercentage.text = Math.Floor((float)expBar.value / expBar.maxValue * 100).ToString() + " %";
            playerMoney.text = DataManager.Instance.Money.ToString();
        }
    else
    {
        Debug.Log("Datamanager�� �ȹ޾�����");
    }
            
    }
    private void SetEnemyCount()
    {
        totalEnemyCount.text = spawnMode.enemyTotalCount.ToString();
        currentKilledEnemyCount.text = GameManager.Instance.currentEnemyCount.ToString();
        
    }
    private void SetHp()
    {
        hpBar.minValue = 0;
        hpBar.maxValue = DataManager.Instance.PlayerMaxHp;
        hpBar.value = player.curHp;
        hpText.text=player.curHp.ToString()+" / "+DataManager.Instance.PlayerMaxHp;
    }

    public void Entrance_button(int dungeonIndex)
    {
        //Dummy Manager에서 던전에 대한 정보를 받아온다.
        dungeonInfo = DummyManager.Instance.dungeon[dungeonIndex];
        
        if (dungeonInfo.isEnableEntrance)
        {
            result1.text = "Accepted";
            result.text = "Dungeon Entranced!!";
            obj.SetActive(true);
            dungeon_menu.SetActive(false);
            GameManager.Instance.dungeonInfo= dungeonInfo;
            SceneManager.LoadScene("InDungeon");
        }
        else
        {
            result1.text = "Declined";
            result.text = "Dungeon Disable!!";
            obj.SetActive(true);
            //SceneManager.LoadScene("dungeon_scene");
        }
    }
       

    public void Exit_button()
    {
        exit_menu.SetActive(!exit_menu.activeSelf);
    }

    public void Main_Scene()
    {
        SceneManager.LoadScene("gps_scene");
    }
}

