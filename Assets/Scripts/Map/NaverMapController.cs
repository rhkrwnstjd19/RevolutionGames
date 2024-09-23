using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Networking;

public class NaverMapController : MonoBehaviour
{
    // 지도 이미지를 표시할 RawImage 컴포넌트
    public RawImage mapImage;
    // 주소 입력 필드와 검색 버튼
    public TMP_InputField addressInput;
    public Button searchButton;
    // 초기 줌 레벨
    public int zoom = 15;
    // 초기 지도 중심 좌표 (서울)
    private float latitude = 37.5665f;
    private float longitude = 126.9780f;

    // 드래그 시작 위치와 마지막 드래그 위치
    private Vector2 pointerDownPosition;
    private Vector2 lastDragPosition;
    // 드래그 동안의 총 이동 거리
    private Vector2 dragDeltaTotal;
    // 드래그 중인지 여부를 나타내는 플래그
    private bool isDragging = false;
    // 클릭으로 간주할 최대 이동 거리
    private const float clickThreshold = 10f;
    // 드래그 감도 조절
    public float dragFactor = 0.00001f;

    void Start()
    {
        // 이벤트 트리거 설정
        SetupEventTrigger();
        // 초기 지도를 불러옵니다.
        StartCoroutine(GetMapTile(latitude, longitude, zoom));

        // 검색 버튼 클릭 이벤트 추가
        searchButton.onClick.AddListener(OnSearchButtonClick);
    }

    // 이벤트 트리거를 설정하는 메서드
    private void SetupEventTrigger()
    {
        // RawImage에 EventTrigger 컴포넌트를 추가합니다.
        EventTrigger trigger = mapImage.gameObject.AddComponent<EventTrigger>();

        // PointerDown 이벤트 트리거 설정
        EventTrigger.Entry entryPointerDown = new EventTrigger.Entry();
        entryPointerDown.eventID = EventTriggerType.PointerDown;
        entryPointerDown.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
        trigger.triggers.Add(entryPointerDown);

        // PointerUp 이벤트 트리거 설정
        EventTrigger.Entry entryPointerUp = new EventTrigger.Entry();
        entryPointerUp.eventID = EventTriggerType.PointerUp;
        entryPointerUp.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
        trigger.triggers.Add(entryPointerUp);

        // Drag 이벤트 트리거 설정
        EventTrigger.Entry entryDrag = new EventTrigger.Entry();
        entryDrag.eventID = EventTriggerType.Drag;
        entryDrag.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
        trigger.triggers.Add(entryDrag);
    }

    // 포인터가 눌러졌을 때 호출되는 메서드
    private void OnPointerDown(PointerEventData eventData)
    {
        // 포인터 다운 위치를 기록합니다.
        pointerDownPosition = eventData.position;
        lastDragPosition = eventData.position;
        dragDeltaTotal = Vector2.zero;
        isDragging = false;
    }

    // 포인터가 떼어졌을 때 호출되는 메서드
    private void OnPointerUp(PointerEventData eventData)
    {
        // 드래그가 발생하지 않았고 클릭으로 간주할 최대 이동 거리 내에 있다면 클릭으로 처리합니다.
        if (!isDragging && Vector2.Distance(pointerDownPosition, eventData.position) < clickThreshold)
        {
            // 클릭 이벤트 처리 (필요시 추가)
        }
    }

    // 드래그 중일 때 호출되는 메서드
    private void OnDrag(PointerEventData eventData)
    {
        // 현재 드래그 위치를 기록하고 이동 거리를 계산합니다.
        Vector2 currentDragPosition = eventData.position;
        Vector2 dragDelta = currentDragPosition - lastDragPosition;
        lastDragPosition = currentDragPosition;

        // 누적된 드래그 이동 거리에 현재 이동 거리를 더합니다.
        dragDeltaTotal += dragDelta;

        // 드래그 중임을 표시합니다.
        isDragging = true;

        // 드래그 이동에 따라 지도 좌표를 변경합니다.
        latitude -= dragDelta.y * dragFactor; // 위도 이동
        longitude -= dragDelta.x * dragFactor; // 경도 이동 (좌우 반전)

        // 지도 타일을 다시 불러옵니다.
        StartCoroutine(GetMapTile(latitude, longitude, zoom));
    }

    // 주소 검색 버튼 클릭 시 호출되는 메서드
    private void OnSearchButtonClick()
    {
        string address = addressInput.text;
        if (!string.IsNullOrEmpty(address))
        {
            StartCoroutine(geocodeManager.Instance.GetGeocode(address, OnGeocodeReceived));
        }
    }

    // Geocode API 응답을 처리하는 콜백 메서드
    private void OnGeocodeReceived(List<Address> addresses)
    {
        if (addresses != null && addresses.Count > 0)
        {
            if (float.TryParse(addresses[0].y, out float lat) && float.TryParse(addresses[0].x, out float lon))
            {
                latitude = lat;
                longitude = lon;
                StartCoroutine(GetMapTile(latitude, longitude, zoom));
            }
            else
            {
                Debug.LogError("Invalid coordinates received from the geocode API.");
            }
        }
        else
        {
            Debug.LogError("No coordinates found for the address.");
        }
    }

    // 지도 타일을 불러오는 코루틴
    IEnumerator GetMapTile(float latitude, float longitude, int zoom)
    {
        // 네이버 지도 API 요청 URL 생성
        string apiUrl = $"{NaverMapAPI.Instance.mapStaticApiUrl}?w=500&h=500&center={longitude},{latitude}&level={zoom}&pos:{longitude} {latitude}";

        // 지도 타일 요청
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(apiUrl);
        request.SetRequestHeader("X-NCP-APIGW-API-KEY-ID", NaverMapAPI.Instance.clientID);
        request.SetRequestHeader("X-NCP-APIGW-API-KEY", NaverMapAPI.Instance.clientSecret);

        // 요청을 보내고 응답을 기다립니다.
        yield return request.SendWebRequest();

        // 요청 결과를 확인합니다.
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            // 오류가 발생한 경우 오류 메시지를 출력합니다.
            Debug.LogError(request.error);
        }
        else
        {
            // 요청이 성공한 경우, 응답으로 받은 텍스처를 RawImage에 적용합니다.
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            mapImage.texture = texture;
            mapImage.SetNativeSize();
            mapImage.rectTransform.anchoredPosition = Vector2.zero; // 화면 가운데로 고정
            mapImage.transform.localScale = Vector3.one * 3; // 이미지 크기 조정
        }
    }
}
