using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PokedexManager : MonoBehaviour
{
    [System.Serializable]
    public class MonsterSlot
    {
        public Image monsterImage;
        public TMP_Text monsterName;
        public TMP_Text monsterDescription;
    }

    public MonsterSlot[] monsterSlots; // 5개의 몬스터 슬롯 (추후 확장 가능)
    private bool[] capturedMonsters; // 몬스터 포획 상태를 추적
    public Button nextPageButton;
    public Button previousPageButton;
    private int currentPage = 0;
    private int slotsPerPage = 5;
    public Slider progressBar; // 도감 진행 상황을 표시할 슬라이더
    public TMP_Text progressText; // 진행 상황을 텍스트로 표시

    void Start()
    {
        // 몬스터 포획 상태 배열 초기화 (초기는 미포획 상태로 시작)
        capturedMonsters = new bool[monsterSlots.Length];

        // 모든 몬스터 슬롯을 초기 상태로 설정 (실루엣 및 비활성화)
        for (int i = 0; i < monsterSlots.Length; i++)
        {
            SetMonsterSlotInactive(i);
        }
    }

    // 몬스터를 포획했을 때 호출
    public void CaptureMonster(int monsterIndex)
    {
        if (monsterIndex < 0 || monsterIndex >= capturedMonsters.Length)
        {
            Debug.LogError("유효하지 않은 몬스터. 인덱스 수정 필요");
            return;
        }

        // 포획된 상태로 설정
        capturedMonsters[monsterIndex] = true;

        // 해당 몬스터 슬롯 활성화
        SetMonsterSlotActive(monsterIndex);
        UpdateProgressBar();
    }

    // 몬스터 슬롯을 활성화 (포획된 몬스터 표시)
    void SetMonsterSlotActive(int index)
    {
        MonsterSlot slot = monsterSlots[index];
        Color brightColor = slot.monsterImage.color;
        brightColor.a = 1.0f; // 이미지 밝게 설정
        slot.monsterImage.color = brightColor;

        slot.monsterName.gameObject.SetActive(true); // 이름 표시
        slot.monsterDescription.gameObject.SetActive(true); // 설명 표시
    }

    // 몬스터 슬롯을 비활성화 (포획 전 상태로 설정)
    void SetMonsterSlotInactive(int index)
    {
        MonsterSlot slot = monsterSlots[index];
        Color darkColor = slot.monsterImage.color;
        darkColor.a = 0.3f; // 이미지 어둡게 설정 (실루엣 효과)
        slot.monsterImage.color = darkColor;

        slot.monsterName.gameObject.SetActive(false); // 이름 숨기기
        slot.monsterDescription.gameObject.SetActive(false); // 설명 숨기기
    }
    void NextPage()
    {
        if ((currentPage + 1) * slotsPerPage < monsterSlots.Length)
        {
            currentPage++;
            UpdatePage();
        }
    }

    // 이전 페이지로 이동
    void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdatePage();
        }
    }
    void UpdatePage()
    {
        for (int i = 0; i < monsterSlots.Length; i++)
        {
            if (i >= currentPage * slotsPerPage && i < (currentPage + 1) * slotsPerPage)
            {
                if (capturedMonsters[i])
                {
                    SetMonsterSlotActive(i);
                }
                else
                {
                    SetMonsterSlotInactive(i);
                }
            }
            else
            {
                monsterSlots[i].monsterImage.gameObject.SetActive(false);
                monsterSlots[i].monsterName.gameObject.SetActive(false);
                monsterSlots[i].monsterDescription.gameObject.SetActive(false);
            }
        }

        // 이전/다음 페이지 버튼 활성화 여부 설정
        previousPageButton.gameObject.SetActive(currentPage > 0);
        nextPageButton.gameObject.SetActive((currentPage + 1) * slotsPerPage < monsterSlots.Length);
    }
    void UpdateProgressBar()
    {
        int capturedCount = 0;
        foreach (bool captured in capturedMonsters)
        {
            if (captured)
            {
                capturedCount++;
            }
        }

        float progress = (float)capturedCount / capturedMonsters.Length;
        progressBar.value = progress;
        progressText.text = $"{capturedCount} / {capturedMonsters.Length}";
    }
}