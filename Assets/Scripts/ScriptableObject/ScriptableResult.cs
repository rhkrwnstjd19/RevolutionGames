using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResultSO", menuName = "ScriptableObject/ResultSO", order = 1)]
public class ScriptableResult : ScriptableObject
{
    public int Gold=0;
    public int EnemyCount=0;
    public float Exp=0;

}
