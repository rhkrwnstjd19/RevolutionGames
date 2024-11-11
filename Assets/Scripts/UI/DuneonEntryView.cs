using System;
using System.Collections;
using System.Collections.Generic;
using Castle.Components.DictionaryAdapter.Xml;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DuneonEntryView : Singleton<DuneonEntryView>
{
    public GameObject RPGDungeonPanel; // 확인 UI 패널
    public GameObject BossDungeonPanel; // 확인 UI 패널
    public AdventureDetailPanel AdventurePanel;
    public GameObject Buttons;
    public Button Accept;
    public Button Diffuse;
    private bool isRPG = false;
    void Start()
    {
        // 시작 시 확인 패널을 비활성화합니다.
        if (RPGDungeonPanel != null)
            RPGDungeonPanel.SetActive(false);
        if (BossDungeonPanel != null)
            BossDungeonPanel.SetActive(false);
        if (Buttons != null)
            Buttons.SetActive(false);
        Accept.onClick.AddListener(OnAccept);
        Diffuse.onClick.AddListener(OnCancel);
    }

    void Update()
    {
        // 터치 입력 처리
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    // 터치한 오브젝트의 이름과 태그를 디버그 로그로 출력합니다.
                    if (hit.transform.tag == "BossDungeon")
                    {
                        isRPG = false;
                        Debug.Log("123클릭한 오브젝트: " + hit.transform.name);
                        ShowConfirmation();
                    }
                    else if (hit.transform.tag == "RPGDungeon")
                    {
                        isRPG = true;
                        Debug.Log("123클릭한 오브젝트: " + hit.transform.name);
                        ShowConfirmation();
                    }
                    else if (hit.transform.tag == "AdventureDungeon")
                    {
                        AdvDungeon touchedDungeon = hit.transform.GetComponent<AdvDungeon>();
                        if (AdventurePanel == null) Debug.LogError("OMG, Adventure Detail Panel is null. \nplease put fucking panel into hierarchy");
                        else if (touchedDungeon == null) Debug.LogError("OMG, Dungeon Script is null. \nDid you idiot forget to put script in prefab?");
                        else AdventurePanel.Init(touchedDungeon);
                    }
                }
            }
        }
        // 마우스 클릭 처리 (에디터 테스트용)
        else if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // 클릭한 오브젝트의 이름을 디버그 로그로 출력합니다.
                Debug.Log("클릭한 오브젝트: " + hit.transform.name + ", 태그: " + hit.transform.tag);

                if (hit.transform.tag == "BossDungeon")
                {
                    isRPG = false;
                    Debug.Log("123클릭한 오브젝트: " + hit.transform.name);
                    ShowConfirmation();
                }   
                else if (hit.transform.tag == "RPGDungeon")
                {
                    isRPG = true;
                    Debug.Log("123클릭한 오브젝트: " + hit.transform.name);
                    ShowConfirmation();
                }
                else if (hit.transform.tag == "AdventureDungeon")
                {
                    AdvDungeon touchedDungeon = hit.transform.GetComponent<AdvDungeon>();
                    if (AdventurePanel == null) Debug.LogError("OMG, Adventure Detail Panel is null. please put fucking panel into hierarchy");
                    else if (touchedDungeon == null) Debug.LogError("OMG, Dungeon Script is null. Did you idiot forget to put script in prefab?");
                    else AdventurePanel.Init(touchedDungeon);
                }
            }
        }
    }

    void ShowConfirmation()
    {
        Buttons.SetActive(true);
        if (isRPG)
            RPGDungeonPanel.SetActive(true);
        else
        {
            BossDungeonPanel.SetActive(true);
        }
    }

    // 수락 버튼 클릭 시 호출되는 함수
    public void OnAccept()
    {
        if (isRPG) SceneManager.LoadScene("RPGDungeon");
        else SceneManager.LoadScene("BossDungeon");
    }

    // 취소 버튼 클릭 시 호출되는 함수
    public void OnCancel()
    {
        RPGDungeonPanel.SetActive(false);
        BossDungeonPanel.SetActive(false);
        Buttons.SetActive(false);
    }
}
