using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPresenter : MonoBehaviour
{
    DungeonView view;
    DungeonModel model;
    int firepos = 0;
    public DungeonPresenter(DungeonView view){
        this.view = view;
        model = new DungeonModel();
        this.model.GetPlayerData();
        this.model.OnModelLoaded += LoadPlayer;
    }

    void LoadPlayer(){
        var player = model.player;
        view.UpdatePlayerView(player);
    }

    public void SwitchSkill(int skillIndex){
        switch(skillIndex){
            case 0:
                view.SkillImage.sprite = model.player.skill[0].SkillIcon;
                break;
            case 1:
                view.SkillImage.sprite = model.player.skill[1].SkillIcon;
                break;
            case 2:
                view.SkillImage.sprite = model.player.skill[2].SkillIcon;
                break;
        }
    }

    public void Fire(GameObject skill){
        if(firepos == 0){
            Instantiate(skill, view.FirePosition1.position, Quaternion.identity);
            firepos = 1;
        }
        else{
            Instantiate(skill, view.FirePosition2.position, Quaternion.identity);
            firepos = 0;
        }
    }


}
