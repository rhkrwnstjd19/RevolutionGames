using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.UI;
public class AdventureDetailPanel : MonoBehaviour
{
    public TMP_Text dungeonName;
    public TMP_Text dungeonLevel;
    public Transform buttonPanel;
    public Button CloseButton;
    public PetButton petButton;


    public void Init(AdvDungeon dungeon)
    {
        gameObject.SetActive(true);
        dungeonName.text = dungeon.dungeonName;
        dungeonLevel.text = dungeon.dungeonLevel.ToString();
        //기본 타겟 카메라의 Priority는 20
        dungeon.targetAheadCamera.Priority = 30;
    
        GetPetList();
        CloseButton.onClick.AddListener(() =>
        {
            dungeon.targetAheadCamera.Priority = 10;
            gameObject.SetActive(false);
        });
        //foreach(var pet in pets)PetButton button=Instantiate(petButton);
        //petButton.transform.parent=buttonPanel
    }
    private void GetPetList()
    {
        //***TODO***
        //return List<Pet>으로 변경
        //받아온 Pet 데이터 기반 PetButton Init

    }
}