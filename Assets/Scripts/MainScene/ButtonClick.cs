using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonClick : MonoBehaviour
{
    public GameObject StageSelectButton;
    public GameObject NoticeButton_First;
    private AudioSource audioSource;     // 오디오 소스
    public AudioClip enterSound;
    public AudioClip backSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
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
        audioSource.PlayOneShot(enterSound);
        SceneManager.LoadScene("MainCollect");
    }

    public void OnButtonClickStage_First_Back()    //ù��° ������ ���� �ȳ�â���� �ڷΰ��⸦ ���� ���
    {
        NoticeButton_First.SetActive(false);
    }

    public void OnButtonClickGoToMain()     //�������� ���ư��� ��ư�� ���� ���
    {
        audioSource.PlayOneShot(backSound);
        SceneManager.LoadScene("(UI)Main Map - DungeonRPG 1");
    }
}
