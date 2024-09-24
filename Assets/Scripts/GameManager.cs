using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : Singleton<GameManager>
{
    [HideInInspector]
    public bool isCleared = false;

    public int currentEnemyCount;
    private Player player;
    public DungeonInfo dungeonInfo;
    public bool isPlayerDead=false;

    public static GameManager Instance;
    public List<Enemy> capturedMonsters = new List<Enemy>();

    private void Start()
    {
        if (GameManager.Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        currentEnemyCount = 0;
        player =GameObject.FindWithTag("Player").GetComponent<Player>();
    }
    // Update is called once per frame
    void Update()
    {
        if (isCleared || isPlayerDead)
        {
            InteractionController.EnableMode("End");
        }
    }
    public void EnemyDefeated()
    {
        currentEnemyCount++;
        if (currentEnemyCount >=dungeonInfo.monsterCount)
        {
            isCleared = true;
        }
    }
    public void SceneChange(string nam)
    {
        SceneManager.LoadScene(nam);
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void CaptureMonster(GameObject monster)
    {
        Enemy enemyData = monster.GetComponent<Enemy>();
        if (!capturedMonsters.Contains(enemyData))
        {
            capturedMonsters.Add(enemyData);
            Debug.Log(enemyData.enemyName + " has been added to your collection.");
            // 도감 UI 업데이트 - 도감 스크립트에서 처리
        }
    }
}
