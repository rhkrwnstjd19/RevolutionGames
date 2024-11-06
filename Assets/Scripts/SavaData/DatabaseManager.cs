using System.Collections;
using System.Collections.Generic;
using System.IO;
using NSubstitute;
using TMPro;
using UnityEngine;

public class DatabaseManager : Singleton<DatabaseManager>
{
    public UserDatabase userDatabase;
    private string filePath;
    public ScriptablePlayer currentPlayer;
    public TMP_Text DebugText;
    void Start(){
        filePath = Path.Combine(Application.persistentDataPath, "playerData123.json");
        LoadUserDatabase();
    }
    private void LoadUserDatabase()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            userDatabase = JsonUtility.FromJson<UserDatabase>(json);
            Debug.Log("User database loaded from: " + filePath);
        }
        else
        {
            userDatabase = new UserDatabase();
            SaveUserDatabase(); // 파일이 없을 때 새로 저장하여 생성합니다.
            Debug.Log("No user database found, creating new one at: " + filePath);
        }
    }

    
    //데이터베이스 저장
    public void SaveUserDatabase()
    {
        string json = JsonUtility.ToJson(userDatabase, true);
        File.WriteAllText(filePath, json);
        Debug.Log("User database saved to: " + filePath);
        DebugText.text += "User database saved to: " + filePath;
    }

    public void SavePlayerData(ScriptablePlayer player)
    {
        int userPos = userDatabase.users.FindIndex(x => x.id == player.id);
        DebugText.text += "Exist!:" + userPos.ToString() + "\n";
        userDatabase.users[userPos].id = player.id;
        userDatabase.users[userPos].Level = player.Level;
        userDatabase.users[userPos].currentExp = player.currentExp;
        userDatabase.users[userPos].MaxExp = player.MaxExp;
        userDatabase.users[userPos].attackVal = player.attackVal;
        userDatabase.users[userPos].defenseVal = player.defenseVal;
        userDatabase.users[userPos].maxHp = player.maxHp;
        userDatabase.users[userPos].basicAttackCooldown = player.basicAttackCooldown;
        userDatabase.users[userPos].Stamina = player.Stamina;
        userDatabase.users[userPos].skill = player.skill;
        userDatabase.users[userPos].inventory = player.inventory;
        SaveUserDatabase();
    }

    public void CurrentPlayerData(ScriptablePlayer player)
    {
        currentPlayer = player;
    }

    
}
