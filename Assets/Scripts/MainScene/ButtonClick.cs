using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{
    public GameObject StageListUI;
    public GameObject GameSelectButton;
    public void OnButtonClickCorrect() //수집형 던전 버튼
    {
        StageListUI.SetActive(true);
        GameSelectButton.SetActive(false);
    }

    public void OnButtonClickBack_StageSelect()
    {
        StageListUI.SetActive(false);
        GameSelectButton.SetActive(true);
    }

    public void OnButtonClickStageOne() //수집형 던전 리스트 버튼
    {
        SceneManager.LoadScene("Test_Collect");
    }

    public void OnButtonClickGoToMain()
    {
        SceneManager.LoadScene("Test_Main");
    }
}
