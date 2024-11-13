using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;

public class AdventureDetailPanel : MonoBehaviour
{
    [Header("UI Component(When Not Combat)")]
    public TMP_Text dungeonName;
    public TMP_Text dungeonLevel;
    public TMP_Text petName;
    public Transform buttonPanel;
    public Button closeButton;
    public PetButton petButton;
    public Button goButton;
    public GameObject startEffect;
    [Header("UI Component(When Combat)")]
    public Button stopButton;
    public Image healthImage;
    public TMP_Text healthText;

    public float animationSpeed = 1.5f;
    private Vector2 originalPosition;
    private Vector2 closePosition;
    private List<PetButton> petButtons = new();
    private int currentSelectedIndex = -1;
    private MainPlayerStatusView mainPlayerStatusView;
    private ScriptablePlayer player;
    private AdvDungeon currentDungeon;
    private List<GameObject> PetObjects = new();
    private void Awake()
    {
        // 패널의 초기 위치를 저장합니다.
        originalPosition = transform.localPosition;
        closePosition = new Vector2(originalPosition.x, -Screen.height);
        transform.localPosition = closePosition;
        mainPlayerStatusView = FindObjectOfType<MainPlayerStatusView>();
        player = mainPlayerStatusView.currentPlayer;

    }
    private void ClearCurrentDungeon()
    {
        if (currentDungeon != null)
        {
            currentDungeon.ClearUICallback();  // AdvDungeon에 새로운 메서드 추가 필요
            currentDungeon.targetAheadCamera.Priority = 10;
            currentDungeon = null;
        }
    }
    public async void Init(AdvDungeon dungeon)
    {
        ClearCurrentDungeon();
        if (PetObjects.Count == 0) PetObjects = await PetObjectList.LoadPetObjects();
        Debug.Log(PetObjects.Count);
        gameObject.SetActive(true);

        //공통 UI Setting
       currentDungeon = dungeon;  // 새 던전 설정
        dungeonName.text = dungeon.dungeonName;
        dungeonLevel.text = $"LV.{dungeon.dungeonLevel}";
        dungeon.targetAheadCamera.Priority = 30;
        healthText.text = dungeon.currentHealth.ToString();
        UpdateHealthBar(dungeon.currentHealth, dungeon.maxHealth);

        // isWorking 상태에 따른 UI 전환
        if (dungeon.isWorking)
        {
            // 전투 중인 상태 UI
            buttonPanel.gameObject.SetActive(false);
            goButton.gameObject.SetActive(false);
            stopButton.gameObject.SetActive(true);

            // 현재 전투 중인 펫 찾기
            var activePets = GameObject.FindObjectsOfType<Pet>();
            var currentPet = activePets.FirstOrDefault(p => p.target == dungeon);
            if (currentPet != null)
            {
                petName.text = currentPet.petData.petName;
            }

            // 패널 위치 설정
            float halfScreenHeight = Screen.height * 0.5f;
            transform.localPosition = new Vector2(originalPosition.x, -halfScreenHeight);
        }
        else
        {
            // 선택 상태 UI
            buttonPanel.gameObject.SetActive(true);
            goButton.gameObject.SetActive(true);
            stopButton.gameObject.SetActive(false);

            petName.text = "**캐릭터를 선택하세요**";
            MakePetButtons();
            OpenAnimation();
        }

        AddListenersToButton();
    }
    private void AddListenersToButton()
    {
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(() =>
        {
            currentDungeon.targetAheadCamera.Priority = 10;
            CloseAnimation();
        });
        goButton.onClick.RemoveAllListeners();
        goButton.onClick.AddListener(() =>
        {
            StartDungeon();
        });
        stopButton.onClick.RemoveAllListeners();
        stopButton.onClick.AddListener(StopDungeon);
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
        if (currentSelectedIndex != -1)
        {
            petButtons[currentSelectedIndex].Deselect();
        }
        currentSelectedIndex = button.buttonNumber;
        petButtons[currentSelectedIndex].WhenSelect();
        petName.text = petButtons[currentSelectedIndex].petData.petName;
        Debug.Log($"{petButtons[currentSelectedIndex].petData.petName} Selected");
    }
    private void StartDungeon()
    {
        if (currentSelectedIndex == -1)
        {
            Debug.LogWarning("No pet selected!");
            return;
        }

        ScriptablePet selectedPetData = petButtons[currentSelectedIndex].petData;
        // PetObjects 리스트에서 선택된 펫의 이름과 일치하는 프리팹 찾기
        GameObject matchingPet = PetObjects.Find(petObj =>
        petObj.GetComponent<Pet>()?.petData.petName == selectedPetData.petName);


        if (matchingPet != null)
        {
            if (currentDungeon != null)
            {
                currentDungeon.StartDungeonHit(matchingPet.gameObject,(current, max)=>{
                    UpdateHealthBar(current,max);
                });
                buttonPanel.gameObject.SetActive(false);
                goButton.gameObject.SetActive(false);
                stopButton.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError("Oh Well, currentDungeon is NULL. I dunno the fucking reason!");
            }

            // Start Effect 애니메이션
            startEffect.SetActive(true);
            startEffect.transform.localScale = Vector3.zero;
            stopButton.gameObject.SetActive(true);

            // 크기가 커지는 애니메이션
            startEffect.transform.DOScale(Vector3.one, 0.5f)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    CanvasGroup canvasGroup = startEffect.GetComponent<CanvasGroup>();
                    if (canvasGroup == null)
                    {
                        canvasGroup = startEffect.AddComponent<CanvasGroup>();
                    }

                    // 페이드 아웃 애니메이션
                    canvasGroup.DOFade(0f, 1f)
                        .SetEase(Ease.InOutQuad)
                        .OnComplete(() =>
                        {
                            startEffect.SetActive(false);
                            canvasGroup.alpha = 1f; // 다음 사용을 위해 초기화
                        });
                });
            float halfScreenHeight = Screen.height * 0.5f;
            // 패널 닫기 애니메이션
            transform.DOLocalMoveY(-halfScreenHeight, animationSpeed)
                .SetEase(Ease.Linear);
        }
        else
        {
            Debug.LogError($"No matching pet prefab found for pet name: {selectedPetData.petName}");
        }
    }
    private void StopDungeon()
    {
        currentDungeon.ExitDungeonHit();
        // UI 전환
        buttonPanel.gameObject.SetActive(true);
        goButton.gameObject.SetActive(true);
        stopButton.gameObject.SetActive(false);
        
        // 패널 위치 원래대로
        transform.DOLocalMoveY(originalPosition.y, animationSpeed)
            .SetEase(Ease.Linear);
    }
    private void UpdateHealthBar(int currentHealth,int maxHealth){
        healthImage.fillAmount=(float)currentHealth/maxHealth;
        healthText.text=currentHealth.ToString();
    }
    private void OnDisable()
    {
        ClearCurrentDungeon();
    }

    private void OnDestroy()
    {
        ClearCurrentDungeon();
    }
}
