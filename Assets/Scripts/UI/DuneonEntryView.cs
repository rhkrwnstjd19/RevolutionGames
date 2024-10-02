using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DuneonEntryView : Singleton<DuneonEntryView>
{
    public GameObject confirmationPanel; // 확인 UI 패널
    public Button Accept;
    public Button Diffuse;
    void Awake()
    {
        // 시작 시 확인 패널을 비활성화합니다.
        if (confirmationPanel != null)
            confirmationPanel.SetActive(false);
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
                    Debug.Log("터치한 오브젝트: " + hit.transform.name + ", 태그: " + hit.transform.tag);

                    if (hit.transform.name == "학생식당")
                    {
                        ShowConfirmation();
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

                if (hit.transform.tag == "Dungeon")
                {
                    Debug.Log("123클릭한 오브젝트: " + hit.transform.name);
                    ShowConfirmation();
                }
            }
        }
    }

    void ShowConfirmation()
    {
        if (confirmationPanel != null)
            confirmationPanel.SetActive(true);
    }

    // 수락 버튼 클릭 시 호출되는 함수
    public void OnAccept()
    {
        SceneManager.LoadScene("dungeon_scene");
    }

    // 취소 버튼 클릭 시 호출되는 함수
    public void OnCancel()
    {
        if (confirmationPanel != null)
            confirmationPanel.SetActive(false);
    }
}
