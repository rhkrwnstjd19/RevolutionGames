using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using Game;
using UnityEngine;

public class DungeonBoss : MonoBehaviour
{
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
        animator = GetComponent<Animator>();
        initialScaleY = HPBar.localScale.y;
        StartCoroutine(SetCameraPosition());

    }

    IEnumerator SetCameraPosition()
    {
        animator.SetBool("Appear", true);
        yield return new WaitForSeconds(3f);
        animator.SetBool("Appear", false);
        target = Camera.main.gameObject;
        animator.SetBool("Move", true);
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
        animator.SetBool("GetHit", true);
        //audioSource.Play();
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
        StopAllCoroutines();
        animator.SetTrigger("Die");
        GetComponent<Collider>().enabled = false;
        DungeonManager.Instance.UpdateExp(exp);
        DungeonManager.Instance.UpdateGold(100);
        DungeonManager.Instance.DefeatedEnemyCount++;
        Destroy(gameObject, 3f);
    }
}
