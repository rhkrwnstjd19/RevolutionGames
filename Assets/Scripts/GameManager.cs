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
            // 게임 클리어 처리
        }
    }
    public void SceneChange(string nam)
    {
        SceneManager.LoadScene(nam);
    }
}
