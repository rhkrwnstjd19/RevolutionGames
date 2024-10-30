using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BCtest : MonoBehaviour
{
    public float resetTime = 3.0f;
    public float captureRate = 1.0f;
    public TMP_Text result;
    public GameObject effect;

    Rigidbody rb;
    bool isReady = true;
    Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isReady) // 사용자가 공을 발사한 공이 날아가고 있는 도중에는 공의 위치를 카메라 앞에 고정시키지 않음
        {
            return;
        }

        SetBallPosition(Camera.main.transform); // 볼을 카메라 전방 위치에 고정한다.

        if (Input.touchCount > 0 && isReady) // 한 개 이상의 터치가 있을 때
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began) // 터치가 시작될 때
            {
                startPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) // 터치가 끝날 때
            {
                float dragDistance = touch.position.y - startPos.y;

                // 사용자 터치와 드래그(당김) 상태 기반의 힘을 가한다.
                Vector3 throwAngle = (Camera.main.transform.forward + Camera.main.transform.up).normalized;

                rb.isKinematic = false; // 물리 적용 활성화
                isReady = false;

                rb.AddForce(throwAngle * dragDistance * 0.005f, ForceMode.VelocityChange);

                Invoke("ResetBall", resetTime); // resetTime 뒤에 공의 위치 및 동작을 초기화한다.
            }
        }
    }

    void SetBallPosition(Transform anchor)
    {
        Vector3 offset = anchor.forward * 0.5f + anchor.up * -0.2f; // 카메라 위치에서 일정한 거리만큼 오프셋 설정.
        transform.position = anchor.position + offset; // 카메라 위치에서 일정한 거리만큼 공을 위치.
    }

    void ResetBall()
    {
        rb.isKinematic = true; // 물리 비활성화
        rb.velocity = Vector3.zero; // 공의 속도 초기화
        isReady = true; // 공 준비
    }
    void OnCollisionEnter(Collision collision)
    {
        if (isReady)
            return;

        // 충돌한 오브젝트가 "Enemy" 태그를 가지고 있을 때만 처리
        if (collision.gameObject.CompareTag("Enemy"))
        {
            float draw = Random.Range(0, 1.0f);
            if (draw <= captureRate)
            {
                result.text = "Catch!!";
            }
            else
            {
                result.text = "fail..";
            }

            Instantiate(effect, collision.transform.position, Camera.main.transform.rotation);

            Destroy(collision.gameObject);
            gameObject.SetActive(false);
        }

    }
}
