using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{
    public GameObject StageListUI;
    public GameObject GameSelectButton;
    public void OnButtonClickCorrect() //������ ���� ��ư
    {
        StageListUI.SetActive(true);
        GameSelectButton.SetActive(false);
    }

    public void OnButtonClickBack_StageSelect()
    {
        StageListUI.SetActive(false);
        GameSelectButton.SetActive(true);
    }

    public void OnButtonClickStageOne() //������ ���� ����Ʈ ��ư
    {
        SceneManager.LoadScene("Test_Collect");
    }

    public void OnButtonClickGoToMain()
    {
        SceneManager.LoadScene("Test_Main");
    }
}
