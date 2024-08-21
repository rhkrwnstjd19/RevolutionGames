using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class ScanMode : MonoBehaviour
{   
    public Material highlightedMaterial; // 감지된 평면을 하이라이트하는 머테리얼
    public Material defaultMaterial;     // 기본 평면 머테리얼
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
        //카메라에서 레이를 쏘아 현재 선택중인 평면을 하이라이트 하는 코드
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            ARPlane planeHit = hit.transform.GetComponent<ARPlane>();
            if (planeHit != null && planeHit != highlightedPlane)
            {
                // 하이라이트 된 평면을 원래대로
                if (highlightedPlane != null)
                {
                    SetPlaneMaterial(highlightedPlane, defaultMaterial);
                }

                // 새로운 평면 하이라이트
                highlightedPlane = planeHit;
                SetPlaneMaterial(highlightedPlane, highlightedMaterial);
            }
        }
    }
    
    public void ConfirmSelection()
    {
        if (highlightedPlane != null)
        {
            //선택한 평면을 저장. 버튼에 할당
            isTracking = false;
            selectedPlane = highlightedPlane;
            DisableOtherPlanes();
            InteractionController.EnableMode("Main");
        }
    }
    void DisableOtherPlanes()
    {
        //선택된 평면 이외의 평면들, 평면 감지 기능 비활성화
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
