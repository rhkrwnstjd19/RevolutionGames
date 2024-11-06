using UnityEngine;
using System.Collections;
public enum AttackType
{
    Throw,
    Target,
    Defense
}
[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class SkillScript : ScriptableObject
{
    public string skillName;
    public string description;
    public Sprite skillIcon;
    public float cooldown;
    public float damage;
    public GameObject skillPrefab;

}
