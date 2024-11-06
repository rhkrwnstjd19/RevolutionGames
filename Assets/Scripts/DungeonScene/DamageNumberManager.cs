using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DamageNumbersPro;
public class DamageNumberManager : Singleton<DamageNumberManager>
{
    public DamageNumber DamageType;
    string nullText = "";
    public void ShowDamageNumber(Vector3 position, float damage)
    {
        DamageType.transform.position = new Vector3(position.x, position.y + 0.3f, position.z);
        if(damage >0)DamageType.Spawn(position, damage);
        else DamageType.Spawn(position, nullText);
    }
}
