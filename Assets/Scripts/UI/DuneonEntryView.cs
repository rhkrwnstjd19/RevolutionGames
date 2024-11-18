using System;
using System.Collections;
using System.Collections.Generic;
using Castle.Components.DictionaryAdapter.Xml;
using Cinemachine;
using DG.Tweening;
using Unity.VisualScripting;
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
    public GameObject ShopUI;
    private bool isRPG = false;
    public CinemachineVirtualCamera targetAheadCamera;
    
    private AudioSource audioSource;     // 오디오 소스
    public AudioClip enterSound;
    public AudioClip backSound;
    private GameObject CurrentPanel;
    private Vector2 originalPosition;
    private Vector2 closePosition;
    public float animationSpeed = 1.5f;
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
        audioSource = GetComponent<AudioSource>();
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
                        Debug.Log("123클릭한 오브젝트: " + hit.transform.name);
                        CurrentPanel = BossDungeonPanel;
                        isRPG = false;
                        ShowConfirmation();
                    }
                    else if (hit.transform.tag == "RPGDungeon")
                    {
                        Debug.Log("123클릭한 오브젝트: " + hit.transform.name);
                        CurrentPanel = RPGDungeonPanel;
                        isRPG = true;
                        ShowConfirmation();
                    }
                    else if (hit.transform.tag == "AdventureDungeon")
                    {
                        AdvDungeon touchedDungeon = hit.transform.GetComponent<AdvDungeon>();
                        if (AdventurePanel == null) Debug.LogError("OMG, Adventure Detail Panel is null. \nplease put fucking panel into hierarchy");
                        else if (touchedDungeon == null) Debug.LogError("OMG, Dungeon Script is null. \nDid you idiot forget to put script in prefab?");
                        else AdventurePanel.Init(touchedDungeon);
                    }
                    else if(hit.transform.tag == "Shop")
                    {
                        Debug.Log("Shop Clicked");
                        hit.transform.gameObject.GetComponent<Shopper>().InitCamera(ShopUI);
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
                    Debug.Log("123클릭한 오브젝트: " + hit.transform.name);
                    CurrentPanel = BossDungeonPanel;
                    isRPG = false;
                    ShowConfirmation();
                }   
                else if (hit.transform.tag == "RPGDungeon")
                {
                    Debug.Log("123클릭한 오브젝트: " + hit.transform.name);
                    CurrentPanel = RPGDungeonPanel;
                    isRPG = true;
                    ShowConfirmation();
                }
                else if (hit.transform.tag == "AdventureDungeon")
                {
                    AdvDungeon touchedDungeon = hit.transform.GetComponent<AdvDungeon>();
                    if (AdventurePanel == null) Debug.LogError("OMG, Adventure Detail Panel is null. please put fucking panel into hierarchy");
                    else if (touchedDungeon == null) Debug.LogError("OMG, Dungeon Script is null. Did you idiot forget to put script in prefab?");
                    else AdventurePanel.Init(touchedDungeon);
                }
                else if(hit.transform.tag == "Shop")
                {
                    Debug.Log("Shop Clicked");
                    hit.transform.gameObject.GetComponent<Shopper>().InitCamera(ShopUI);
                }
            }
        }
    }

    void ShowConfirmation()
    {
        
        PanelPosition();
    }

    // 수락 버튼 클릭 시 호출되는 함수
    public void OnAccept()
    {
        audioSource.PlayOneShot(enterSound);
        if (isRPG) SceneManager.LoadScene("RPGDungeon");
        else SceneManager.LoadScene("BossDungeon");
    }

    // 취소 버튼 클릭 시 호출되는 함수
    public void OnCancel()
    {
        audioSource.PlayOneShot(backSound);
        RPGDungeonPanel.SetActive(false);
        BossDungeonPanel.SetActive(false);
        Buttons.SetActive(false);
    }


    void PanelPosition(){

        originalPosition = CurrentPanel.transform.localPosition;
        closePosition = new Vector2(originalPosition.x, -Screen.height);
        CurrentPanel.transform.localPosition = closePosition;
        CurrentPanel.SetActive(true);
        Buttons.SetActive(true);
        OpenAnimation();
    }
    public void OpenAnimation()
    {
        // 패널 애니메이션(Linear로 올라옴)
        CurrentPanel.transform.DOLocalMoveY(originalPosition.y, animationSpeed)
        .SetEase(Ease.Linear);
    }
    public void CloseAnimation()
    {
        // 패널 애니메이션(Linear로 내려감)
        transform.DOLocalMoveY(closePosition.y, animationSpeed)
        .SetEase(Ease.Linear)
        .OnComplete(() => gameObject.SetActive(false));
    }
}
