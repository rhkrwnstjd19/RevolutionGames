using UnityEngine;
using Enums;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "PlayerSO", menuName = "ScriptableObject/PlayerSO", order = 1)]
public class ScriptablePlayer : ScriptableObject
{
    public string id;
    public int Level = 1;
    public float currentExp = 0;
    public float MaxExp{
        get{
            return 100*Level;
        }
    }
    public int attackVal;
    public int defenseVal;
    public int maxHp;
    public float basicAttackCooldown;
    public float Stamina = 0;
    public int gold = 0;
    public List<ScriptableSkill> skill;
    public List<ScriptableBall> ballList;
    public List<ScriptablePet> petList;
    //public ScriptableInventory inventory;

    // public List<ScriptableBall> ballList;

    // public ScriptablePetList petList;
    public void LevelUp(){
        currentExp = 0;
        Level++;
        attackVal += 10;
        defenseVal += 10;
        maxHp += 100;

    }
    public void AddGold(int gold){
        this.gold+=gold;
    }
}
