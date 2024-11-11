using UnityEngine;
using TMPro;
using Cinemachine;
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
    private List<PetButton> petButtons=new();
    private int currentSelectedIndex=-1;
    private MainPlayerStatusView mainPlayerStatusView;

    private void Awake()
    {
        // 패널의 초기 위치를 저장합니다.
        originalPosition = transform.localPosition;
        closePosition = new Vector2(originalPosition.x, -Screen.height);
        transform.localPosition = closePosition;
        mainPlayerStatusView = FindObjectOfType<MainPlayerStatusView>();
    }

    public void Init(AdvDungeon dungeon)
    {
        gameObject.SetActive(true);
        //UI Setting
        dungeonName.text = dungeon.dungeonName;
        dungeonLevel.text = dungeon.dungeonLevel.ToString();
        characterName.text="**캐릭터를 선택하세요**";
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
        GetPetList();

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

    private void GetPetList()
    {
        // ***TODO***
        // return List<Pet>으로 변경
        // 받아온 Pet 데이터 기반 PetButton Init
        //return mainPlayerStatusView.currentPlayer.petList;
    }
    private void SelectPet(int buttonNumber){
        if(currentSelectedIndex==-1){
            
        }else{
            petButtons[currentSelectedIndex].Deselect();
            currentSelectedIndex=buttonNumber;
            petButtons[currentSelectedIndex].WhenSelect();
        }
    }
    public void StartDungeon()
    {
        // ***TODO***
    }
}
