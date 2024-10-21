using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float resetTime = 3.0f;  // 볼 리셋 시간 (3초)
    public float captureRate = 1.0f;  // 포획 확률
    public TMP_Text result;  // 결과 텍스트
    public GameObject effect;  // 포획 이펙트
    public MonsterManager monsterManager;  // MonsterManager 참조
    public int targetMonsterIndex;  // 포획하려는 몬스터의 인덱스


    /**/
    private Rigidbody selectedRigidbody;
    private Camera mainCamera;
    private Vector3 offset;
    private float zCoord;
     // 던지기 속도 조절을 위한 변수
    private Vector3 lastMousePosition;
    private float velocity;

    // 던지기 적용을 위한 변수
    public float throwForceMultiplier = 5f;

    /**/

    Rigidbody rb;
    bool isReady = true;
    Vector2 startPos;

    void Start()
    {
        result.text = "";
        rb = GetComponent<Rigidbody>();
        //rb.isKinematic = true;  // 초기 상태에서는 움직이지 않음
        mainCamera = Camera.main;
    }

    // void Update()
    // {
    //     if (!isReady) return;

    //     //SetBallPosition(Camera.main.transform);

    //     if (Input.touchCount > 0 && isReady)
    //     {
    //         Touch touch = Input.GetTouch(0);
    //         if (touch.phase == TouchPhase.Began)
    //         {
    //             startPos = touch.position;
    //         }
    //         else if (touch.phase == TouchPhase.Ended)
    //         {
    //             float dragDistance = touch.position.y - startPos.y;
    //             Vector3 throwAngle = (Camera.main.transform.forward + Camera.main.transform.up).normalized;

    //             rb.isKinematic = false;
    //             isReady = false;

    //             rb.AddForce(throwAngle * dragDistance * 0.005f, ForceMode.VelocityChange);
    //             Invoke("ResetBall", resetTime);
    //         }
    //     }
    // }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // 특정 레이어 (예: Draggable) 만 감지하려면 LayerMask 사용
            // int layerMask = LayerMask.GetMask("Draggable");
            // if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))

            if (Physics.Raycast(ray, out hit))
            {
                // 선택된 오브젝트의 Rigidbody 가져오기
                selectedRigidbody = hit.rigidbody;
                if (selectedRigidbody != null)
                {
                    // 드래그 시작 시의 Z 좌표 저장
                    zCoord = mainCamera.WorldToScreenPoint(selectedRigidbody.position).z;

                    // 드래그 시작 시의 오프셋 계산
                    offset = selectedRigidbody.position - GetMouseWorldPosition();

                    // Rigidbody의 중력을 일시적으로 비활성화하여 드래그 중에 떨어지지 않도록 함
                    selectedRigidbody.useGravity = false;
                    
                }
            }
        }

        // 마우스 버튼을 누르고 있을 때
        if (Input.GetMouseButton(0) && selectedRigidbody != null)
        {
            rb.isKinematic = true;
            Vector3 currentMousePosition = GetMouseWorldPosition();
            selectedRigidbody.position = currentMousePosition + offset;

            // 던지기 속도 계산을 위해 현재 마우스 위치 저장
            velocity = lastMousePosition.y - currentMousePosition.y;
            lastMousePosition = currentMousePosition;
        }

        // 마우스 버튼을 뗄 때
        if (Input.GetMouseButtonUp(0) && selectedRigidbody != null)
        {
            // Rigidbody의 중력 다시 활성화
            selectedRigidbody.useGravity = true;
            Vector3 throwAngle = (Camera.main.transform.forward + Camera.main.transform.up).normalized;
             rb.isKinematic = false;

            // 던지기 힘 적용
            selectedRigidbody.AddForce(velocity * throwAngle * 0.05f, ForceMode.VelocityChange);

            // 선택된 Rigidbody 초기화
            selectedRigidbody = null;
        }
    }
    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoord; // 드래그할 오브젝트의 깊이 유지
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }

    void SetBallPosition(Transform anchor)
    {
        Vector3 offset = anchor.forward * 0.5f + anchor.up * 0.2f;
        transform.position = anchor.position + offset;
    }

    void ResetBall()
    {
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        isReady = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(MagneticBall(collision.gameObject));
    }

    void Capture(GameObject gameObject)
    {
        if (!isReady) return;

        float draw = Random.Range(0, 1f);
        if (draw <= captureRate)
        {
            result.text = "Successful capture!";
            monsterManager.CaptureMonster(targetMonsterIndex);  // 몬스터 포획 및 도감 업데이트
        }
        else
        {
            result.text = "You failed and it ran away...";
        }

        Instantiate(effect, gameObject.transform.position, Camera.main.transform.rotation);
        Destroy(gameObject.gameObject);  // 충돌한 몬스터 삭제
        gameObject.SetActive(false);   // 몬스터볼 비활성
    }

    IEnumerator MagneticBall(GameObject target)
    {
        float duration = 1.0f; // 애니메이션 지속 시간(초)
        float elapsedTime = 0f;

        Vector3 startingPos = transform.position; // 현재 오브젝트의 시작 위치
        Vector3 targetPos = target.transform.position; // 목표 오브젝트의 위치

        while (elapsedTime < duration)
        {
            // 선형 보간을 사용하여 오브젝트를 이동
            transform.position = Vector3.Lerp(startingPos, targetPos, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // 정확한 목표 위치로 설정하여 이동 완료
        transform.position = targetPos;
        Capture(target);
    }

}
