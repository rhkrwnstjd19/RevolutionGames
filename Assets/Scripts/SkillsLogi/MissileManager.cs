using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileManager : Singleton<MissileManager>
{
    public GameObject leftMissilePrefab;
    public GameObject rightMissilePrefab;
    public Transform explosionEffect;
    private Camera mainCamera;
    private bool isInit=false;

    public List<Transform> initPos = new List<Transform>();
    public List<GameObject> missiles = new List<GameObject>();

    public int firedMissile;
    public void Init(){
        if(!isInit){
            mainCamera = Camera.main;
            InstantiteMissile();
            isInit = true;
        }
        else {
            StartCoroutine(FireMissiles());
            //FireRay();

        }
    }

    public void setMissile(){
        for(int i = 0; i <=5; i++){ // 0~2 왼쪽 미사일
            missiles[i].transform.position = initPos[i].position;
            missiles[i].SetActive(true);
        }
    }

    //미사일 생성
    void InstantiteMissile(){
        for(int i = 0; i <=2; i++){ // 0~2 왼쪽 미사일
            GameObject missile = Instantiate(leftMissilePrefab, initPos[i].position, Quaternion.identity);
            missile.SetActive(false);
            missiles.Add(missile);
        }
        for(int i=3; i <=5 ; i++){ // 3~5 오른쪽 미사일
            GameObject missile = Instantiate(rightMissilePrefab, initPos[i].position, Quaternion.identity);
            missile.SetActive(false);
            missiles.Add(missile);
        }
        firedMissile =4;
        setMissile();
    }
    public void FireRay(){
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2.0f); // Ray 시각화
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Ray hit : " + hit.collider.gameObject.name);
            if (hit.collider.gameObject.tag == "Enemy")
            {
                StartCoroutine(FireMissiles());
            }
        }
        else
        {
            Debug.Log("Ray did not hit any object");
        }
    }
    
    IEnumerator FireMissiles(){
        setMissile();
        yield return new WaitForSeconds(0.1f);
        for(int i = 0; i < missiles.Count/2; i++){
            missiles[i].GetComponent<Missile>().Fire();
            missiles[missiles.Count-i-1].GetComponent<Missile>().Fire();
            yield return new WaitForSeconds(0.1f);
        }
    }
}
