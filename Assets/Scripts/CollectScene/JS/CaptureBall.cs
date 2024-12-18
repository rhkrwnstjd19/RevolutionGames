using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Math = System.Math;
using UnityEngine.UI;

public class CaptureBall : MonoBehaviour
{
    public float resetTime = 3.0f;
    public float captureRate = 1.0f;
    public GameObject effect;
    public Transform ballSpawnPoint; // 공이 나타날 위치
    private CaptureManager captureManager;
    Rigidbody rb;
    bool isReady = true;
    Vector3 startPos;
    Camera mainCamera;

    public AudioSource audioSource;  // 오디오 소스 참조
    public AudioClip throwSound;     // 공 던질 때 사운드
    public AudioClip captureSound;   // 수집 성공 사운드

    void Awake()
    {

        mainCamera = Camera.main;
        captureManager = FindObjectOfType<CaptureManager>();
        audioSource = GetComponent<AudioSource>();
        InitialBall();
    }
    /// <summary>
    /// 볼 생성 코드
    /// </summary>
    void InitialBall()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;

        SetBallPosition();
    }
    void SetColliderVeryBig(){
        // Collider 크기를 크게 만들어서 충돌 범위를 넓힘
        BoxCollider collider = gameObject.GetComponent<BoxCollider>();
        collider.size = new Vector3(5, 5, 5);
    }
    void Update()
    {
        if (!isReady) // 사용자가 공을 발사한 공이 날아가고 있는 도중에는 공의 위치를 카메라 앞에 고정시키지 않음
        {
            return;
        }

        // if (Input.touchCount > 0 && isReady) // 한 개 이상의 터치가 있을 때
        // {
        //     Touch touch = Input.GetTouch(0);

        //     if (touch.phase == TouchPhase.Began) // 터치가 시작될 때
        //     {
        //         startPos = touch.position;
        //     }
        //     else if (touch.phase == TouchPhase.Ended) // 터치가 끝날 때
        //     {
        //         float dragDistance = touch.position.y - startPos.y;

        //         // 사용자 터치와 드래그(당김) 상태 기반의 힘을 가한다.
        //         Vector3 throwAngle = (mainCamera.transform.forward +mainCamera.transform.up).normalized;

        //         rb.isKinematic = false; // 물리 적용 활성화
        //         isReady = false;

        //         rb.AddForce(throwAngle * dragDistance * 0.005f, ForceMode.VelocityChange);
                
        //         // 던질 때 사운드 재생
        //         audioSource.PlayOneShot(throwSound);
        //         captureManager.ThrowBall();
        //         if(captureManager.GetBallCount() >0)Invoke("ResetBall", resetTime); // resetTime 뒤에 공의 위치 및 동작을 초기화한다.
        //     }
        // }
        if (Input.touchCount > 0 && isReady) // 한 개 이상의 터치가 있을 때
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) // 터치가 시작될 때
            {
            startPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) // 터치가 끝날 때
            {
            float dragDistanceY = touch.position.y - startPos.y;
            float dragDistanceX = touch.position.x - startPos.x;

            if (dragDistanceY >= 5)
            {
                Vector3 throwAngle = mainCamera.transform.forward + mainCamera.transform.up;

                if (Math.Abs(dragDistanceX) > 300){
                    Debug.Log($"dragDistanceX : {dragDistanceX}");
                    throwAngle +=new Vector3(dragDistanceX,0,0).normalized;
                }

                throwAngle = throwAngle.normalized;

                rb.isKinematic = false; // 물리 적용 활성화
                isReady = false;

                rb.AddForce(throwAngle * dragDistanceY * 0.001f, ForceMode.VelocityChange);

                // 던질 때 사운드 재생
                audioSource.PlayOneShot(throwSound);

                captureManager.ThrowBall();
                if (captureManager.GetBallCount() > 0) Invoke("ResetBall", resetTime); // resetTime 뒤에 공의 위치 및 동작을 초기화한다.
            }
            else Debug.Log("Not enough drag distance");
            }
        }
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭 시
        {
            startPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0)) // 마우스 왼쪽 버튼을 뗄 때
        {
            float dragDistanceY = Input.mousePosition.y - startPos.y;

            float dragDistanceX =  Input.mousePosition.x-startPos.x;
            //dragdistanceX가 양수 일때는 오른쪽으로 드래그, 음수일때는 왼쪽으로 드래그
            if (dragDistanceY >= 5)
            {
                Vector3 throwAngle = mainCamera.transform.forward +mainCamera.transform.up;

                if(Math.Abs(dragDistanceX) > 300){
                    Debug.Log($"dragDistanceX : {dragDistanceX}");
                    throwAngle +=new Vector3(dragDistanceX,0,0).normalized;
                }

                throwAngle = throwAngle.normalized;
                // 사용자 마우스 클릭과 드래그(당김) 상태 기반의 힘을 가한다.

                rb.isKinematic = false; // 물리 적용 활성화
                isReady = false;

                rb.AddForce(throwAngle * dragDistanceY * 0.001f, ForceMode.VelocityChange);
                
                // 던질 때 사운드 재생
                audioSource.PlayOneShot(throwSound);

                captureManager.ThrowBall();
                if(captureManager.GetBallCount() >0)Invoke("ResetBall", resetTime); // resetTime 뒤에 공의 위치 및 동작을 초기화한다.
            }
            else Debug.Log("Not enough drag distance");

        }
    if (Input.GetMouseButton(0) && isReady) // 마우스 왼쪽 버튼을 누르고 있을 때
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = mainCamera.nearClipPlane + 0.5f; // 카메라 앞에 위치하도록 z값 설정
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        transform.position = worldPosition;
    }
    if (Input.touchCount > 0 && isReady) // 한 개 이상의 터치가 있을 때
    {
        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Moved) // 터치가 이동 중일 때
        {
            Vector3 touchPosition = touch.position;
            touchPosition.z = mainCamera.nearClipPlane + 0.5f; // 카메라 앞에 위치하도록 z값 설정
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);
            transform.position = worldPosition;
        }
    }
    }

    void SetBallPosition()
    {
        Debug.Log("SetBallPosition");
        transform.position = mainCamera.transform.position + mainCamera.transform.forward * 0.5f + mainCamera.transform.up * -0.2f;
    }

    void ResetBall()
    {
        rb.isKinematic = true; // 물리 비활성화
        transform.position = mainCamera.transform.position + mainCamera.transform.forward * 0.5f + mainCamera.transform.up * -0.2f;
        rb.velocity = Vector3.zero; // 공의 속도 초기화
        isReady = true; // 공 준비
        gameObject.SetActive(true); // 공 다시 활성화
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"collsion : {other.gameObject.tag}");

        // 충돌한 오브젝트가 "Enemy" 태그를 가지고 있을 때만 처리
        if (other.gameObject.CompareTag("Pet"))
        {
            Debug.Log($"compared tag : {other.gameObject.name}");
            float draw = Random.Range(0, 1.0f);
            if (draw <= captureRate)
            {
                captureManager.CapturePet(other.gameObject);
                captureManager.SetResultPanel();

                // 몬스터 잡을 때 사운드 재생
                audioSource.PlayOneShot(captureSound);
                Instantiate(effect, other.transform.position, Camera.main.transform.rotation);
                Destroy(other.gameObject);
            }
            else
            {
                Debug.Log("Fail to capture");
                StartCoroutine(captureManager.CaptureFail());
            }

            Instantiate(effect, other.transform.position, Camera.main.transform.rotation);


            gameObject.SetActive(false);
            Invoke("ResetBall", 3.0f); // 3초 뒤에 공을 다시 나타나게 한다.
        }

    }
}

    