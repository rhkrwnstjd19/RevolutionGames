using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AdventureListPanel : MonoBehaviour
{
    public AdvButtons adventureButtonPrefab;
    public AdventureDetailPanel adventureDetailPanel;

    public GameObject contentsPanel;
    public Button closeButton;
    private List<AdvDungeon> dungeonList = new();
    private void Start()
    {
        closeButton.onClick.AddListener(ClosePanel);
    }
    /// <summary>
    /// 현재 플레이어 특정 반경 내에 있는 탐험 던전의 리스트 초기화 함수입니다. 
    /// </summary>
    public void InitDungeonList()
    {
        gameObject.SetActive(true);
        dungeonList = AdvDungeonList.GetAdvDungeons();
        if (dungeonList.Count == 0)
        {
            Debug.LogError("Uhoh, No Adventure Dungeon detected. Wait Until Search Dungeon Again");
        }
        //현재 던전에 따라 버튼을 생성 및 초기화 진행
        foreach (var dungeon in dungeonList)
        {
            AdvButtons button = Instantiate(adventureButtonPrefab);
            button.transform.parent = contentsPanel.transform;
            button.Init(dungeon,SetDetailPanel);
        }
    }
    private void SetDetailPanel(AdvDungeon currentDungeon){
        //DetailPanel을 초기화하고, 현재 패널을 비활성화
        adventureDetailPanel.Init(currentDungeon);
        gameObject.SetActive(false);
    }
    private void ClosePanel() => gameObject.SetActive(false);
}
