using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class SpawnMonster : MonoBehaviour
{
    public GameObject[] monsterPrefab; // 몬스터 배열
    private ARRaycastManager raycastManager; // ARRaycast 매니저

    private List<ARRaycastHit> hit = new List<ARRaycastHit>(); // Raycast 히트 결과 저장

    private bool objectSpawn = false; // 오브젝트가 생성되었는지 확인
    public int selectedMonsterIndex = -1;   // 선택된 몬스터 인덱스

    public Text messagePanel;
    public GameObject Swipe;
    public GameObject Result;

    private AudioSource audioSource;     // 오디오 소스
    public AudioClip spawnSound;        // 몬스터 스폰 효과음

    private void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        ShowMessage("스캔 중...");

        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {

    }

    public void Spawn()
    {
        if (!objectSpawn)
        {
            GameObject selectedMonster = monsterPrefab[selectedMonsterIndex];

            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            Ray ray = Camera.main.ScreenPointToRay(screenCenter);

            if (raycastManager.Raycast(ray, hit, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hit[0].pose;

                ShowMessage("Hit Position: " + hitPose.position);

                Instantiate(selectedMonster, hitPose.position, hitPose.rotation);
                objectSpawn = true;

                // 몬스터 스폰 효과음 재생
                audioSource.PlayOneShot(spawnSound);

                Swipe.SetActive(true);

                Invoke("DeactivateSwipe", 6f);

            }
            else
            {
                ShowMessage("평면 인식 실패.");
            }
        }
    }

    private void ShowMessage(string message)
    {
        if (messagePanel != null)
        {
            messagePanel.text = message;
        }
    }

    private void DeactivateSwipe()
    {
        Swipe.SetActive(false);

        Invoke("EndStage", 6f);

    }
    private void EndStage()
    {
        Result.SetActive(true);
    }

    public void SetSelectedMonster(int index)
    {
        selectedMonsterIndex = index;
    }
}
