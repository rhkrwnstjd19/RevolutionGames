using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float resetTime = 3.0f; // 볼 리셋 시간 (3초)
    public float captureRate = 1.0f;
    public TMP_Text result;
    
    Rigidbody rb;
    bool isReady = true;
    Vector2 startPos;

    // Start is called before the first frame update
    void Start()
    {
        result.text = "";

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isReady)
            return; // 사용자가 공을 생성한 후에만 던질 수 있음. 던질 때까지 기다림.

        SetBallPosition(Camera.main.transform); // 볼 위치를 화면 중앙에 위치시킴.

        if (Input.touchCount > 0 && isReady) // 사용자의 손가락 터치가 감지되면, 터치 위치에 따라 공을 던질 준비를 함.
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) // 터치가 시작되었을 때
            {
                startPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) // 터치가 끝났을 때
            {
                float dragDistance = touch.position.y - startPos.y;

                // 공의 방향과 힘을 계산하여 던짐.
                Vector3 throwAngle = (Camera.main.transform.forward + Camera.main.transform.up).normalized;

                rb.isKinematic = false;
                isReady = false;

                // 공을 던짐 (방향 및 드래그 거리)
                rb.AddForce(throwAngle * dragDistance * 0.005f, ForceMode.VelocityChange);

                Invoke("ResetBall", resetTime); // resetTime 후에 공을 리셋함.
            }
        }
    }

    void SetBallPosition(Transform anchor) // 공을 카메라 앞에 위치시키는 함수
    {
        Vector3 offset = anchor.forward * 0.5f + anchor.up * 0.2f;
        transform.position = anchor.position + offset;
    }

    void ResetBall() // 공을 리셋하는 함수
    {
        rb.isKinematic = true; // 물리 엔진 활성화
        rb.velocity = Vector3.zero; // 공의 속도를 0으로 리셋
        isReady = true; // 공을 던질 준비 완료
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isReady)
            return;

        float draw = Random.Range(0, 1f); // 0에서 1까지의 무작위 값 생성
        if (draw <= captureRate)
        {
            result.text = "Successful capture!";
        }
        else
        {
            result.text = "You failed and it ran away...";
        }

        Destroy(collision.gameObject); // 충돌한 객체(몬스터) 삭제
        gameObject.SetActive(false);   // 몬스터볼 비활성화
    }
}
