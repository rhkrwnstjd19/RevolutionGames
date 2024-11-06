using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventorySO", menuName = "ScriptableObject/InventorySO", order = 1)]
public class ScriptableInventory : ScriptableObject
{
    public float gold=0;
    public List<GameObject> CatchBalls;
    
}
