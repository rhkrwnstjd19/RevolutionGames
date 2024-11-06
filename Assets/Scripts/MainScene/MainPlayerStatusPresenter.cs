using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainPlayerStatusPresenter : MonoBehaviour
{
    MainPlayerStatusView view;
    MainPlayerStatusModel model;
    public MainPlayerStatusPresenter(MainPlayerStatusView view){
        this.view = view;
        model = new MainPlayerStatusModel();
        this.model.GetPlayerData();
        this.model.OnModelLoaded += LoadPlayer;
    }

    public void LoadPlayer(){
        var player = model.currentPlayer;
        view.InitPlayer(player);
    }

    public void StaminaPlus(){
        view.currentPlayer = model.StaminaPlus();
        view.UpdatePlayerView();
    }


}
