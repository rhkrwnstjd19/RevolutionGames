using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using JetBrains.Annotations;

[System.Serializable]
public class UserData
{
    public string id;
    public int Level;
    public float currentExp;
    public float MaxExp;
    public int attackVal;
    public int defenseVal;
    public int maxHp;
    public float basicAttackCooldown;
    public float Stamina;
    public List<ScriptableSkill> skill;
    // public ScriptableInventory inventory;
    public List<ScriptableBall> ballList;
    public List<ScriptablePet> petList;
}
[System.Serializable]
public class UserDatabase
{
    public List<UserData> users = new List<UserData>();
}

public class LoginManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public Button loginButton;

    public ScriptablePlayer newPlayerData;
    private ScriptablePlayer currentPlayer;

    void Awake(){
        GetPlayerData();
    }
    public async void GetPlayerData()
    {
        var playerdata = Addressables.LoadAssetAsync<ScriptablePlayer>("SO/MAIN");
        await playerdata.Task;
        currentPlayer = playerdata.Result;
    }
    private void Start()
    {
        loginButton.onClick.AddListener(() => { if (usernameInput.text.Length > 0) AttemptLogin(usernameInput.text); });
    }


    
    //로그인 시도
    public void AttemptLogin(string username)
    {
        UserData loadedData = LoadUserData(username);

        if (loadedData != null)
        {
            Debug.Log("Login successful!");
            InitCurrentPlayer(loadedData);
            SceneManager.LoadScene("Main Map - DungeonRPG");
        }
        else
        {
            Debug.Log($"Invalid '{username}' or no saved user data found.");
            RegisterUser(username, newPlayerData);
            SceneManager.LoadScene("Main Map - DungeonRPG");
        }
    }

    void InitCurrentPlayer(UserData loadedData)
    {
        currentPlayer.id = loadedData.id;
        currentPlayer.Level = loadedData.Level;
        currentPlayer.currentExp = loadedData.currentExp;
        currentPlayer.MaxExp = loadedData.MaxExp;
        currentPlayer.attackVal = loadedData.attackVal;
        currentPlayer.defenseVal = loadedData.defenseVal;
        currentPlayer.maxHp = loadedData.maxHp;
        currentPlayer.basicAttackCooldown = loadedData.basicAttackCooldown;
        currentPlayer.Stamina = loadedData.Stamina;
        currentPlayer.skill = loadedData.skill;
        // currentPlayer.inventory = loadedData.inventory;
        currentPlayer.ballList = loadedData.ballList;
        currentPlayer.petList = loadedData.petList;

        DatabaseManager.Instance.CurrentPlayerData(currentPlayer);
    }

    //유저 데이터 로드
    public UserData LoadUserData(string username)
    {
        foreach (UserData user in DatabaseManager.Instance.userDatabase.users)
        {
            if (user.id == username)
            {
                Debug.Log("User data loaded: " + user.id);
                return user;
            }
        }

        Debug.LogWarning("User not found: " + username);
        return null;
    }

    //유저 등록
    public void RegisterUser(string username, ScriptablePlayer playerData)
    {
        UserData newUser = new UserData();
        newUser.id = username;
        newUser.Level = playerData.Level;
        newUser.currentExp = playerData.currentExp;
        newUser.MaxExp = playerData.MaxExp;
        newUser.attackVal = playerData.attackVal;
        newUser.defenseVal = playerData.defenseVal;
        newUser.maxHp = playerData.maxHp;
        newUser.basicAttackCooldown = playerData.basicAttackCooldown;
        newUser.Stamina = playerData.Stamina;
        newUser.skill = playerData.skill;
       // newUser.inventory = playerData.inventory;
        newUser.ballList = playerData.ballList;
        newUser.petList = playerData.petList;

        DatabaseManager.Instance.userDatabase.users.Add(newUser);
        DatabaseManager.Instance.SaveUserDatabase(); //
        InitCurrentPlayer(newUser);
        
        Debug.Log("User registered successfully!");
    }

}
