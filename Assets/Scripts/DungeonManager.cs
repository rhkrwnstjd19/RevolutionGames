using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
namespace Game
{
    public class DungeonManager : Singleton<DungeonManager>
    {

        int level;
        public TMP_Text current_dungeon;
        public TMP_Text total_monster;
        public DummyManager dummyManager;
        public UIManager uiManager;
        public DungeonInfo dungeonInfo { get; private set; }
        // Start is called before the first frame update
        void Start()
        {
            current_dungeon.text = "";
            total_monster.text = "";

            Debug.Log(level);
            dungeonInfo = dummyManager.dungeon[level];
            check_stage();

        }

        // Update is called once per frame
        void Update()
        {

  
        }


        void check_stage()
        {
            if (level == 0)
            {
                current_dungeon.text = "Boss : Dungeon";
            }
            else
            {
                current_dungeon.text = "Lv" + level.ToString() + " : Dungeon";
                total_monster.text = dungeonInfo.monsterCount.ToString();
            }
        }

    }
}