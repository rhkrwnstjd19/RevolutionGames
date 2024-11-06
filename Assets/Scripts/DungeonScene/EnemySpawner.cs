using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class EnemySpawner : MonoBehaviour
{
    [Header("적 프리팹")]
    public GameObject enemyPrefab; // 생성할 적의 프리팹

    [Header("스폰 설정")]
    public float spawnCool = 5f; // 스폰 간격 (초)
    public int maxEnemies = 10; // 동시에 존재할 수 있는 최대 적 수

    [Header("AR 관련 설정")]
    public ARPlaneManager planeManager; // ARPlaneManager 참조

    private Camera mainCamera;

    void Start()
    {
        if (planeManager == null)
        {
            planeManager = FindObjectOfType<ARPlaneManager>();
            if (planeManager == null)
            {
                Debug.LogError("ARPlaneManager가 씬에 존재하지 않습니다!");
                return;
            }
        }

        mainCamera = Camera.main;
        StartCoroutine(SpawnEnemiesOnPlanes());
    }

    IEnumerator SpawnEnemiesOnPlanes()
    {
        while (true)
        {
            // 현재 존재하는 적의 수를 확인
            int currentEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            if (currentEnemyCount < maxEnemies)
            {
                SpawnEnemyOnRandomPlane();
            }

            yield return new WaitForSeconds(spawnCool);
        }
    }

    void SpawnEnemyOnRandomPlane()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("Enemy Prefab이 설정되지 않았습니다!");
            return;
        }

        // 활성화된 평면들 중 하나를 무작위로 선택
        List<ARPlane> activePlanes = new List<ARPlane>();
        foreach (var plane in planeManager.trackables)
        {
            if (plane.alignment == PlaneAlignment.HorizontalUp || plane.alignment == PlaneAlignment.HorizontalDown)
            {
                activePlanes.Add(plane);
            }
        }

        if (activePlanes.Count == 0)
        {
            Debug.LogWarning("활성화된 수평 평면이 없습니다!");
            return;
        }

        ARPlane selectedPlane = activePlanes[Random.Range(0, activePlanes.Count)];

        // 평면의 경계를 사용하여 랜덤한 위치 선택
        Vector2 randomPoint2D = new Vector2(Random.Range(-selectedPlane.size.x / 2, selectedPlane.size.x / 2), Random.Range(-selectedPlane.size.y / 2, selectedPlane.size.y / 2));
        Vector3 randomPosition = selectedPlane.transform.TransformPoint(new Vector3(randomPoint2D.x+2, 0, randomPoint2D.y+2));

        // 소환 위치가 기존 적과 너무 가까운지 확인
        float minDistance = 1f; // 최소 거리 설정 (필요에 따라 조정)
        bool isTooClose = false;
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            if (Vector3.Distance(randomPosition, enemy.transform.position) < minDistance)
            {
                isTooClose = true;
                break;
            }
        }
        if (isTooClose)
        {
            // 소환 위치가 너무 가까우면 소환을 건너뜁니다.
            return;
        }

        // 평면의 회전과 일치하도록 설정 (옵션)
        Quaternion rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

        var tmp = enemyPrefab.GetComponent<DungeonEnemy>().enemyType;
        if(tmp == DungeonEnemy.EnemyType.Flying){
            randomPosition.y += 0.5f;
        }
        // 적 생성
        Instantiate(enemyPrefab, randomPosition, rotation);
    }
}
