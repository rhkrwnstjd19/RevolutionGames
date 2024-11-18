using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;
using AssetKits.ParticleImage;

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
    public ParticleImage moneyAnimation;
    private Vector2 originalPosition;
    private Vector2 closePosition;
    private List<PetButton> petButtons = new();
    private int currentSelectedIndex = -1;
    private MainPlayerStatusView mainPlayerStatusView;
    private ScriptablePlayer player;
    private AdvDungeon currentDungeon;
    private List<GameObject> petObjects = new();
    private Dictionary<string, GameObject> petObjectsMap = new();

    [Header("UI Sounds")]
    private AudioSource audioSource;
    public AudioClip enterSound;
    public AudioClip backSound;
    private async void Awake()
    {
        // 패널의 초기 위치를 저장합니다.
        originalPosition = transform.localPosition;
        closePosition = new Vector2(originalPosition.x, -Screen.height);
        transform.localPosition = closePosition;
        mainPlayerStatusView = FindObjectOfType<MainPlayerStatusView>();
        player = mainPlayerStatusView.currentPlayer;
        audioSource = GetComponent<AudioSource>();
        var petObjects = await PetObjectList.LoadPetObjects();
        foreach (var petObj in petObjects)
        {
            if (petObj.TryGetComponent<Pet>(out var pet))
            {
                petObjectsMap[pet.petData.petName] = petObj;
                pet.petData.isAttacking = false;//공격 중인 상태에서 종료시, ScriptableObject 저장이 되는 관계로 임시 설정
            }
        }
        InitializeButtonListeners();
    }
    private void InitializeButtonListeners()
    {
        closeButton.onClick.AddListener(() =>
        {
            if (currentDungeon != null)
            {
                currentDungeon.targetAheadCamera.Priority = 10;
            }
            CloseAnimation();
        });

        goButton.onClick.AddListener(StartDungeon);
        stopButton.onClick.AddListener(StopDungeon);
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
    public void Init(AdvDungeon dungeon)
    {
        gameObject.SetActive(true);
        ClearCurrentDungeon();


        //공통 UI Setting
        currentDungeon = dungeon;  // 새 던전 설정
        dungeonName.text = dungeon.dungeonName;
        dungeonLevel.text = $"LV.{dungeon.dungeonLevel}";
        dungeon.targetAheadCamera.Priority = 30;
        healthText.text = dungeon.currentHealth.ToString();
        UpdateHealthBar(dungeon.currentHealth, dungeon.maxHealth);
        currentDungeon.SetUICallback((current, max) =>
                {
                    UpdateHealthBar(current, max);
                });
        // isWorking 상태에 따른 UI 전환
        if (dungeon.isWorking)
        {
            // 전투 중인 상태 UI
            buttonPanel.gameObject.SetActive(false);
            goButton.gameObject.SetActive(false);
            stopButton.gameObject.SetActive(true);

            // 현재 전투 중인 펫 찾기
            var currentPet = currentDungeon.petObject.GetComponent<Pet>();
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

    }
    public void OpenAnimation()
    {
        audioSource.PlayOneShot(enterSound);
        // 패널 애니메이션(Linear로 올라옴)
        transform.DOLocalMoveY(originalPosition.y, animationSpeed)
        .SetEase(Ease.Linear);
    }
    public void CloseAnimation()
    {
        audioSource.PlayOneShot(backSound);
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
        if (currentSelectedIndex == -1 || currentDungeon == null)
        {
            Debug.LogWarning("No pet selected or dungeon is null!");
            return;
        }

        ScriptablePet selectedPetData = petButtons[currentSelectedIndex].petData;
        selectedPetData.isAttacking = true;
        if (petObjectsMap.TryGetValue(selectedPetData.petName, out GameObject matchingPet))
        {
            currentDungeon.StartDungeonHit(matchingPet.gameObject, (current, max) =>
                {
                    UpdateHealthBar(current, max);
                }, player, (moneyAmount) =>
                {
                    UpdateCoin(moneyAmount);
                });
            buttonPanel.gameObject.SetActive(false);
            goButton.gameObject.SetActive(false);
            stopButton.gameObject.SetActive(true);

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
            float halfScreenHeight = Screen.height * 0.8f;
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
        ScriptablePet selectedPetData = petButtons[currentSelectedIndex].petData;
        selectedPetData.isAttacking = false;
        // 패널 위치 원래대로
        transform.DOLocalMoveY(originalPosition.y, animationSpeed)
            .SetEase(Ease.Linear);
    }
    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        healthImage.fillAmount = (float)currentHealth / maxHealth;
        healthText.text = currentHealth.ToString();
    }
    private void UpdateCoin(int moneyAmount)
    {
        if (moneyAnimation.attractorTarget != null) moneyAnimation.Play();
        else Debug.LogError("Oh My God, no Attractor Target in Particle Image. plzplzplz set target in it(메인_동전)");
        player.AddGold(moneyAmount);
       DOTween.To(() => player.gold - moneyAmount, // 시작값
        (value) => mainPlayerStatusView.UpdateCoinView(value), // 업데이트
        player.gold, // 목표값
        1f) // 애니메이션 시간
        .SetDelay(2f) // 2초 딜레이
        .SetEase(Ease.OutQuad);
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
