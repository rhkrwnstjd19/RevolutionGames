using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    public static T Instance { get; private set; }
    public static bool IsInitiailized
    {
        get { return Instance != null; }
    }
    protected virtual private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("error occured");
        }
        else
            Instance = (T)this;
    }
    protected virtual void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
}
