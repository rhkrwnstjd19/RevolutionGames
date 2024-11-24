using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BCtest : MonoBehaviour
{
    [System.Serializable]
    public class BallInfo
    {
        public string ballName;
        public GameObject ballPrefab;
        public int ballCount; // 소모품인 볼의 갯수 (Lv 2~5)
        public TMP_Text countText;
    }

    public BallInfo[] balls; // 볼 종류 정보 (Lv1~5)
    private int currentBallIndex = 0; // 현재 선택된 볼의 인덱스
    public GameObject ballPanel; // BallPanel 오브젝트

    public float resetTime = 3.0f;
    public float captureRate = 1.0f;
    public TMP_Text result;
    public GameObject effect;
    public PokedexManager pokedexManager;
    public Transform ballSpawnPoint; // 공이 나타날 위치

    private GameObject currentBall; // 현재 활성화된 공 오브젝트

    Rigidbody rb;
    bool isReady = true;
    Vector3 startPos;
    Camera mainCamera;

    void Awake()
    {
        // 각 볼 버튼에 클릭 이벤트 추가
        for (int i = 0; i < balls.Length; i++)
        {
            int index = i; // 버튼 클릭 시 사용하기 위한 로컬 변수
            Button ballButton = ballPanel.transform.GetChild(i).GetComponent<Button>();
            //ballButton.onClick.AddListener(() => SelectBall(index));
        }

        // 게임 시작 시 레벨 1의 볼 생성
        InitialBall();
        mainCamera = Camera.main;
    }
    /// <summary>
    /// 볼 생성 코드
    /// </summary>
    void InitialBall()
    {
        // 레벨 1의 볼을 화면 중앙 하단부에 생성
        currentBall = this.gameObject;
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
#if UNITY_EDITOR
    SetColliderVeryBig();
#endif
        SetBallPosition();
    }
    void SetColliderVeryBig(){
        // Collider 크기를 크게 만들어서 충돌 범위를 넓힘
        SphereCollider collider = currentBall.GetComponent<SphereCollider>();
        collider.radius = 30f;
    }
    void Update()
    {
        if (!isReady) // 사용자가 공을 발사한 공이 날아가고 있는 도중에는 공의 위치를 카메라 앞에 고정시키지 않음
        {
            return;
        }

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
                Vector3 throwAngle = (mainCamera.transform.forward +mainCamera.transform.up).normalized;

                rb.isKinematic = false; // 물리 적용 활성화
                isReady = false;

                rb.AddForce(throwAngle * dragDistance * 0.005f, ForceMode.VelocityChange);

                Invoke("ResetBall", resetTime); // resetTime 뒤에 공의 위치 및 동작을 초기화한다.
            }
        }
        if (Input.GetMouseButtonDown(0)) // 마우스 왼쪽 버튼 클릭 시
        {
            startPos = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0)) // 마우스 왼쪽 버튼을 뗄 때
        {
            float dragDistanceY = Input.mousePosition.y - startPos.y;

            float dragDistanceX = Input.mousePosition.x - startPos.x;
            //dragdistanceX가 양수 일때는 오른쪽으로 드래그, 음수일때는 왼쪽으로 드래그
            if (dragDistanceY >= 5)
            {
                Vector3 throwAngle = mainCamera.transform.forward +mainCamera.transform.up;

                if(dragDistanceX > 3){
                    throwAngle +=new Vector3(dragDistanceX,0,0).normalized;
                }

                throwAngle = throwAngle.normalized;
                // 사용자 마우스 클릭과 드래그(당김) 상태 기반의 힘을 가한다.

                rb.isKinematic = false; // 물리 적용 활성화
                isReady = false;

                rb.AddForce(throwAngle * dragDistanceY * 0.001f, ForceMode.VelocityChange);

                Invoke("ResetBall", resetTime); // resetTime 뒤에 공의 위치 및 동작을 초기화한다.
            }
            else Debug.Log("Not enough drag distance");

        }
    }

    void SetBallPosition()
    {
        Debug.Log("SetBallPosition");
        transform.position = new Vector3(0, -0.2f, 0.75f);
    }

    void ResetBall()
    {
        rb.isKinematic = true; // 물리 비활성화
        currentBall.transform.position = new Vector3(0, -0.2f, 0.75f); // 공을 초기 위치로 이동
        rb.velocity = Vector3.zero; // 공의 속도 초기화
        result.text = ""; // 결과 텍스트 초기화
        isReady = true; // 공 준비
        currentBall.SetActive(true); // 공 다시 활성화
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"collsion : {other.gameObject.tag}");

        // 충돌한 오브젝트가 "Enemy" 태그를 가지고 있을 때만 처리
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log($"compared tag : {other.gameObject.name}");
            float draw = Random.Range(0, 1.0f);
            if (draw <= captureRate)
            {
                result.text = "Catch!!";
                int a = GetMonsterIndex(other.gameObject.name);
                Debug.Log($"monster index : {a}");
                pokedexManager.CaptureMonster(a);
            }
            else
            {
                result.text = "fail..";
            }

            Instantiate(effect, other.transform.position, Camera.main.transform.rotation);

            Destroy(other.gameObject);
            currentBall.SetActive(false);
            Invoke("ResetBall", 3.0f); // 3초 뒤에 공을 다시 나타나게 한다.
        }
    }

    int GetMonsterIndex(string monsterName)
    {
        Debug.LogWarning(monsterName);
        // 몬스터 이름을 기반으로 인덱스를 결정 (추후 확장 또는 변경)
        switch (monsterName)
        {
            case "Monster1": return 0;
            case "Monster2": return 1;
            case "Monster3": return 2;
            case "Monster4": return 3;
            case "Monster5": return 4;
            default: return -1;
        }
    }
}
