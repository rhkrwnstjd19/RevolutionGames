using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

[Serializable]
public class PlayerData
{
    public string ID;
    public float level;
    public float xp;
    public float damage;
    public bool isOnline;

    public PlayerData(string ID, float lv, float x, float dmg, bool isOn)
    {
        this.ID = ID;
        level = lv;
        xp = x;
        damage = dmg;
        isOnline = isOn;
    }
}

public class PlayerDataManager : MonoBehaviour
{
    [Serializable]
    private class Serialization<T>
    {
        public List<T> Player;

        public Serialization(List<T> Player)
        {
            this.Player = Player;
        }

        public List<T> ToList()
        {
            return Player;
        }
    }
    [ContextMenu("From Json Data")]
    private void LoadDataFromJson(string id){
        string path = Path.Combine(Application.dataPath, "playerData.json");
        List<PlayerData> allPlayerData = new List<PlayerData>();

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            allPlayerData = JsonUtility.FromJson<Serialization<PlayerData>>(json).ToList();
        }
        foreach(var a in allPlayerData){
            if (a.ID == id) {
                OnPlayerDataLoaded(a);
                return ;
            }
        }
        PlayerData newPlayer = new PlayerData(id, 1, 0, 5, false);
        StartCoroutine(AddPlayerData(newPlayer));
    }
    private void OnPlayerDataLoaded(PlayerData playerData)
    {
       
    }

    [ContextMenu("To Json Data")]
    void SaveData()
    {
        PlayerData data2 = new PlayerData("Admin", 999, 0, 99999, true);
        string jsonPlayer = JsonUtility.ToJson(data2,true);
        SavePlayerData(data2);

    }

    public void SavePlayerData(PlayerData newData)
    {
        string path = Path.Combine(Application.dataPath, "playerData.json");
        List<PlayerData> allPlayerData = new List<PlayerData>();

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            allPlayerData = JsonUtility.FromJson<Serialization<PlayerData>>(json).ToList();
        }
        foreach(var a in allPlayerData){
            if (a.ID == newData.ID) return ;
        }
        allPlayerData.Add(newData);

        string updatedJson = JsonUtility.ToJson(new Serialization<PlayerData>(allPlayerData), true);
        File.WriteAllText(path, updatedJson);
    }

    public IEnumerator AddPlayerData(PlayerData playerData)
    {
        string jsonPlayer = JsonUtility.ToJson(playerData, true);

        SavePlayerData(playerData);

        OnPlayerDataLoaded(playerData);

        yield return new WaitForSeconds(5.0f);
    }

   
    

    [ContextMenu("To Json Data")]
    public PlayerData UpdatePlayerData(PlayerData playerData)
    {
        

       string path = Path.Combine(Application.dataPath, "playerData.json");
        List<PlayerData> allPlayerData = new List<PlayerData>();

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            allPlayerData = JsonUtility.FromJson<Serialization<PlayerData>>(json).ToList();
        }
        foreach(var a in allPlayerData){
            if (a.ID == playerData.ID) {
                a.level +=1;
                a.damage = 5 + (a.level-1)*5;
                string updatedJson = JsonUtility.ToJson(new Serialization<PlayerData>(allPlayerData), true);
                File.WriteAllText(path, updatedJson);
                return a;
            }
        }

        return null;
    }
    // public async void LoadPlayerDataAsync(string playerID, System.Action<PlayerData> onCompleted)
    // {
    //     await databaseReference.Child("Player").GetValueAsync().ContinueWithOnMainThread(task =>
    //     {
    //         if (task.IsCompleted)
    //         {
    //             dubugText.text = "In task...complete";
    //             DataSnapshot snapshot = task.Result;
    //             List<PlayerData> allPlayerData = new List<PlayerData>();

    //             foreach (DataSnapshot playerSnapshot in snapshot.Children)
    //             {
    //                 string json = playerSnapshot.GetRawJsonValue();
                    
    //                 Debug.Log($"json : {json}");

    //                 try
    //                 {
    //                     PlayerData playerData = JsonUtility.FromJson<PlayerData>(json);
    //                     if (playerData != null)
    //                     {
    //                         SavePlayerData(playerData);
    //                         allPlayerData.Add(playerData);
    //                     }
    //                 }
    //                 catch (Exception ex)
    //                 {
    //                     Debug.LogError($"Failed to parse JSON: {ex.Message}");
    //                 }
    //             }

    //             // 모든 플레이어 데이터를 로드한 후 처리
    //             foreach (var player in allPlayerData)
    //             {
    //                 if (player.ID == playerID)
    //                 {
    //                     dubugText.text = "Player exist!";
    //                     onCompleted(player);
    //                     return;
    //                 }
    //             }

    //             // PlayerID가 없는 경우 기본 값을 반환
    //             PlayerData newPlayer = new PlayerData(playerID, 1, 0, 5, false);
    //             dubugText.text = "Player don't exist!";
    //             AddPlayerData(newPlayer);
    //             onCompleted(newPlayer);
    //         }
    //         else
    //         {
    //             dubugText.text = "In task...fail";
    //             Debug.LogError("Failed to load player data: " + task.Exception);
    //         }
    //     });
    // }
}
