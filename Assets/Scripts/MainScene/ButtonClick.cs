using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{
    public GameObject StageSelectButton;
    public GameObject NoticeButton_First;

    public void OnButtonClickCollect() //������ ���� �������� ���� ���
    {
        StageSelectButton.SetActive(true);
    }

    public void OnButtonClickBack_StageSelect() //������ ���� ����Ʈ�� x ��ư�� ���� ���
    {
        StageSelectButton.SetActive(false);
    }

    public void OnButtonClickStage_First() //ù��° ������ ���� ��ư�� ���� ���
    {
        NoticeButton_First.SetActive(true);
    }

    public void OnButtonClickStage_First_Enter()    //ù��° ������ ���� �ȳ�â���� ���⸦ ���� ���
    {
        SceneManager.LoadScene("MainCollect");
    }

    public void OnButtonClickStage_First_Back()    //ù��° ������ ���� �ȳ�â���� �ڷΰ��⸦ ���� ���
    {
        NoticeButton_First.SetActive(false);
    }

    public void OnButtonClickGoToMain()     //�������� ���ư��� ��ư�� ���� ���
    {
        SceneManager.LoadScene("(UI)Main Map - DungeonRPG 1");
    }
}
