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
    
    public ScriptablePlayer currentPlayer;

    void Awake(){
        GetPlayerData();
    }
    public async void GetPlayerData()
    {
        var playerdata = Addressables.LoadAssetAsync<ScriptablePlayer>("PlayerData");
        await playerdata.Task;
        currentPlayer = playerdata.Result;
        UpdatePlayerView();
    }
    void Start()
    {
        
    }

    public void UpdatePlayerView(){
        PlayerLevel.text = currentPlayer.Level.ToString();
        CurrentGold.text = "Gold : " + currentPlayer.inventory.gold.ToString();
    }
}
