using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;
    public GameObject enemyPrefab;
    public int enemyCount = 5;
    
    public float spawnCooltime = 3f;
    public float spawnRadius = 10f;

    private ARPlane spawnPlane;
    private GameObject player;
    private int curEnemyCount = 5;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        spawnPlane = ScanManager.Instance.selectedPlane;
        StartCoroutine(StartSpawning());
    }
    
    IEnumerator StartSpawning()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemyNearPlayer();
            yield return new WaitForSeconds(spawnCooltime);
        }
    }

    void SpawnEnemyNearPlayer()
    {
        Vector2 randomPoint = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = new Vector3(randomPoint.x, spawnPlane.transform.position.y, randomPoint.y) + player.transform.position;

        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

}