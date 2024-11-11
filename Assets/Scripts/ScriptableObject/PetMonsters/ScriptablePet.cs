using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[CreateAssetMenu(fileName = "PetSO", menuName = "ScriptableObject/PetSO", order = 1)]
public class ScriptablePet : ScriptableObject
{
    public string petName;
    public int level=1;
    public float currentExp;
    public float maxExp;
    public int attackVal=10;

    public Sprite petSprite;

    public void LevelUp()
    {
        level++;
        maxExp = level * 100;
        currentExp = 0;
        attackVal += 10;
    }
}
