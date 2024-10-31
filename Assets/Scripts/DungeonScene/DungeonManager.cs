using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

//아직 사용 미정.
namespace Game
{
    public class DungeonManager : Singleton<DungeonManager>
    {
        [Header("Dungeon Info")]
        public int DefeatedEnemyCount = 0;
        public float TotalExp = 0;
        public int TotalGold = 0;

        public DungeonView dungeonView;

        public void UpdateExp(float exp){
            dungeonView.UpdateExp(exp);
            TotalExp += exp;
        }

        
        

    }
}