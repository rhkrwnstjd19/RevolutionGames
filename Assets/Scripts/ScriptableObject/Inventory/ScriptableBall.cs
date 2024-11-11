using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BallSO", menuName = "ScriptableObject/BallSO", order = 1)]
public class ScriptableBall : ScriptableObject
{
    public int id;
    public GameObject ballPrefab;
    public float catchRate;
    public Sprite ballSprite;
    public int ballPrice;
    public int ballCount=1;
    public int ballCountMax = 50;
    public void AddBall(int count)
    {
        ballCount += count;
    }
    public void UseBall(int count)
    {
        ballCount -= count;
    }
}
