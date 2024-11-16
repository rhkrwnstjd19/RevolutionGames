using System.Collections;
using System.Collections.Generic;
using Game;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameResult : MonoBehaviour
{
    [Header("Game Result")]
    public TMP_Text Gold;
    public TMP_Text EnemyCount;
    public TMP_Text Exp;

    public GameObject NewRecordGold;
    public GameObject NewRecordEnemy;
    public GameObject NewRecordExp;

    private int gold;
    private int enemyCount;
    private float exp;

    public ScriptableResult result;

    public AudioSource audioSource;     // 오디오 소스
    public AudioClip resultSound;

    void Start(){
        AddressablesLoad();
        SetResult();
    }
    public void SetResult()
    {
        this.gold = DungeonManager.Instance.TotalGold;
        this.enemyCount = DungeonManager.Instance.DefeatedEnemyCount;
        this.exp = DungeonManager.Instance.TotalExp;

        StartCoroutine(ShowResult());
        audioSource = GetComponent<AudioSource>();
    }

    async void AddressablesLoad()
    {
        var handle = Addressables.LoadAssetAsync<ScriptableResult>("ResultSO");
        await handle.Task;
        result = handle.Result;
    }
    IEnumerator ShowResult()
    {
        yield return StartCoroutine(AnimateValue(Gold, 0, gold, 0.5f, "Gold : "));
        yield return StartCoroutine(AnimateValue(EnemyCount, 0, enemyCount, 0.5f, "Enemy : "));
        yield return StartCoroutine(AnimateValue(Exp, 0, exp, 0.5f, "Total Exp : "));
        audioSource.PlayOneShot(resultSound);
        CheckNewRecord();
    }

    IEnumerator AnimateValue(TMP_Text uiText, float startValue, float endValue, float duration, string prefix = "")
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float currentValue = Mathf.Lerp(startValue, endValue, elapsedTime / duration);
            uiText.text = prefix + Mathf.RoundToInt(currentValue).ToString();
            yield return null;
        }

        // Ensure the value reaches the exact target value at the end
        uiText.text = prefix + endValue.ToString();
    }


    void CheckNewRecord(){
        if(result.Gold > gold){
            result.Gold = gold;
            NewRecordGold.SetActive(true);
        }
        if(result.EnemyCount > enemyCount){
            result.EnemyCount = enemyCount;
            NewRecordEnemy.SetActive(true);
        }
        if(result.Exp > exp){
            result.Exp = exp;
            NewRecordExp.SetActive(true);
        }
    }
}
