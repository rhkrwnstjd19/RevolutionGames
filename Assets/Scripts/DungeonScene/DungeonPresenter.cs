using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonPresenter : MonoBehaviour
{
    DungeonView view;
    DungeonModel model;
    int firepos = 0;
    public DungeonPresenter(DungeonView view){
        this.view = view;
        model = new DungeonModel();
        Debug.Log("presenter Invoke model.GetPlayerData");
        this.model.GetPlayerData();
        Debug.Log("presenter LoadPlayer");
        this.model.OnModelLoaded += LoadPlayer;
        LoadPlayer();
    }

    void LoadPlayer(){
        Debug.Log("presenter LoadPlayer");
        var player = model.player;
        view.InitPlayer(player);
        SwitchSkill(0);
    }

    public void LevelUp(){
        model.LevelUp();
        view.UpdatePlayerView();
    }
    public void SwitchSkill(int skillIndex){
        switch(skillIndex){
            case 0:
                view.SkillImage.sprite = model.player.skill[0].SkillIcon;
                view.Skill = model.player.skill[0];
                break;
            case 1:
                view.SkillImage.sprite = model.player.skill[1].SkillIcon;
                view.Skill = model.player.skill[1];
                break;
            case 2:
                view.SkillImage.sprite = model.player.skill[2].SkillIcon;
                view.Skill = model.player.skill[2];
                break;
        }
    }

    public void Fire(ScriptableSkill skill){
        // 기본공격
        if(skill.attackType == Enums.AttackType.Basic){
            if(firepos == 0){

                Instantiate(skill.SkillEffect, view.FirePosition1.position, Quaternion.identity);
                firepos = 1;
            }
            else{
                Instantiate(skill.SkillEffect, view.FirePosition2.position, Quaternion.identity);
                firepos = 0;
            }
        }
        // 범위 공격
        else if(skill.attackType == Enums.AttackType.MultipleAttack){
            skill.SkillEffect.GetComponent<Explosion>().Init();
        }
        //보스 공격
        else if(skill.attackType == Enums.AttackType.SingleAttack){
            MissileManager.Instance.Init();
        }
    }

    public void ExitDungeon(){
        model.SavePlayerData(view.player);
        SceneManager.LoadScene("Main Map - DungeonRPG");
    }


}
