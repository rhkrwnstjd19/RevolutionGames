using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class SpawnMonster : MonoBehaviour
{
    public GameObject[] monsterPrefab; // 몬스터 배열
    private ARRaycastManager raycastManager; // 레이캐스트 매니저

    private List<ARRaycastHit> hit = new List<ARRaycastHit>(); // 레이캐스트 히트 결과

    private bool objectSpawn = false; // 오브젝트가 생성되었는지 확인
    public int selectedMonsterIndex = -1;   //monster select

    public Text messagePanel;
    public GameObject Swipe;
    public GameObject Result;



    private void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();

        ShowMessage("시작 중...");
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

                Swipe.SetActive(true);

                Invoke("DeactivateSwipe", 6f);

            }
            else
            {
                ShowMessage("지면 인식 실패.");
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