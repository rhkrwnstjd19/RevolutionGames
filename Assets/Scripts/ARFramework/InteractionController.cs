using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

[System.Serializable]
public class InteracionModeDictionay :
       SerializableDictionaryBase<string, GameObject>
{ }
public class InteractionController : Singleton<InteractionController>
{
    [SerializeField] InteracionModeDictionay interactionModes;
    GameObject currentMode;

    private void Awake()
    {
        base.Awake();
        ResetAllNodes();
    }
    void ResetAllNodes()
    {
        foreach(GameObject mode in interactionModes.Values)
        {
            mode.SetActive(false);
        }
    }
    public static void EnableMode(string name)
    {
        Instance?._EnableMode(name);
    }
    void _EnableMode(string name)
    {
        GameObject modeObject;
        if(interactionModes.TryGetValue(name,out modeObject))
        {
            StartCoroutine(ChangeMode(modeObject));
        }
        else
        {
            Debug.LogError("Undefined MOde");
        }
    }
    IEnumerator ChangeMode(GameObject mode)
    {
        if (mode == currentMode)
            yield break;
        if (currentMode)
        {
            currentMode.SetActive(false);
            yield return null;
        }
        currentMode = mode;
        mode.SetActive(true);
    }
    private void Start()
    {
        _EnableMode("Startup");
    }

}
