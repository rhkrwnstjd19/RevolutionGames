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
    public GameObject PetFrame;

    public float animationSpeed = 1.5f;
    private Vector2 originalPosition;
    private Vector2 closePosition;
    private List<PetButton> petButtons=new();
    private int currentSelectedIndex=-1;
    private MainPlayerStatusView mainPlayerStatusView;
    private ScriptablePlayer player;
    private bool[] InstantiateDonePet = new bool[100];

    public AudioSource audioSource;     // 오디오 소스
    public AudioClip openSound;
    public AudioClip closeSound;
    private void Awake()
    {
        // 패널의 초기 위치를 저장합니다.
        originalPosition = transform.localPosition;
        closePosition = new Vector2(originalPosition.x, -Screen.height);
        transform.localPosition = closePosition;
        mainPlayerStatusView = FindObjectOfType<MainPlayerStatusView>();
        player = mainPlayerStatusView.currentPlayer;

        audioSource = GetComponent<AudioSource>();
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
        UpdateCapturedPet();
        OpenAnimation();
    }

    public void OpenAnimation()
    {
        audioSource.PlayOneShot(openSound);
        // 패널 애니메이션(Linear로 올라옴)
        transform.DOLocalMoveY(originalPosition.y, animationSpeed)
        .SetEase(Ease.Linear);
    }
    public void CloseAnimation()
    {
        audioSource.PlayOneShot(closeSound);
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

     void UpdateCapturedPet(){
        for(int i = 0; i < player.petList.Count; i++){
            if(InstantiateDonePet[i]) continue;
            GameObject pet = Instantiate(PetFrame, buttonPanel);
            pet.transform.SetParent(buttonPanel, false);
            // 자식 오브젝트의 이름이 "Button"이라고 가정합니다.
            Transform child = pet.transform.Find("Button");
            if (child != null){
                Image childImage = child.GetComponent<Image>();
                if (childImage != null){
                    childImage.sprite = player.petList[i].petSprite;
                }
            }
            InstantiateDonePet[i] = true;
        }
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
