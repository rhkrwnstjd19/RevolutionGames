using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DungeonButton : MonoBehaviour
{
    public Button entranceButton;
    public TMP_Text entranceText;
    public DungeonInfo currentDungeonInfo;
    public int dungeonInfoIndex;

    private TMP_Text currentDungeonLevel;
    // Start is called before the first frame update
    void Start()
    {
        currentDungeonInfo = DummyManager.Instance.dungeon[dungeonInfoIndex];
        currentDungeonLevel = GetComponent<TMP_Text>();
        currentDungeonLevel.text = "LV." + currentDungeonInfo.dungeonLevel.ToString()+"\nDungeon";
        DungeonTextColor();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentDungeonInfo.isEnableEntrance)
        {
            enterEnable();
        }
        else
        {
            enterDisable();
        }
    }
    void DungeonTextColor()
    {
        switch (dungeonInfoIndex)
        {
            case 4:
                currentDungeonLevel.color = Color.red;break;
            case 3:
                currentDungeonLevel.color = Color.yellow; break;
            case 2:
                currentDungeonLevel.color = Color.blue; break;
            case 1:
                currentDungeonLevel.color = Color.green; break;
            case 0:
                currentDungeonLevel.color = Color.black; break;
        }
        
    }
    void enterEnable()
    {
        entranceButton.GetComponent<Image>().color= Color.green;
        entranceText.text = "Entrance Available";
    }
    void enterDisable()
    {
        entranceButton.GetComponent<Image>().color = Color.white;
        entranceText.text = "Not Available";
    }
}
