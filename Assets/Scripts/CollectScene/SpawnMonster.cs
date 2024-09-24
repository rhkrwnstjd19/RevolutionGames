using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class SpawnMonster : MonoBehaviour
{
    public GameObject Cube; // ť�� ������Ʈ
    private ARRaycastManager raycastManager; // ����ĳ��Ʈ �Ŵ���

    private List<ARRaycastHit> hit = new List<ARRaycastHit>(); // ����ĳ��Ʈ ��Ʈ ���

    private bool objectSpawn = false; // ������Ʈ�� �����Ǿ����� Ȯ��

    public Text messagePanel;
    public GameObject GotoMainButton;

    private void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();

        ShowMessage("���� ��...");
    }

    private void Update()
    {
        Spawn();
    }

    void Spawn()
    {
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
                    Instantiate(Cube, hitPose.position, hitPose.rotation);
                    objectSpawn = true;

                    ShowMessage("ť�� ���� �Ϸ�!");

                    Invoke("EndStage", 2f);
                }
                else
                {
                    // ����ĳ��Ʈ ���� ��
                    ShowMessage("���� �ν� ����.");
                }
            }
        }
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
}