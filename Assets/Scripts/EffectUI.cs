using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class EffectUI : MonoBehaviour
{
    public Button nextButton;
    public Button previousButton;
    public TMP_Text effectName;
    public Toggle effectToggle;
    public Dictionary<string, GameObject> checkedEffects = new ();

    // 각 효과의 토글 상태를 저장할 Dictionary 추가
    private Dictionary<string, bool> effectToggleStates = new ();

    [Header("Effects")]
    public List<GameObject> effectsParent = new();

    private int currentEffectIndex = 0;
    private List<GameObject> _effects = new ();

    void Start()
    {
        foreach (GameObject parent in effectsParent)
        {
            foreach (Transform child in parent.transform)
            {
                _effects.Add(child.gameObject);
                child.gameObject.SetActive(false);
                // 초기 토글 상태를 false로 설정
                effectToggleStates[child.name] = false;
            }
        }
        Debug.Log(_effects.Count);
        nextButton.onClick.AddListener(NextEffect);
        previousButton.onClick.AddListener(PreviousEffect);
        effectToggle.onValueChanged.AddListener(delegate { ToggleEffect(); });
        _effects[currentEffectIndex].SetActive(true);
        UpdateToggleState();
        LoadCheckedEffects();
    }

    public void ToggleEffect()
    {
        string currentEffectName = _effects[currentEffectIndex].name;
        if (effectToggle.isOn)
        {
            checkedEffects[currentEffectName] = _effects[currentEffectIndex];
            effectToggleStates[currentEffectName] = true;
            Debug.Log(effectToggleStates[currentEffectName]);
        }
        else
        {
            checkedEffects.Remove(currentEffectName);
            effectToggleStates[currentEffectName] = false;
        }
    }

    public void NextEffect()
    {
        _effects[currentEffectIndex].SetActive(false);
        currentEffectIndex++;
        if (currentEffectIndex >= _effects.Count)
        {
            currentEffectIndex = 0;
        }
        _effects[currentEffectIndex].SetActive(true);
        UpdateToggleState();
    }

    public void PreviousEffect()
    {
        _effects[currentEffectIndex].SetActive(false);
        currentEffectIndex--;
        if (currentEffectIndex < 0)
        {
            currentEffectIndex = _effects.Count - 1;
        }
        _effects[currentEffectIndex].SetActive(true);
        UpdateToggleState();
    }

    private void UpdateToggleState()
    {
        string currentEffectName = _effects[currentEffectIndex].name;
        effectName.text = currentEffectName;
        effectToggle.isOn = effectToggleStates[currentEffectName];
    }

    private void SaveCheckedEffects()
    {
        EffectData data = new EffectData();
        data.checkedEffectNames = new List<string>(checkedEffects.Keys);

        string json = JsonUtility.ToJson(data);

        // 파일로 저장
        string path = Application.persistentDataPath + "/checkedEffects.json";
        File.WriteAllText(path, json);

        Debug.Log("Checked effects saved to: " + path);
    }

    private void LoadCheckedEffects()
    {
        string path = Application.persistentDataPath + "/checkedEffects.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            EffectData data = JsonUtility.FromJson<EffectData>(json);

            checkedEffects.Clear();
            foreach (string effectName in data.checkedEffectNames)
            {
                GameObject effect = _effects.Find(e => e.name == effectName);
                if (effect != null)
                {
                    checkedEffects[effectName] = effect;
                    effectToggleStates[effectName] = true;
                }
            }

            UpdateToggleState();
            Debug.Log("Checked effects loaded from: " + path);
        }
    }
    private void OnApplicationQuit()
    {
        SaveCheckedEffects();
    }
}
[System.Serializable]
public class EffectData
{
    public List<string> checkedEffectNames = new List<string>();
}