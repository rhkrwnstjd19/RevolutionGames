using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using GoMap;
public class AdvButtons : MonoBehaviour {
    
    public Image dungeonImage;
    public TMP_Text dungeonName;
    public TMP_Text dungeonLevel;
    public TMP_Text BuildingName;
    Button dungeonButton;

    public void Init(AdvDungeon dungeon,UnityAction<AdvDungeon> onClickEvent){
        // dungeonImage
        dungeonButton=GetComponent<Button>();
        dungeonName.text=dungeon.dungeonName;
        dungeonLevel.text=$"Lv.{dungeon.dungeonLevel}";
        dungeonButton.onClick.AddListener(()=>onClickEvent(dungeon));
        BuildingName.text = dungeon.GetComponentInParent<GOFeatureBehaviour>().goFeature.name;
    }

}