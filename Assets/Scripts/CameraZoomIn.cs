using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomIn : MonoBehaviour
{
    public float zoomSpeed = 2.0f;            // 줌인 속도
    public float zoomDistance = 5.0f;         // 줌인할 거리
    public Transform cameraTransform;        // 메인 카메라 Transform
    private Vector3 originalPosition;         // 카메라의 원래 위치
    private Quaternion originalRotation;      // 카메라의 원래 회전값
    private bool isZoomingIn = false;         // 줌인 중인지 확인
    private bool isReturning = false;         // 원래 위치로 돌아가는 중인지 확인
    private Transform targetPortal;           // 클릭한 포탈의 위치

    void Start()
    {
        // MainCamera 태그를 가진 카메라 찾기
        GameObject cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        if (cameraObject != null)
        {
            cameraTransform = cameraObject.transform;
            originalPosition = cameraTransform.position;
            originalRotation = cameraTransform.rotation;
        }
        else
        {
            Debug.LogError("카메라를 찾을 수 없습니다.");
        }
    }

    void Update()
    {
        if (cameraTransform == null) return;

        // 포탈 터치 감지
        if (Input.GetMouseButtonDown(0)) // 모바일에서는 터치, PC에서는 마우스 클릭으로 테스트 가능
        {
            Debug.Log("터치먹음");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.transform.CompareTag("RPGDungeon"))
                {
                    Debug.Log("인식함");
                    ZoomInToPortal(hit.transform);
                }
            }
        }
        ZoomIn();

    }

    void ZoomIn()
    {
        Debug.Log(isZoomingIn);
        if (isZoomingIn && targetPortal != null)
        {
            Debug.Log("체크했다");
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPortal.position - targetPortal.forward * zoomDistance, Time.deltaTime * zoomSpeed);
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, Quaternion.LookRotation(targetPortal.position - cameraTransform.position), Time.deltaTime * zoomSpeed);
            Debug.Log("체크했다2");
            if (Vector3.Distance(cameraTransform.position, targetPortal.position - targetPortal.forward * zoomDistance) < 0.1f)
            {
                isZoomingIn = false;
                Debug.Log("줌했다");
            }
        }
        else if (isReturning)
        {
            // 원래 위치로 돌아가는 중
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, originalPosition, Time.deltaTime * zoomSpeed);
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, originalRotation, Time.deltaTime * zoomSpeed);

            // 원래 위치 복귀 완료 확인
            if (Vector3.Distance(cameraTransform.position, originalPosition) < 0.1f)
            {
                isReturning = false;
            }
        }
    }
    // 포탈을 터치했을 때 호출되는 함수
    private void ZoomInToPortal(Transform portalTransform)
    {
        targetPortal = portalTransform;
        isZoomingIn = true;
        isReturning = false;
    }

    // "아니오" 버튼을 누르면 호출되는 함수
    public void ReturnToOriginalPosition()
    {
        isReturning = true;
        isZoomingIn = false;
    }
}
