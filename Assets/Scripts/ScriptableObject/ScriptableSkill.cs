using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;

[CreateAssetMenu(fileName = "SkillSO", menuName = "ScriptableObject/SkillSO", order = 1)]
public class ScriptableSkill : ScriptableObject
{
    public string SkillName;
    public AttackType attackType;
    public int AttackVal;
    public float Cooldown;
    public Sprite SkillIcon;
    public GameObject SkillEffect;
}
