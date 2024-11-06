using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class MainPlayerStatusView : MonoBehaviour
{
    public TMP_Text PlayerLevel;
    public TMP_Text CurrentGold;
    public TMP_Text CurrentExp;
    public Slider ExpSlider;

    public TMP_Text Stamina;
    
    public ScriptablePlayer currentPlayer;
    MainPlayerStatusPresenter presenter;


    void Awake(){
        presenter = new MainPlayerStatusPresenter(this);
    }

    public void UpdatePlayerView(){
        PlayerLevel.text = currentPlayer.Level.ToString();
        CurrentGold.text = "Gold : " + currentPlayer.inventory.gold.ToString();
        Stamina.text = "+" + currentPlayer.Stamina.ToString();
    }

    public void InitPlayer(ScriptablePlayer player){
        currentPlayer = player;
        UpdatePlayerView();
    }

    public void StaminaPlus(){
        presenter.StaminaPlus();
    }
}
