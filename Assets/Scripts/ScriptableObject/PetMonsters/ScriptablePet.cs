using System.Collections;
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

[CreateAssetMenu(fileName = "PetSO", menuName = "ScriptableObject/PetSO", order = 1)]
public class ScriptablePet : ScriptableObject
{
    public string petName;
    public int level = 1;
    public int currentExp;
    public int maxExp
    {
        get
        {
            return level * 50;
        }
    }
    public int attackVal{
        get{
            return level*50;
        }
    }
    public bool isAttacking;
    public Sprite petSprite;
    public void AddExp(int addedExp){   
        currentExp+=addedExp;
        if(currentExp>=maxExp){
            LevelUp();
        }
    }
    public void LevelUp()
    {
        level++;
        currentExp-=maxExp;
        Debug.Log($"{petName} level up! current Level={level}");
    }
}
