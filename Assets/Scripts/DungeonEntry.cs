using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonEntry : MonoBehaviour
{
    public GameObject confirmationPanel; // 확인 UI 패널

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
                    Debug.Log("터치한 오브젝트: " + hit.transform.name);
                    if (hit.transform == transform)
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
                Debug.Log("터치한 오브젝트: " + hit.transform.name);
                if (hit.transform == transform)
                {
                    ShowConfirmation();
                }
            }
        }
    }

    void ShowConfirmation()
    {
        if (confirmationPanel != null){
          // DuneonEntryView.Instance.ShowConfirmation();
            Debug.Log($"confirmationPanel is not null : {confirmationPanel}");
        }
        else{
            Debug.Log($"confirmationPanel is null : {confirmationPanel}");
        }
    }

    // 수락 버튼 클릭 시 호출되는 함수
    public void OnAccept()
    {
        SceneManager.LoadScene("DungeonScene");
    }

    // 취소 버튼 클릭 시 호출되는 함수
    public void OnCancel()
    {
        if (confirmationPanel != null)
            confirmationPanel.SetActive(false);
    }
}
