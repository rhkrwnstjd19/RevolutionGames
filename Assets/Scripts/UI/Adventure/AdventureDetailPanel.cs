using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class AdventureDetailPanel : MonoBehaviour
{
    [Header("UI Component")]
    public TMP_Text dungeonName;
    public TMP_Text dungeonLevel;
    public TMP_Text characterName;
    public Transform buttonPanel;
    public Button CloseButton;
    public PetButton petButton;
    public Button GoButton;

    public float animationSpeed = 1.5f;
    private Vector2 originalPosition;
    private Vector2 closePosition;
    private List<PetButton> petButtons = new();
    private int currentSelectedIndex = -1;
    private MainPlayerStatusView mainPlayerStatusView;
    private ScriptablePlayer player;
    private void Awake()
    {
        // 패널의 초기 위치를 저장합니다.
        originalPosition = transform.localPosition;
        closePosition = new Vector2(originalPosition.x, -Screen.height);
        transform.localPosition = closePosition;
        mainPlayerStatusView = FindObjectOfType<MainPlayerStatusView>();
        player = mainPlayerStatusView.currentPlayer;
    }

    public void Init(AdvDungeon dungeon)
    {
        gameObject.SetActive(true);
        //UI Setting
        dungeonName.text = dungeon.dungeonName;
        dungeonLevel.text = dungeon.dungeonLevel.ToString();
        characterName.text = "**캐릭터를 선택하세요**";
        dungeon.targetAheadCamera.Priority = 30;

        CloseButton.onClick.AddListener(() =>
        {
            dungeon.targetAheadCamera.Priority = 10;
            CloseAnimation();
        });
        GoButton.onClick.AddListener(() =>
        {
            StartDungeon();
        });
        //내부 로직
        MakePetButtons();
        OpenAnimation();
    }

    public void OpenAnimation()
    {
        // 패널 애니메이션(Linear로 올라옴)
        transform.DOLocalMoveY(originalPosition.y, animationSpeed)
        .SetEase(Ease.Linear);
    }
    public void CloseAnimation()
    {
        // 패널 애니메이션(Linear로 내려감)
        transform.DOLocalMoveY(closePosition.y, animationSpeed)
        .SetEase(Ease.Linear)
        .OnComplete(() => gameObject.SetActive(false));
    }

    private List<ScriptablePet> GetPetList() => player.petList;
    void MakePetButtons()
    {
        List<ScriptablePet> list = GetPetList();
        //1. Pet List를 불러옵니다.
        int petButtonCount = petButtons.Count;
        Debug.Log($"Pet Button Count : {petButtonCount}");
        //2.Pet List를 기반으로 버튼을 생성합니다.
        //이때, 이미 생성된 버튼에 대해서는 더이상 생성하지 않습니다.
        for (int i = petButtonCount; i < list.Count; i++)
        {
            
            PetButton pet = Instantiate(petButton, buttonPanel);
            pet.transform.SetParent(buttonPanel, false);
            petButtons.Add(pet);
        }
        //3. 모든 버튼
        for (int i = 0; i < petButtons.Count; i++)
        {
            petButtons[i].Init(list[i], i, SelectPet);
        }
    }
    private void SelectPet(PetButton button)
    {
        if(currentSelectedIndex!=-1){
            petButtons[currentSelectedIndex].Deselect();
        }
        currentSelectedIndex = button.buttonNumber;
        petButtons[currentSelectedIndex].WhenSelect();
        Debug.Log($"{petButtons[currentSelectedIndex].petData.petName} Selected");
    }
    public void StartDungeon()
    {
        // ***TODO***
        //1. petButtons[currentSelectedIndex]의 ScriptableData를 받아온다.
        //2. PetPrefab 리스트에서 ScriptableData Id를 매칭시켜서 프리팹을 주변에 소환시킨다.
        //3. 던전 확인 후, 이미 공격중인 던전이라면 다른 패널 등장
    }
}
