using UnityEngine;
using UnityEngine.UI;

public class PetButton : MonoBehaviour{
    public Image image;
    public void Init(Sprite sprite){
        image.sprite=sprite;
    }
}