using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class SpawnMonster : MonoBehaviour
{
    public GameObject[] monsterPrefab; // ���� �迭
    private ARRaycastManager raycastManager; // ����ĳ��Ʈ �Ŵ���

    private List<ARRaycastHit> hit = new List<ARRaycastHit>(); // ����ĳ��Ʈ ��Ʈ ���

    private bool objectSpawn = false; // ������Ʈ�� �����Ǿ����� Ȯ��
    public int selectedMonsterIndex = -1;   //monster select

    public Text messagePanel;
    public GameObject GotoMainButton;

    private void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();

        ShowMessage("���� ��...");
    }

    private void Update()
    {
        if (selectedMonsterIndex > -1)
        {
            Spawn();
        }
    }

    void Spawn()
    {
        if (!objectSpawn)
        {
            GameObject selectedMonster = monsterPrefab[selectedMonsterIndex];

            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            Ray ray = Camera.main.ScreenPointToRay(screenCenter);

            if (raycastManager.Raycast(ray, hit, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hit[0].pose;

                ShowMessage("Hit Position: " + hitPose.position);

                Instantiate(selectedMonster, hitPose.position, hitPose.rotation);
                objectSpawn = true;

                //ShowMessage("���� ���� �Ϸ�!");

                Invoke("EndStage", 2f);
            }
            else
            {
                ShowMessage("���� �ν� ����.");
            }
        }



        /*
        // ��ġ �Է� Ȯ��
        if (Input.touchCount > 0 && !objectSpawn)
        {
            Touch touch = Input.GetTouch(0);

            // ��ġ�� ���۵� �� ����ĳ��Ʈ ����
            if (touch.phase == TouchPhase.Began)
            {
                // ��ġ�� ��ġ�� �������� ����ĳ��Ʈ
                if (raycastManager.Raycast(touch.position, hit, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hit[0].pose;


                }
                else
                {
                    // ����ĳ��Ʈ ���� ��
                    ShowMessage("���� �ν� ����.");
                }
            }
        }
        */
    }
    // �޽��� �гο� �ؽ�Ʈ ǥ��
    private void ShowMessage(string message)
    {
        if (messagePanel != null)
        {
            messagePanel.text = message;
        }
    }

    private void EndStage()
    {
        GotoMainButton.SetActive(true);
    }

    public void SetSelectedMonster(int index)
    {
        selectedMonsterIndex = index;    
    }
}