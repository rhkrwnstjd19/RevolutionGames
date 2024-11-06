using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterManager : MonoBehaviour
{
    public List<MonsterData> monsters = new List<MonsterData>();  // 몬스터 데이터 리스트 (ScriptableObject로 관리)
    public GameObject pokedexPanel;  // 도감 UI 패널
    public Transform pokedexContent; // 도감 내용이 표시될 위치
    public GameObject pokedexEntryPrefab;  // 도감 항목 프리팹
    public GameObject[] monsterUIElements;  // 개별 몬스터 UI (간단한 도감)

    // 게임 시작 시 초기화
    void Start()
    {
        pokedexPanel.SetActive(false);  // 처음에는 도감이 보이지 않음
        //UpdatePokedex();  // 도감 UI 업데이트
        monsterUIElements = new GameObject[monsters.Count];
    }

    // 몬스터를 포획한 후 도감 갱신
    public void CaptureMonster(int monsterIndex)
    {
        PlayerPrefs.SetInt("Monster_" + monsterIndex, 1);  // 포획 여부 저장
        UpdatePokedex();  // 포획 후 도감 업데이트
    }

    // 도감 UI 업데이트 (개별 UI 요소)
    public void UpdatePokedex()
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            bool isCollected = PlayerPrefs.GetInt("Monster_" + i, 0) == 1;  // 포획 여부 확인

            if (isCollected)
            {
                monsterUIElements[i].transform.Find("CollectText").GetComponent<Text>().text = "Collected!";
                monsterUIElements[i].transform.Find("MonsterImage").GetComponent<Image>().sprite = monsters[i].monsterImage;
                monsterUIElements[i].transform.Find("MonsterImage").gameObject.SetActive(true);
            }
            else
            {
                monsterUIElements[i].transform.Find("CollectText").GetComponent<Text>().text = "Not Collected";
                monsterUIElements[i].transform.Find("MonsterImage").gameObject.SetActive(false);
            }
        }
    }

    // 도감 패널 열기
    public void OpenPokedex()
    {
        pokedexPanel.SetActive(true);
        PopulatePokedexUI();
    }

    // 도감 패널 닫기
    public void ClosePokedex()
    {
        pokedexPanel.SetActive(false);
    }

    // 도감 UI를 동적으로 생성하여 내용 표시
    public void PopulatePokedexUI()
    {
        foreach (Transform child in pokedexContent)
        {
            Destroy(child.gameObject);  // 기존 항목 제거
        }

        for (int i = 0; i < monsters.Count; i++)
        {
            bool isCollected = PlayerPrefs.GetInt("Monster_" + i, 0) == 1;
            if (isCollected)
            {
                GameObject entry = Instantiate(pokedexEntryPrefab, pokedexContent);
                entry.transform.Find("MonsterName").GetComponent<Text>().text = monsters[i].monsterName;
                entry.transform.Find("MonsterDescription").GetComponent<Text>().text = monsters[i].monsterDescription;
                entry.transform.Find("MonsterImage").GetComponent<Image>().sprite = monsters[i].monsterImage;
            }
        }
    }
}