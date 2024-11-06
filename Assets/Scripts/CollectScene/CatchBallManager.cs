using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CatchBallManager : MonoBehaviour
{
    [System.Serializable]
    public class BallInfo
    {
        public string ballName;
        public GameObject ballPrefab;
        public Button selectButton;
        public int ballCount; // 소모품인 볼의 갯수 (Lv 2~5)
        public TMP_Text countText;
    }

    public BallInfo[] balls; // 볼 종류 정보 (Lv1~5)
    private int currentBallIndex = 0; // 현재 선택된 볼의 인덱스
    
    // CatchBall 패널과 이를 여는 버튼
    public GameObject catchBallPanel; // CatchBall 패널 오브젝트
    public Button openPanelButton; // 패널을 여는 버튼
    public GameObject activeBall; // 현재 사용 중인 공 오브젝트

    void Start()
    {
        // 각 볼 버튼에 클릭 이벤트 추가
        for (int i = 0; i < balls.Length; i++)
        {
            int index = i; // 버튼 클릭 시 사용하기 위한 로컬 변수
            balls[i].selectButton.onClick.AddListener(() => SelectBall(index));
            UpdateBallCountText(i);
        }

        // 기본적으로 레벨 1의 볼을 선택
        SelectBall(0);

        // 패널을 여는 버튼에 클릭 이벤트 추가
        openPanelButton.onClick.AddListener(ToggleCatchBallPanel);

        // 패널을 처음에는 비활성화 상태로 설정
        catchBallPanel.SetActive(false);

        activeBall = GameObject.FindWithTag("CatchBall");
    }

    // 볼 선택
    void SelectBall(int index)
    {
        if (index < 0 || index >= balls.Length)
        {
            Debug.LogError("Invalid ball index");
            return;
        }

        // 선택된 볼 인덱스 업데이트
        currentBallIndex = index;
        Debug.Log($"Selected Ball: {balls[currentBallIndex].ballName}");
        SpawnBall();
        // 볼 선택 후 패널 닫기
        catchBallPanel.SetActive(false);
    }

    void SpawnBall()
    {
        // 기존에 사용 중인 볼이 있으면 삭제
        if (activeBall != null)
        {
            Destroy(activeBall);
        }

        // 선택된 볼을 화면 중앙 하단부에 생성
        activeBall = Instantiate(balls[currentBallIndex].ballPrefab);
    }

    // 현재 선택된 볼을 던질 때 호출
    public GameObject GetSelectedBall()
    {
        BallInfo selectedBall = balls[currentBallIndex];

        // 레벨 1의 볼은 영구적으로 사용 가능
        if (currentBallIndex == 0)
        {
            return selectedBall.ballPrefab;
        }
        
        // 레벨 2~5의 볼은 소모품
        if (selectedBall.ballCount > 0)
        {
            selectedBall.ballCount--;
            UpdateBallCountText(currentBallIndex);
            if (selectedBall.ballCount == 0)
            {
                // 레벨 1의 볼로 자동 변경
                SelectBall(0);
            }
            return selectedBall.ballPrefab;
        }
        else
        {
            // 볼이 없으면 레벨 1의 볼로 변경
            SelectBall(0);
            return balls[0].ballPrefab;
        }
    }

    // 볼 갯수 텍스트 업데이트
    void UpdateBallCountText(int index)
    {
        if (index == 0)
        {
            balls[index].countText.text = "Unlimited"; // 레벨 1의 볼은 무제한 사용 가능
        }
        else
        {
            balls[index].countText.text = balls[index].ballCount.ToString();
        }
    }

    // CatchBall 패널 토글
    void ToggleCatchBallPanel()
    {
        catchBallPanel.SetActive(!catchBallPanel.activeSelf);
    }

    // CatchBall 패널 열기
    void OpenCatchBallPanel()
    {
        catchBallPanel.SetActive(true);
    }

    // CatchBall 패널 닫기
    void CloseCatchBallPanel()
    {
        catchBallPanel.SetActive(false);
    }

    void Update()
    {
        
    }
}
