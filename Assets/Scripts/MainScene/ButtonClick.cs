using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{
    public GameObject StageListUI;
    public GameObject Button;
    public void OnButtonClickOne() //������ ���� ��ư
    {
        StageListUI.SetActive(true);
        Button.SetActive(false);
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
