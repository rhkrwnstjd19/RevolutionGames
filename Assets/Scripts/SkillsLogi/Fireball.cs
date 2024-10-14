using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public ScriptableSkill skill;
    
    void Start(){
        StartCoroutine(shootFire());
    }
    IEnumerator shootFire(){
       // 카메라의 forward 방향을 기준으로 이동 방향 설정
        Vector3 direction = Camera.main.transform.forward;

        // Fireball의 회전을 카메라의 회전과 일치시킴
        transform.rotation = Camera.main.transform.rotation;

        // Fireball의 속도 설정
        float speed = 10f;

        // Fireball의 생존 시간 설정
        float lifetime = 5f;
        float elapsed = 0f;

        while (elapsed < lifetime)
        {
            // 지속적으로 Fireball을 이동시킴
            transform.Translate(direction * speed * Time.deltaTime, Space.World);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // 생존 시간이 지난 후 Fireball 파괴
        Destroy(gameObject);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<ZombieScript>().decreaseEnemyHp(skill.AttackVal);
            Destroy(gameObject);
        }
    }


}
