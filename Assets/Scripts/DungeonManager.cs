using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//아직 사용 미정.
namespace Game
{
    public class DungeonManager : Singleton<DungeonManager>
    {
        public DungeonView dungeonView;

        // Start is called before the first frame update

        public void UpdateExp(float exp){
            dungeonView.UpdateExp(exp);
        }
        

    }
}