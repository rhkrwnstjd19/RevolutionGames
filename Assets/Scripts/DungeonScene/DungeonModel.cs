using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DungeonModel
{
    public event Action OnModelLoaded;
    public ScriptablePlayer player;
    public async void GetPlayerData()
    {
        Debug.Log("model GetPlayerData");
        var playerdata = Addressables.LoadAssetAsync<ScriptablePlayer>("PlayerData");
        await playerdata.Task;
        player = playerdata.Result;
        Debug.Log($"{player} model GetPlayerData Done");
        OnModelLoaded?.Invoke();

    }

    public void SavePlayerData(ScriptablePlayer player){
        DatabaseManager.Instance.SavePlayerData(player);
    }
    public void LevelUp(){
        player.LevelUp();
    }
}
