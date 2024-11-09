// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using UnityEditor;
// using UnityEngine;
// using UnityEngine.AddressableAssets;
// using UnityEngine.SceneManagement;

// public class DungeonModel : MonoBehaviour
// {
//     public event Action OnModelLoaded1;
//     public ScriptablePlayer player;
//     public string debug;
//     public async void GetPlayerData()
//     {
//         var playerdata = Addressables.LoadAssetAsync<ScriptablePlayer>("SO/MAIN");
//         await playerdata.Task;
//         player = playerdata.Result;
//         OnModelLoaded1?.Invoke();
//     }

//     public void SavePlayerData(ScriptablePlayer player){
        
        
        
//     }
//     public void LevelUp(){
//         player.LevelUp();
//     }
// }
