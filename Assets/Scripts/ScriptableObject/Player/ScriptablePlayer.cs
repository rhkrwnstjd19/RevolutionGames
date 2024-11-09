using UnityEngine;
using Enums;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "PlayerSO", menuName = "ScriptableObject/PlayerSO", order = 1)]
public class ScriptablePlayer : ScriptableObject
{
    public string id;
    public int Level = 1;
    public float currentExp = 0;
    public float MaxExp = 50;
    public int attackVal;
    public int defenseVal;
    public int maxHp;
    public float basicAttackCooldown;
    public float Stamina = 0;
    public List<ScriptableSkill> skill;
    public ScriptableInventory inventory;

    public List<ScriptableBall> ballList;

    public ScriptablePetList petList;
    public void LevelUp(){
        currentExp = 0;
        MaxExp *= 2f;
        Level++;
        attackVal += 10;
        defenseVal += 10;
        maxHp += 100;

    }
}
