using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainPlayerStatusView : MonoBehaviour
{
    public TMP_Text PlayerLevel;
    public TMP_Text CurrentGold;
    public TMP_Text CurrentExp;
    public Slider ExpSlider;
    
    public ScriptablePlayer player;

    // Start is called before the first frame update
    void Start()
    {
        PlayerLevel.text = player.Level.ToString();
        CurrentGold.text = player.inventory.gold.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
