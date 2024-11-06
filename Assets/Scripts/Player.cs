using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int curHp { get; private set; }
    [HideInInspector]public bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        curHp = DataManager.Instance.PlayerMaxHp;
        isDead = false;
        GameManager.Instance.isPlayerDead = isDead;
    }
    
    public void decreaseHp(int attackAmount)
    {
        //HP °¨¼Ò
        curHp -= attackAmount;
        if (curHp <= 0)
        {
            isDead = true;
            GameManager.Instance.isPlayerDead = isDead;
            curHp = 0;
        }
    }
}
