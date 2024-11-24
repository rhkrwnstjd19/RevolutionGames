using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;

// DungeonEnemy 클래스 - FlyingEnemy 또는 WalkingEnemy 중 하나를 선택할 수 있게 함
public interface IEnemy
{
    IEnumerator Move();
    IEnumerator Attack();
    void TakeDamage(float amount);
}
public class DungeonEnemy : MonoBehaviour
{
    //적이 공중형인지 지상형인지 구분 후 y축을 조절하여 생성된다.
    public enum EnemyType { Flying, Walking }
    public EnemyType enemyType;
    public float enemyHp = 50;
    public float maxHP = 50;
    public float speed = 3;
    public int damage = 5;
    public float exp  = 55f;
    public GameObject target;
    private Animator animator;
    private AudioSource audioSource;
    public Transform HPBar;
    private float initialScaleY;
    private bool isAttack = false;

    public GameObject gold;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        target = Camera.main.gameObject;
        animator = GetComponent<Animator>();
        animator.SetBool("Move", true);
        initialScaleY = HPBar.localScale.y;

    }

    private void Update()
    {   //플레이어 위치를 탐색해 플레이어에게 이동
        if (target != null)
        {
            transform.LookAt(target.transform);
            Vector3 direction = (target.transform.position - transform.position).normalized;
            
            if(Vector3.Distance(transform.position, target.transform.position) < 5f)
            {
                //플레이어와의 거리가 5f 이하일 때 공격
                animator.SetBool("Attack", true);
                if(!isAttack)StartCoroutine(Attack(damage));
            }
            else
            {
                transform.position += direction * speed * Time.deltaTime;
                animator.SetBool("Attack", false);
            }
        }
            
    }
    IEnumerator Attack(float damage)
    {
        isAttack = true;
        //target.gameObject.GetComponentInParent<Player>().decreaseHp((int)damage);
        yield return new WaitForSeconds(3f);
        isAttack = false;
    }
    public void TakeDamage(float amount){
        amount = Random.Range(1500, 3000);
        animator.SetBool("GetHit", true);
        audioSource.Play();
        DamageNumberManager.Instance.ShowDamageNumber(transform.position, amount);
        enemyHp -= amount;
        UpdateHPBar();
        if(enemyHp <= 0){
            Die();
        }
        Invoke(nameof(ResetGetHitTrigger), 1f);
    }
    void UpdateHPBar()
    {
        float hpPercentage = enemyHp / maxHP;
        Vector3 newScale = HPBar.localScale;
        newScale.y = initialScaleY * hpPercentage;
        HPBar.localScale = newScale;
    }
    void ResetGetHitTrigger(){
        animator.SetBool("GetHit",false);
    }
    void Die(){
        // 사망후 추가 상호작용이 생기므로 콜라이더를 비활성화 시킨후 사망 애니메이션을 실행
        // 사망한 Enemy는 플레이어에게 경험치를 전달.
        StopAllCoroutines();
        animator.SetTrigger("Die");
        GetComponent<Collider>().enabled = false;
        DungeonManager.Instance.UpdateExp(exp);
        DungeonManager.Instance.DefeatedEnemyCount++;
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z - 0.2f);
        var a = Instantiate(gold, pos, Quaternion.identity);
        Destroy(gameObject, 0.5f);
    }
}
