using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    private MainPlayerStatusView mainPlayerStatusView;
    public List<Button> Items;
    private ScriptablePlayer player;
    public Button exitButton;

    public TMP_Text purchaseText;


    void Awake(){
        mainPlayerStatusView = FindObjectOfType<MainPlayerStatusView>();
        player = mainPlayerStatusView.currentPlayer;
        foreach (var item in Items)
        {
            item.onClick.AddListener(delegate{StartCoroutine(BuyItem(item));});
        }
    }


    IEnumerator BuyItem(Button b){
        int findIndex = Items.FindIndex(x => x == b);   
        if(player.gold >= 10 * (findIndex + 1)){
            player.gold -= 10 * (findIndex + 1);
            player.ballList[findIndex].ballCount++;
            purchaseText.text = "구매완료!";
            Debug.Log("Item Bought");
        }
        else{
            purchaseText.text = "골드가 부족한건 아닌가요?";
            Debug.Log("Not enough gold");
        }

        yield return new WaitForSeconds(1.0f);
        purchaseText.text = "";
    }
}
