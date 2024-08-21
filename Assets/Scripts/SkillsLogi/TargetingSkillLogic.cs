using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System.Collections;

public class TargetingSkillLogic : MonoBehaviour, ISkillLogic
{
    public float detectionRadius = 10f; // Ž�� �ݰ�
    public float fieldOfView = 60f;     // �þ߰�
    public GameObject targetSkillPrefab;
    public SkillScript skillData;
    private Camera cam; // AR ī�޶�
    public float aliveTime = 5f;
    private bool isDestroyed = false;
    private bool isCooltime = false;
    public GameObject nearestEnemy;
    private void Start()
    {
        cam = Camera.main;
    }

    public void Activate()
    {
        if (!isCooltime)
        {
            nearestEnemy = FindNearestEnemyWithinSight();
            if (nearestEnemy != null)
            {
                StartCoroutine(Cooltime(skillData.cooldown));
                Vector3 screenCenter = new Vector3(0.5f, 0.5f, 0.5f); // z ���� ī�޶󿡼� �󸶳� �ָ� ������ �ִ����� ����
                Vector3 fireStartPosition = Camera.main.ViewportToWorldPoint(screenCenter);

                GameObject skill = Instantiate(targetSkillPrefab, fireStartPosition, Quaternion.identity);
                if (skill.GetComponent<Lightning>())
                {
                    skill.GetComponent<Lightning>().targetEnemy=nearestEnemy;
                }
                StartCoroutine(DestroySkill(skill));
            }
        }
        
    }
    IEnumerator Cooltime(float cooltime)
    {
        isCooltime = true;
        yield return new WaitForSeconds(cooltime);
        isCooltime = false;
    }
    IEnumerator DestroySkill(GameObject skill)
    {
        yield return new WaitForSeconds(aliveTime);

        // �̹� �ı����� �ʾҴٸ� �ı�
        if (skill != null)
        {
            Destroy(skill);
        }
    }

    GameObject FindNearestEnemyWithinSight()
    {
        GameObject nearestEnemy = null;
        float minDistance = float.MaxValue;

        Collider[] hitColliders = Physics.OverlapSphere(cam.transform.position, detectionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.CompareTag("Enemy"))
            {
                Vector3 directionToTarget = (hitCollider.transform.position - cam.transform.position).normalized;
                float angleToTarget = Vector3.Angle(cam.transform.forward, directionToTarget);
                float distanceToTarget = Vector3.Distance(cam.transform.position, hitCollider.transform.position);

                if (angleToTarget < fieldOfView / 2 && distanceToTarget < minDistance)
                {
                    minDistance = distanceToTarget;
                    nearestEnemy = hitCollider.gameObject;
                }
            }
        }

        return nearestEnemy;
    }

}