using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class SpawnMonster : MonoBehaviour
{
    public GameObject[] monsterPrefab; // 몬스터 프리팹 배열
    private ARRaycastManager raycastManager; // ARRaycastManager 컴포넌트

    private List<ARRaycastHit> hit = new List<ARRaycastHit>(); // AR 레이캐스트 히트 결과 리스트

    private bool objectSpawn = false; // 오브젝트가 스폰되었는지 여부 확인
    public int selectedMonsterIndex = -1;   // 선택된 몬스터 인덱스

    public Text messagePanel; // 메시지를 표시할 UI 텍스트 패널
    public GameObject Swipe; // 스와이프 UI 오브젝트
    public GameObject Result; // 결과 UI 오브젝트
    public SpinRoulette sr;
    public BCtest bt;
    public BCtest2 bt2;

    private void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();

        ShowMessage("초기화 중...");
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

                //bt.InitialBall();
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
