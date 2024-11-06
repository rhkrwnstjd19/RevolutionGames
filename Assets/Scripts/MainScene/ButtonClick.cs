using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{
    public GameObject StageSelectButton;
    public GameObject NoticeButton_First;

    public void OnButtonClickCollect() //수집형 던전 아이콘을 누를 경우
    {
        StageSelectButton.SetActive(true);
    }

    public void OnButtonClickBack_StageSelect() //수집형 던전 리스트의 x 버튼을 누를 경우
    {
        StageSelectButton.SetActive(false);
    }

    public void OnButtonClickStage_First() //첫번째 수집형 던전 버튼을 누를 경우
    {
        NoticeButton_First.SetActive(true);
    }

    public void OnButtonClickStage_First_Enter()    //첫번째 수집형 던전 안내창에서 들어가기를 누를 경우
    {
        SceneManager.LoadScene("(UI)Test_Collect 1");
    }

    public void OnButtonClickStage_First_Back()    //첫번째 수집형 던전 안내창에서 뒤로가기를 누를 경우
    {
        NoticeButton_First.SetActive(false);
    }

    public void OnButtonClickGoToMain()     //메인으로 돌아가기 버튼을 누를 경우
    {
        SceneManager.LoadScene("(UI)Main Map - DungeonRPG 1");
    }
}
