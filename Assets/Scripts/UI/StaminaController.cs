using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StaminaController : MonoBehaviour
{
    private Animator animator;
    public AnimationClip clip;
    public MainPlayerStatusView mainPlayerStatusView;
    bool first = true;

    void Start(){
        mainPlayerStatusView = GameObject.Find("PlayerStatus").GetComponent<MainPlayerStatusView>();
        first  = false;
        animator = gameObject.GetComponent<Animator>();
        gameObject.SetActive(false);
    }
    void OnEnable() {
        StaminaPlus();
    }
    void OnDisable() {
        
    }

    public void StaminaPlus(){
        Debug.Log("Active true stamina");
        if(!first)mainPlayerStatusView.StaminaPlus();
    }
}
