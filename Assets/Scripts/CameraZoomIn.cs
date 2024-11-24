// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class CameraZoomIn : MonoBehaviour
// {
//     public float zoomSpeed = 2.0f;            // ���� �ӵ�
//     public float zoomDistance = 5.0f;         // ������ �Ÿ�
//     public Transform cameraTransform;        // ���� ī�޶� Transform
//     private Vector3 originalPosition;         // ī�޶��� ���� ��ġ
//     private Quaternion originalRotation;      // ī�޶��� ���� ȸ����
//     private bool isZoomingIn = false;         // ���� ������ Ȯ��
//     private bool isReturning = false;         // ���� ��ġ�� ���ư��� ������ Ȯ��
//     private Transform targetPortal;           // Ŭ���� ��Ż�� ��ġ

//     void Start()
//     {
//         // MainCamera �±׸� ���� ī�޶� ã��
//         GameObject cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
//         if (cameraObject != null)
//         {
//             cameraTransform = cameraObject.transform;
//             originalPosition = cameraTransform.position;
//             originalRotation = cameraTransform.rotation;
//         }
//         else
//         {
//             Debug.LogError("ī�޶� ã�� �� �����ϴ�.");
//         }
//     }

//     void Update()
//     {
//         if (cameraTransform == null) return;

//         // ��Ż ��ġ ����
//         if (Input.GetMouseButtonDown(0)) // ����Ͽ����� ��ġ, PC������ ���콺 Ŭ������ �׽�Ʈ ����
//         {
//             Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
//             if (Physics.Raycast(ray, out RaycastHit hit))
//             {
//                 if (hit.transform.CompareTag("RPGDungeon"))
//                 {
//                     Debug.Log("�ν���");
//                     ZoomInToPortal(hit.transform);
//                 }
//             }
//         }
//         ZoomIn();

//     }

//     void ZoomIn()
//     {
//         // Debug.Log(isZoomingIn);
//         if (isZoomingIn && targetPortal != null)
//         {
//             cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPortal.position - targetPortal.forward * zoomDistance, Time.deltaTime * zoomSpeed);
//             cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, Quaternion.LookRotation(targetPortal.position - cameraTransform.position), Time.deltaTime * zoomSpeed);
//             if (Vector3.Distance(cameraTransform.position, targetPortal.position - targetPortal.forward * zoomDistance) < 0.1f)
//             {
//                 isZoomingIn = false;
//             }
//         }
//         else if (isReturning)
//         {
//             // ���� ��ġ�� ���ư��� ��
//             cameraTransform.position = Vector3.Lerp(cameraTransform.position, originalPosition, Time.deltaTime * zoomSpeed);
//             cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, originalRotation, Time.deltaTime * zoomSpeed);

//             // ���� ��ġ ���� �Ϸ� Ȯ��
//             if (Vector3.Distance(cameraTransform.position, originalPosition) < 0.1f)
//             {
//                 isReturning = false;
//             }
//         }
//     }
//     // ��Ż�� ��ġ���� �� ȣ��Ǵ� �Լ�
//     private void ZoomInToPortal(Transform portalTransform)
//     {
//         targetPortal = portalTransform;
//         isZoomingIn = true;
//         isReturning = false;
//     }

//     // "�ƴϿ�" ��ư�� ������ ȣ��Ǵ� �Լ�
//     public void ReturnToOriginalPosition()
//     {
//         isReturning = true;
//         isZoomingIn = false;
//     }
// }
