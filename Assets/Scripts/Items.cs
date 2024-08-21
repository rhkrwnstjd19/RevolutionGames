using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public struct ItemInfo
{
    public int cost;
    public float dmg;

    public ItemInfo(int cost, float dmg)
    {
        this.cost = cost;
        this.dmg = dmg;
    }
}

public class Items : MonoBehaviour
{
    public ItemInfo[] ITEM = new ItemInfo[6];
    public TMP_Text Header;
    public TMP_Text Context;

    //for test
    int test_gold = 100000;

    // Start is called before the first frame update
    void Start()
    {
        ITEM[0] = new ItemInfo(15000, 100);
        ITEM[3] = new ItemInfo(15000, 100);

        ITEM[1] = new ItemInfo(150000, 1000);
        ITEM[4] = new ItemInfo(150000, 1000);

        ITEM[2] = new ItemInfo(1500000, 10000);
        ITEM[5] = new ItemInfo(1500000, 10000);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Purchase_button(int itemIndex)
    {
        if (ITEM[itemIndex].cost <= DataManager.Instance.Money)
        {
            test_gold -= ITEM[itemIndex].cost;
            Header.text = "Purchased!";
            Context.text = "Nice choice you got there!";
            
        }
        else
        {
            Header.text = "Denied!!";
            Context.text = "you need more gold...";
        }
    }


}
