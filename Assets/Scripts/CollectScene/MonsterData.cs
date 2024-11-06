using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MonsterData", menuName = "ScriptableObjects/MonsterData", order = 1)]
public class MonsterData : ScriptableObject
{
    public string monsterName;
    public Sprite monsterImage;
    public string monsterDescription;
    public bool isCaptured = false;
}
