using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class PetButton : MonoBehaviour{
    public Image image;
    public int buttonNumber{get;private set;}
    public bool isSelected{get;private set;}=false;
    public void Init(Sprite sprite,int number){
        buttonNumber=number;
        image.sprite=sprite;
    }
    public void WhenSelect(){
        isSelected=true;
    }
    public void Deselect(){
        isSelected=false;
    }
}