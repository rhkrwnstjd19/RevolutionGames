using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EndMode : MonoBehaviour
{
    public TMP_Text resultText;

    private Player player;
    private void OnEnable()
    {
        UIController.ShowUI("End");
    }
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isCleared)
        {
            resultText.text = "You Defeated enemies all";
        }
        else if (player.isDead)
        {
            resultText.text = "You Dead";
        }
    }
}
