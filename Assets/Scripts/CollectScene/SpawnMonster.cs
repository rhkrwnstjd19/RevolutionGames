using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class SpawnMonster : MonoBehaviour
{
    public GameObject Cube; // 큐브 오브젝트
    private ARRaycastManager raycastManager; // 레이캐스트 매니저

    private List<ARRaycastHit> hit = new List<ARRaycastHit>(); // 레이캐스트 히트 결과

    private bool objectSpawn = false; // 오브젝트가 생성되었는지 확인

    public Text messagePanel;
    public GameObject GotoMainButton;

    private void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();

        ShowMessage("시작 중...");
    }

    private void Update()
    {
        Spawn();
    }

    void Spawn()
    {
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
                    Instantiate(Cube, hitPose.position, hitPose.rotation);
                    objectSpawn = true;

                    ShowMessage("큐브 생성 완료!");

                    Invoke("EndStage", 2f);
                }
                else
                {
                    // 레이캐스트 실패 시
                    ShowMessage("지면 인식 실패.");
                }
            }
        }
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
}