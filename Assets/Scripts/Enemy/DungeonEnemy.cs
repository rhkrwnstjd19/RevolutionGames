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
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        target = Camera.main.gameObject;
        animator = GetComponent<Animator>();
        animator.SetBool("Move", true);
        initialScaleY = HPBar.localScale.y;

    }

    private void Update()
    {
        if (target != null)
        {
            transform.LookAt(target.transform);
            // Vector3 currentRotation = transform.eulerAngles;
            // transform.eulerAngles = new Vector3(0, currentRotation.y, 0);
            Vector3 direction = (target.transform.position - transform.position).normalized;
            
            if(Vector3.Distance(transform.position, target.transform.position) < 5f)
            {
                animator.SetBool("Attack", true);
                if(!isAttack)StartCoroutine(Attack(damage));
            }
            else
            {
                transform.position += direction * speed * Time.deltaTime;
                animator.SetBool("IsAttack", false);
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
        animator.SetBool("GetHit", true);
        audioSource.Play();
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
        StopAllCoroutines();
        animator.SetTrigger("Die");
        GetComponent<Collider>().enabled = false;
        DungeonManager.Instance.UpdateExp(exp);
        Destroy(gameObject, 1f);
    }
}
