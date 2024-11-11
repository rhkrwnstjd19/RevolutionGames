using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public enum Size{
    소형=1,
    중형,
    대형
}
[CreateAssetMenu(fileName = "ScriptableObjects/ScriptableDungeon", menuName = "ScriptableDungeon", order = 0)]
public class ScriptableDungeon : ScriptableObject {
    public string id{
        get{
            return $"{dungeonName}-{level}";
        }
    }
    public int level=1;
    public Size size=Size.소형;
    public string dungeonName{
        get{
            return $"{size.ToString()} 허수아비";
        }
    }
    public int maxHealth {
        get {
            return (int)(150 * Math.Pow(level, 4));
        }
    }
    
    public int moneyAmount{
        get{
            return (int)(10*Math.Pow(level, 2));
        }
    }
    public int expAmount{
        get{
            return (int)(10*Math.Pow(level, 3));
        }
    }
}
