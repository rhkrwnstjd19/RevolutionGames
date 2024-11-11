using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CaptureBallManager : MonoBehaviour
{



    public GameObject ballPrefab;
    public Button selectButton;
    public int ballCount; 
    public TMP_Text countText;

    private int currentBallIndex = 0; // 현재 선택된 볼의 인덱스
    
    // CatchBall 패널과 이를 여는 버튼
    public GameObject catchBallPanel; // CatchBall 패널 오브젝트
    public Button openPanelButton; // 패널을 여는 버튼
    public GameObject activeBall; // 현재 사용 중인 공 오브젝트


}
