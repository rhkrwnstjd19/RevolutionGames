using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class ScanManager : MonoBehaviour
{
    public static ScanManager Instance;
    public SpawnManager spawnManager;
    public Material highlightedMaterial; // ������ ����� ���̶���Ʈ�ϴ� ���׸���
    public Material defaultMaterial;     // �⺻ ��� ���׸���
    [HideInInspector]
    public ARPlane selectedPlane;

    private ARPlaneManager planeManager;
    private ARPlane highlightedPlane;
    public bool isTracking = true;
    

    void Start()
    {
        planeManager = FindObjectOfType<ARPlaneManager>();
        spawnManager.gameObject.SetActive(!isTracking);
    }

    void Update()
    {
        //ī�޶󿡼� ���̸� ��� ���� �������� ����� ���̶���Ʈ �ϴ� �ڵ�
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            ARPlane planeHit = hit.transform.GetComponent<ARPlane>();
            if (planeHit != null && planeHit != highlightedPlane)
            {
                // ���̶���Ʈ �� ����� �������
                if (highlightedPlane != null)
                {
                    SetPlaneMaterial(highlightedPlane, defaultMaterial);
                }

                // ���ο� ��� ���̶���Ʈ
                highlightedPlane = planeHit;
                SetPlaneMaterial(highlightedPlane, highlightedMaterial);
            }
        }
    }
    public void ConfirmSelection()
    {
        if (highlightedPlane != null)
        {
            //������ ����� ����. ��ư�� �Ҵ�
            isTracking = false;
            spawnManager.gameObject.SetActive(!isTracking);
            selectedPlane = highlightedPlane;
            DisableOtherPlanes();
            this.gameObject.SetActive(false);
        }
    }
    void DisableOtherPlanes()
    {
        //���õ� ��� �̿��� ����, ��� ���� ��� ��Ȱ��ȭ
        planeManager.enabled = false;
        foreach (var plane in planeManager.trackables)
        {
            if (plane != selectedPlane)
            {
                plane.gameObject.SetActive(false);
            }
        }
    }
    private void SetPlaneMaterial(ARPlane plane, Material material)
    {
        var renderer = plane.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            renderer.material = material;
        }
    }
}