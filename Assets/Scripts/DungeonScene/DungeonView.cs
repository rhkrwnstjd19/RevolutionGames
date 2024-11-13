using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class DungeonView : MonoBehaviour
{
    [Header("player info UI")]
    public TMP_Text PlayerLevel;
    public TMP_Text CurrentGold;
    public TMP_Text CurrentExp;
    public Image SkillImage;
    public TMP_Dropdown SwitchSkill;
    public Slider ExpSlider;
    DungeonPresenter presenter;
    public ScriptableSkill Skill;
    public Button fireButton;
    public Button ExitDungeon;

    [Header("Fire positions for skills")]
    public Transform FirePosition1;
    public Transform FirePosition2;

    public ScriptablePlayer player;
    
    private float pendingExp = 0f;
    private bool isAnimatingExp = false;

    public TMP_Text dubuggingText;
    void Awake()
    {
        presenter = new DungeonPresenter(this);
        SwitchSkill.onValueChanged.AddListener(delegate { presenter.SwitchSkill(SwitchSkill.value); });
        fireButton.onClick.AddListener(delegate{presenter.Fire(Skill);} );
        ExitDungeon.onClick.AddListener(delegate{presenter.ExitDungeon();});
        UpdatePlayerView();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePlayerView(){
        PlayerLevel.text = "Level : " + player.Level;
        CurrentExp.text = $"{(player.currentExp / player.MaxExp) * 100:F2}%";
        ExpSlider.maxValue = player.MaxExp;
        ExpSlider.value = player.currentExp;
        CurrentGold.text = "Gold : " + player.gold.ToString();
    }

    public void InitPlayer(ScriptablePlayer player){
        this.player = player;
        UpdatePlayerView();
    }
    public void UpdateExp(float exp)
    {
        pendingExp += exp;
        Debug.Log("UpdateExp : " + pendingExp);
        if (!isAnimatingExp)
        {
            StartCoroutine(ExpAnimation());
        }
    }

    IEnumerator ExpAnimation()
    {
        isAnimatingExp = true;
        while (pendingExp > 0)
        {
            float currentExp = player.currentExp;
            float maxExp = player.MaxExp;
            float expToAdd = pendingExp;
            pendingExp = 0f;

            if (currentExp + expToAdd >= maxExp)
            {
                float expForLevelUp = maxExp - currentExp;
                float remainExp = (currentExp + expToAdd) - maxExp;

                // 경험치 바 애니메이션
                float t = 0f;
                while (t < 1)
                {
                    t += Time.deltaTime;
                    ExpSlider.value = Mathf.Lerp(currentExp, maxExp, t);
                    yield return null;
                }

                LevelUp();
                player.currentExp = 0f;
                pendingExp += remainExp;
                continue; // 남은 경험치 처리
            }
            else
            {
                float targetExp = currentExp + expToAdd;
                float t = 0f;
                while (t < 1)
                {
                    t += Time.deltaTime;
                    ExpSlider.value = Mathf.Lerp(currentExp, targetExp, t);
                    yield return null;
                }
                player.currentExp = targetExp;
                UpdatePlayerView();
                pendingExp = 0f;
            }
        }
        isAnimatingExp = false;
    }
    void LevelUp(){
        Debug.Log($"Player Level UP : {player.currentExp} / {player.MaxExp}");
        presenter.LevelUp();
    }

    public void UpdateGold(int gold)
    {
        player.gold += gold;
        UpdatePlayerView();
    }
}
