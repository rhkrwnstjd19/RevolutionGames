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
    public float maxExp{
        get{
            return level*150;
        }
    }
    public int attackVal=10;

    public Sprite petSprite;

    public void LevelUp()
    {
        level++;
        currentExp = 0;
        attackVal += 10;
    }
}
