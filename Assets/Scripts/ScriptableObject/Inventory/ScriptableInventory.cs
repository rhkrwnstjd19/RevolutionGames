using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CatchBallInfo
{
    public GameObject ball;
    public int count;
    public CatchBallInfo(GameObject ball, int count)
    {
        this.ball = ball;
        this.count = count;
    }
}

[CreateAssetMenu(fileName = "InventorySO", menuName = "ScriptableObject/InventorySO", order = 1)]
public class ScriptableInventory : ScriptableObject
{
    [Header("Inventory")]
    public float gold = 0;
    public List<CaptureBall> ballList = new();
    [SerializeField]
    public List<CatchBallInfo> catchBallList = new();
}
