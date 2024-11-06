using System;
using System.Collections;
using System.Collections.Generic;
using SharpUI.Source.Common.UI.Base.Presenter;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MainPlayerStatusModel
{
    public event Action OnModelLoaded;
    public ScriptablePlayer currentPlayer;
    public async void GetPlayerData()
    {
        var playerdata = Addressables.LoadAssetAsync<ScriptablePlayer>("SO/MAIN");
        await playerdata.Task;
        currentPlayer = playerdata.Result;
        OnModelLoaded?.Invoke();
    }

    public ScriptablePlayer StaminaPlus(){
        currentPlayer.Stamina += 0.5f;
        return currentPlayer;
    }
}
