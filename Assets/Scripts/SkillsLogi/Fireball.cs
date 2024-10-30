using System.Collections;
using System.Linq.Expressions;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public ScriptableSkill skill;
    public ParticleSystem fireballHitEffect;
    public float speed = 10f; // Fireball의 속도
    public float lifetime = 5f; // Fireball의 생존 시간
    private AudioSource audioSource;
    
    [Header("발사 각도")]
    static float horizontalAngle = 5f; // 5도로 설정
    private float changeAngle = -1f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(ShootFire());
    }

    IEnumerator ShootFire()
    {
        // 카메라의 forward 방향과 horizontalAngle을 적용한 방향 벡터 설정
        Camera mainCamera = Camera.main;
        Vector3 forward = mainCamera.transform.forward;

        horizontalAngle = horizontalAngle * -1;
        Debug.Log("발사 각도 : " + horizontalAngle);
        // 회전 행렬을 사용하여 방향 벡터 회전
        Vector3 direction = Quaternion.AngleAxis(horizontalAngle, Vector3.up) * forward;
        direction = (direction + mainCamera.transform.up * 0).normalized; // 

        // Fireball의 회전을 이동 방향에 맞게 설정
        transform.rotation = Quaternion.LookRotation(direction);

        float elapsed = 0f;

        while (elapsed < lifetime)
        {
            // Fireball을 설정된 방향으로 지속적으로 이동
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
            other.GetComponent<DungeonEnemy>().TakeDamage(skill.AttackVal);
            Debug.Log("Fireball hit enemy");
            // other.GetComponent<ZombieScript>().decreaseEnemyHp(skill.AttackVal);
            Vector3 hitpos = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + 0.5f);
            var tmp = Instantiate(fireballHitEffect, hitpos, Quaternion.identity);
            tmp.Play();
            Destroy(tmp.gameObject, 1f);
            Destroy(gameObject);
        }
    }
}
