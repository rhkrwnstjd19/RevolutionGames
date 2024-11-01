using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

public class Gold : MonoBehaviour
{
    public GameObject goldMagnet;
    public float speed = 3f;
    public float amplitude = 0.5f; // 떠오르는 높이
    public float frequency = 1f;   // 떠오르는 속도
    public float rotateSpeed = 1; // 회전 속도 (초당 90도)

    private Vector3 startPos;

    void Start()
    {
        goldMagnet = GameObject.Find("GoldMagnet");
        startPos = transform.position;
        StartCoroutine(flipGold());  
    }
    



    IEnumerator flipGold()
    {
        float time = 0f;
        while(time < 1.2f){
            // 위아래로 부드럽게 움직이기
            float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            // X축을 중심으로 회전하기
            transform.Rotate(Vector3.right, rotateSpeed * Time.deltaTime);
            time += Time.deltaTime;
            Debug.Log($"time: {time}");
            yield return null;
        }
        StartCoroutine(AbsorbGold());  

    }
    
    IEnumerator AbsorbGold()
    {

        while(true){
            if(goldMagnet != null)
            {
                transform.LookAt(goldMagnet.transform);
                Vector3 direction = (goldMagnet.transform.position - transform.position).normalized;
                
                if(Vector3.Distance(transform.position, goldMagnet.transform.position) < 0.5f)
                {
                    DungeonManager.Instance.UpdateGold(1);
                    break;
                }
                else
                {
                    transform.position += direction * speed * Time.deltaTime;
                }
            }
            yield return null;
        }

        Destroy(gameObject);
    }
}
