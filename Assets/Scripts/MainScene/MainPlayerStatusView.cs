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
    public Slider StaminaSlider;

    public TMP_Text Stamina;
    
    public ScriptablePlayer currentPlayer;
    MainPlayerStatusPresenter presenter;


    void Awake(){
        presenter = new MainPlayerStatusPresenter(this);
        UpdatePlayerView();
    }

    public void UpdatePlayerView(){
        PlayerLevel.text = currentPlayer.Level.ToString();
        CurrentGold.text = "Gold : " + currentPlayer.gold.ToString();
        Stamina.text = "+" + currentPlayer.Stamina.ToString();
        CurrentExp.text = currentPlayer.currentExp/currentPlayer.MaxExp * 100 + "%";
        ExpSlider.value = currentPlayer.currentExp / currentPlayer.MaxExp;
        StaminaSlider.value = currentPlayer.Stamina/ 100f;
    }

    public void InitPlayer(ScriptablePlayer player){
        currentPlayer = player;
        UpdatePlayerView();
    }

    public void StaminaPlus(){
        currentPlayer.Stamina += 0.5f;
        UpdatePlayerView();
        //presenter.StaminaPlus();
    }
}
