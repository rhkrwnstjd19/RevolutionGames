using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.UI;
using DG.Tweening;

public class AdventureDetailPanel : MonoBehaviour
{
    [Header("UI Component")]
    public TMP_Text dungeonName;
    public TMP_Text dungeonLevel;
    public Transform buttonPanel;
    public Button CloseButton;
    public PetButton petButton;
    public Button GoButton;

    public float animationSpeed = 1.5f;
    private Vector2 originalPosition;
    private Vector2 closePosition;

    private void Awake()
    {
        // 패널의 초기 위치를 저장합니다.
        originalPosition = transform.localPosition;
        closePosition = new Vector2(originalPosition.x, -Screen.height);
        transform.localPosition = closePosition;
    }

    public void Init(AdvDungeon dungeon)
    {
        gameObject.SetActive(true);
        dungeonName.text = dungeon.dungeonName;
        dungeonLevel.text = dungeon.dungeonLevel.ToString();
        dungeon.targetAheadCamera.Priority = 30;

        GetPetList();
        CloseButton.onClick.AddListener(() =>
        {
            dungeon.targetAheadCamera.Priority = 10;
            CloseAnimation();
        });
        GoButton.onClick.AddListener(() =>
        {
            StartDungeon();
        });

        OpenAnimation();
    }

    public void OpenAnimation()
    {
        // 패널이 부드럽게 올라오는 애니메이션
        transform.DOLocalMoveY(originalPosition.y, animationSpeed)
        .SetEase(Ease.Linear);
    }
    public void CloseAnimation()
    {
        // 패널이 부드럽게 올라오는 애니메이션
        transform.DOLocalMoveY(closePosition.y, animationSpeed)
        .SetEase(Ease.Linear)
        .OnComplete(() => gameObject.SetActive(false));
    }

    private void GetPetList()
    {
        // ***TODO***
        // return List<Pet>으로 변경
        // 받아온 Pet 데이터 기반 PetButton Init
    }

    public void StartDungeon()
    {
        // ***TODO***
    }
}
