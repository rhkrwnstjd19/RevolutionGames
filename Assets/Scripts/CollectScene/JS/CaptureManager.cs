
using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

public class CaptureManager : MonoBehaviour
{   
    public Transform addPet;
    public ScriptablePlayer player;

    public GameObject PetFrame;
    public List<TMP_Text> ballCounts = new();
    public List<Button> ballButtons = new();
    public GameObject MenuButton;    

    public Image CurrentBallImage;
    public TMP_Text CurrentBallCount;

    private int currentBallIndex = -1;
    private GameObject CurrentBall;
    
    void Start(){
        UpdateCapturedPet();
        UpdateBallCount();

        for(int i = 0; i < ballButtons.Count; i++){
            int i1 = i;
            Debug.Log($"{i1} : ballButtons[{i1}]");
            ballButtons[i1].onClick.AddListener(() => InstantiateBall(i1));
        }

    }

    void UpdateCapturedPet(){
        for(int i = 0; i < player.petList.petList.Count; i++){
        GameObject pet = Instantiate(PetFrame, addPet);
        pet.transform.SetParent(addPet, false);
        // 자식 오브젝트의 이름이 "Button"이라고 가정합니다.
        Transform child = pet.transform.Find("Button");
        if (child != null){
            Image childImage = child.GetComponent<Image>();
            if (childImage != null){
                childImage.sprite = player.petList.petList[i].petSprite;
            }
        }
    }
    }

    void UpdateBallCount(){
        for(int i = 0; i < player.ballList.Count; i++){
            ballCounts[i].text = player.ballList[i].ballCount.ToString() + "/" + player.ballList[i].ballCountMax.ToString();
        }
        if(currentBallIndex>-1)CurrentBallCount.text = player.ballList[currentBallIndex].ballCount.ToString() + "/"+ player.ballList[currentBallIndex].ballCountMax.ToString();
    }  

    void InstantiateBall(int index){
        Debug.Log($"InstantiateBall {index}");
        if(player.ballList[index].ballCount > 0){
            if(CurrentBall != null){
                Destroy(CurrentBall);
            }
            CurrentBallImage.sprite = player.ballList[index].ballSprite;
            CurrentBall = Instantiate(player.ballList[index].ballPrefab);
            currentBallIndex = index;
            UpdateBallCount();
            MenuButton.SetActive(true);
            gameObject.SetActive(false);
        }
    } 

    public void CapturePet(GameObject pet){
        Debug.Log("CapturePet");
        player.petList.petList.Add(pet.GetComponent<Pet>().petData);
        UpdateCapturedPet();
    }

    public void ThrowBall(){
        Debug.Log("ThrowBall");
        player.ballList[currentBallIndex].UseBall(1);
        UpdateBallCount();
    }

    public int GetBallCount(){
        return player.ballList[currentBallIndex].ballCount;
    }
}
