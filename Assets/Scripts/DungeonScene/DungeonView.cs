using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DungeonView : MonoBehaviour
{
    public TMP_Text PlayerLevel;
    public TMP_Text CurrentGold;
    public TMP_Text CurrentExp;
    public Image SkillImage;

    public TMP_Dropdown SwitchSkill;
    public Slider ExpSlider;
    DungeonPresenter presenter;
    public GameObject Skill;
    public Button fireButton;
    public Transform FirePosition1;
    public Transform FirePosition2;

    void Awake()
    {
        presenter = new DungeonPresenter(this);
        SwitchSkill.onValueChanged.AddListener(delegate { presenter.SwitchSkill(SwitchSkill.value); });
        fireButton.onClick.AddListener(delegate{presenter.Fire(Skill);} );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePlayerView(ScriptablePlayer player){
        PlayerLevel.text = "Level : " + player.Level;
        CurrentExp.text = $"{player.currentExp/player.MaxExp}%";
    }
}
