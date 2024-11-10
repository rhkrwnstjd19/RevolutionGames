using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(fileName = "ScriptableObjects", menuName = "ScriptableDungeonList", order = 0)]
public class ScriptableDungeonList : ScriptableObject {
    public List<ScriptableDungeon> list;
}
