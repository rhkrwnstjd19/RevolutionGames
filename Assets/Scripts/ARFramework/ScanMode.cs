using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class ScanMode : MonoBehaviour
{   
    public Material highlightedMaterial; // ������ ����� ���̶���Ʈ�ϴ� ���׸���
    public Material defaultMaterial;     // �⺻ ��� ���׸���
    [HideInInspector]
    public static ARPlane selectedPlane;

    public ARPlaneManager planeManager;
    private ARPlane highlightedPlane;
    public bool isTracking = true;
    private void OnEnable()
    {
        UIController.ShowUI("Scan");
    }

    // Update is called once per frame
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
            selectedPlane = highlightedPlane;
            DisableOtherPlanes();
            InteractionController.EnableMode("Main");
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
