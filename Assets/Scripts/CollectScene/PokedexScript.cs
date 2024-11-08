using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PokedexScript : MonoBehaviour
{
    public GameObject monsterBookPanel;  // 도감 패널
    public Button openButton;            // 버튼
    public TMP_Text[] monsterStatusTexts; // 포획 여부를 표시할 텍스트 배열
    public bool[] monsterCaught = new bool[3]; // 몬스터 포획 여부 상태

    void Start()
    {
        // 버튼에 클릭 이벤트 추가
        openButton.onClick.AddListener(ToggleBook);
        // 패널을 숨김
        // monsterBookPanel.SetActive(false);
    }

    public void ToggleBook()
    {
        // 도감 패널을 켜고 끄는 기능
        monsterBookPanel.SetActive(!monsterBookPanel.activeSelf);
    }
    void UpdateMonsterStatus()
    {
        // 포획 여부에 따라 텍스트 업데이트
        for (int i = 0; i < monsterStatusTexts.Length; i++)
        {
            if (monsterCaught[i])
                monsterStatusTexts[i].text = "포획함";
            else
                monsterStatusTexts[i].text = "미포획";
        }
    }

    public void CatchMonster(int index)
    {
        if (index >= 0 && index < monsterCaught.Length)
        {
            monsterCaught[index] = true;  // 해당 몬스터 포획 표시
            UpdateMonsterStatus();  // UI 갱신
        }
    }
}
