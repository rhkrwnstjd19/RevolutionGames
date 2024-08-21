using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopper : MonoBehaviour
{
    public GameObject Shop;
    public GameObject shopQ;

    ShopInfo shopInfo;
    private bool isVisible = true;
    private bool enteredShop = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Your continuous update logic goes here
        shopInfo = DummyManager.Instance.SI;
        Debug.Log(shopInfo.isShopEnable);
        if (shopInfo.isShopEnable && !enteredShop)
        {
            ShowNPC();
            gameObject.GetComponent<CapsuleCollider>().enabled = true;
        }
        else
        {
            HideNPC();
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
        }
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // Assume only one touch for simplicity

            // Check if it's the beginning of a touch
            if (touch.phase == TouchPhase.Began)
            {
                // Perform a raycast from the touch position
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                // Set the maximum distance for the raycast based on your scene
                float maxRaycastDistance = 10f;

                // Perform the raycast
                if (Physics.Raycast(ray, out hit, maxRaycastDistance))
                {
                    // Check if the ray hit the NPC
                    if (hit.collider.CompareTag("shopper"))
                    {
                        // NPC touched, perform interaction logic
                        NPCInteractionLogic();
                        shopQ.SetActive(true);
                    }
                }
            }
        }
    }

    //for test in Unity
    void OnMouseDown()
    {
        shopQ.SetActive(true);
    }


    void NPCInteractionLogic()
    {
        // Your NPC interaction logic goes here
        Debug.Log("NPC Touched!");
    }
   
    public void EnterShop()
    {
        Shop.SetActive(true);
        enteredShop = true;
        shopQ.SetActive(false);
    }

    public void ExitShop()
    {
       Shop.SetActive(false);
       enteredShop = false;
       shopQ.SetActive(false);
    }
    // Method to show the NPC
    private void ShowNPC()
    {
        isVisible = true;
        transform.localScale = new Vector3(3f, 3f, 3f);
    }

    // Method to hide the NPC
    private void HideNPC()
    {
        isVisible = false;
        transform.localScale = new Vector3(0f, 0f, 0f);
    }
}
