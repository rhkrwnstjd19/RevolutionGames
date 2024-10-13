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
    public GameObject GotoMainButton;

    private void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();

        ShowMessage("시작 중...");
    }

    private void Update()
    {
        if (selectedMonsterIndex > -1)
        {
            Spawn();
        }
    }

    void Spawn()
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

                //ShowMessage("몬스터 생성 완료!");

                Invoke("EndStage", 2f);
            }
            else
            {
                ShowMessage("지면 인식 실패.");
            }
        }



        /*
        // 터치 입력 확인
        if (Input.touchCount > 0 && !objectSpawn)
        {
            Touch touch = Input.GetTouch(0);

            // 터치가 시작될 때 레이캐스트 실행
            if (touch.phase == TouchPhase.Began)
            {
                // 터치한 위치를 기준으로 레이캐스트
                if (raycastManager.Raycast(touch.position, hit, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hit[0].pose;


                }
                else
                {
                    // 레이캐스트 실패 시
                    ShowMessage("지면 인식 실패.");
                }
            }
        }
        */
    }
    // 메시지 패널에 텍스트 표시
    private void ShowMessage(string message)
    {
        if (messagePanel != null)
        {
            messagePanel.text = message;
        }
    }

    private void EndStage()
    {
        GotoMainButton.SetActive(true);
    }

    public void SetSelectedMonster(int index)
    {
        selectedMonsterIndex = index;    
    }
}