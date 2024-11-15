using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;

public class Shopper : MonoBehaviour
{
    public GameObject Shop;
    public CinemachineVirtualCamera targetAheadCamera;
    
    private Vector2 originalPosition;
    private Vector2 closePosition;
    public float animationSpeed = 1.5f;
    void Start()
    {

    }


    public void InitCamera(GameObject shopUI)
    {
        Shop = shopUI;
        targetAheadCamera.Priority = 30;
        SetShopPosition();
    }

    void SetShopPosition(){

        originalPosition = Shop.transform.localPosition;
        closePosition = new Vector2(originalPosition.x, -Screen.height);
        Shop.transform.localPosition = closePosition;
        Shop.SetActive(true);
        OpenAnimation();
    }
    public void OpenAnimation()
    {
        // 패널 애니메이션(Linear로 올라옴)
        Shop.transform.DOLocalMoveY(originalPosition.y, animationSpeed)
        .SetEase(Ease.Linear);
    }
    public void CloseAnimation()
    {
        // 패널 애니메이션(Linear로 내려감)
        transform.DOLocalMoveY(closePosition.y, animationSpeed)
        .SetEase(Ease.Linear)
        .OnComplete(() => gameObject.SetActive(false));
    }
    public void EnterShop()
    {
        float halfScreenHeight = Screen.height * 0.5f;
        Shop.transform.localPosition = new Vector2(originalPosition.x, -halfScreenHeight);

    }

}
