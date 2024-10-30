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
    public GameObject Swipe;
    public GameObject Result;



    private void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();

        ShowMessage("���� ��...");
    }

    private void Update()
    {

    }

    public void Spawn()
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

                Swipe.SetActive(true);

                Invoke("DeactivateSwipe", 6f);

            }
            else
            {
                ShowMessage("���� �ν� ����.");
            }
        }
    }
    private void ShowMessage(string message)
    {
        if (messagePanel != null)
        {
            messagePanel.text = message;
        }
    }

    private void DeactivateSwipe()
    {
        Swipe.SetActive(false);

        Invoke("EndStage", 6f);

    }
    private void EndStage()
    {
        Result.SetActive(true);
    }

    public void SetSelectedMonster(int index)
    {
        selectedMonsterIndex = index;
    }
}