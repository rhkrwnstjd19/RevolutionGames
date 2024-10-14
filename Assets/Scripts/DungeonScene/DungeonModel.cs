using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DungeonModel : MonoBehaviour
{
    public event Action OnModelLoaded;
    public ScriptablePlayer player;
    string scriptablePlayerPath;
    public async void GetPlayerData()
    {
        var playerdata = Addressables.LoadAssetAsync<ScriptablePlayer>("PlayerData");
        await playerdata.Task;
        player = playerdata.Result;
        OnModelLoaded?.Invoke();
    }
}
