using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{
    public GameObject StageListUI;
    public GameObject Button;
    public void OnButtonClickOne() //수집형 던전 버튼
    {
        StageListUI.SetActive(true);
        Button.SetActive(false);
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
