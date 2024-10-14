using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSO", menuName = "ScriptableObject/PlayerSO", order = 1)]
public class ScriptablePlayer : ScriptableObject
{
    public string id;
    public int currentLevel = 1;
    public int currentExp = 0;
    public int MaxExp = 50;
    public int attackVal;
    public int defenseVal;
    public int maxHp;
    public float basicAttackCooldown;
    public string skill;
    public void LevelUp(){
        currentLevel++;
        attackVal += 10;
        defenseVal += 10;
        maxHp += 100;

    }
}
