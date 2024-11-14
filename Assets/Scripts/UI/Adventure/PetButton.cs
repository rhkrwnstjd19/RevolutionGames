using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PetButton : MonoBehaviour
{
    public Image image;
    public GameObject selectedEffect;
    public GameObject workingEffect;
    public int buttonNumber { get; private set; }
    public bool isSelected { get; private set; } = false;
    public ScriptablePet petData{get;private set;}
    private Button button;
    

    public void Init(ScriptablePet petData, int number, UnityAction<PetButton> onClickEvent)
    {
        if(button==null)
            button = GetComponent<Button>();
        this.petData=petData;
        buttonNumber = number;
        image.sprite=this.petData.petSprite;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClickEvent(this));
        //이미 탐험을 진행중인 펫은 선택 불가.
        workingEffect.SetActive(petData.isAttacking);
        button.interactable=!petData.isAttacking;
    }
    public void WhenSelect()
    {
        isSelected = true;
        selectedEffect.SetActive(true);
    }

    public void Deselect()
    {
        isSelected = false;
        selectedEffect.SetActive(false);
    }
}