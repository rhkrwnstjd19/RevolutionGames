using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{
    public GameObject CollectSceneButton;
    public GameObject StageSelectButton;
    public void OnButtonClickCollect() //������ ���� ��ư
    {
        StageSelectButton.SetActive(true);
    }

    public void OnButtonClickBack_StageSelect()
    {
        StageSelectButton.SetActive(false);
    }

    public void OnButtonClickStageOne() //������ ���� ����Ʈ ��ư
    {
        SceneManager.LoadScene("(UI)Test_Collect 1");
    }

    public void OnButtonClickGoToMain()
    {
        SceneManager.LoadScene("(UI)Main Map - DungeonRPG 1");
    }
}
