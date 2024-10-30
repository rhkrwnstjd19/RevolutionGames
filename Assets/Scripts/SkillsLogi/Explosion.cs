using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using CartoonFX;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class Explosion : MonoBehaviour
{
    public ParticleSystem fireposition;
    public ParticleSystem explosionEffect;
    private Camera mainCamera;

    public void Init(){
        if(mainCamera==null)mainCamera = Camera.main;
        else if(mainCamera!=null) FireRay();
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
                TriggerExplosion(hit.point);
            }
        }
        else
        {
            Debug.Log("Ray did not hit any object");
        }
    }
    void TriggerExplosion(Vector3 position)
    {
        // 폭발 효과 생성
        var a = Instantiate(gameObject, position, Quaternion.identity);
        Collider[] hitEnemies = Physics.OverlapBox(position, new Vector3(2.5f, 2.5f, 2.5f), Quaternion.identity, LayerMask.GetMask("Enemy"));
        foreach (Collider enemy in hitEnemies)
        {
            DungeonEnemy enemyComponent = enemy.GetComponent<DungeonEnemy>();
            if (enemyComponent != null)
            {
                enemyComponent.TakeDamage(10); // 예시로 10의 데미지 적용
                Debug.Log("Explosion hit enemy: " + enemy.name);
            }
        }
        Destroy(a, 1f);
        Debug.Log("Explosion triggered at: " + position);
    }
}
