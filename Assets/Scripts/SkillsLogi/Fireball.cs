using System.Collections;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public ScriptableSkill skill;
    public ParticleSystem fireballHitEffect;
    public float speed = 10f; // Fireball의 속도
    public float lifetime = 5f; // Fireball의 생존 시간
    [Range(-45f, 45f)]
    public float horizontalAngle = 0f; // 좌우 각도 (-45 ~ 45도)

    void Start()
    {
        StartCoroutine(ShootFire());
    }

    IEnumerator ShootFire()
    {
        // 카메라의 forward 방향과 horizontalAngle을 적용한 방향 벡터 설정
        Camera mainCamera = Camera.main;
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        // 수평 각도를 라디안으로 변환
        float angleRad = horizontalAngle * Mathf.Deg2Rad;

        // 회전 행렬을 사용하여 방향 벡터 회전
        Vector3 direction = Quaternion.AngleAxis(horizontalAngle, Vector3.up) * forward;
        direction = (direction + mainCamera.transform.up * 0.2f).normalized; // 약간의 상향 조정 (필요에 따라 조절 가능)

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
            Debug.Log("Fireball hit enemy");
            ZombieScript zombie = other.gameObject.GetComponent<ZombieScript>();
            if (zombie != null)
            {
                zombie.decreaseEnemyHp(skill.AttackVal);
            }

            // 충돌 지점 근처에 Hit Effect 소환
            Vector3 hitPosition = other.ClosestPoint(transform.position) + Vector3.up * 1f;

            if (fireballHitEffect != null)
            {
                ParticleSystem tmp = Instantiate(fireballHitEffect, hitPosition, Quaternion.identity);
                tmp.Play();
                Destroy(tmp.gameObject, tmp.main.duration); // 효과가 끝난 후 파괴
            }

            // Fireball 파괴
            Destroy(gameObject);
        }
    }
}
