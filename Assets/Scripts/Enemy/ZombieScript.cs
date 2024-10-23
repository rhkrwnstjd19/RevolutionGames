using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieScript : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float impulsePower = 10f;
    public int enemyExp = 10;
    public int moneyDrop = 3;

    public AudioClip zombieWalk;
    public AudioClip zombieDead;

    public Transform HPBar;
    private float initialScaleY;

    AudioSource zombieSound;
    Rigidbody rb;
    GameObject target;
    Vector3 direction;
    [SerializeField]
    private float enemyHp = 3;
    private float maxHP = 3;
    private int attackAmount = 2;
    private bool isDead;
    private bool isAttack=false;
    private Animator animator;

    void Awake(){
        initialScaleY = HPBar.localScale.y;
    }
    // Start is called before the first frame update
    void Start()
    {
        zombieSound=GetComponent<AudioSource>();
        target = Camera.main.gameObject;
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        zombieSound.clip = zombieWalk;
        zombieSound.Play();
        maxHP = enemyHp;
    }
    private void Update()
    {
        if (target != null)
        {
            transform.LookAt(target.transform);
            Vector3 currentRotation = transform.eulerAngles;
            transform.eulerAngles = new Vector3(0, currentRotation.y, 0);
            //Beacuse Trigger was not working, Calculated the distance for attacking
            Debug.Log((Vector3.Distance(transform.position, target.transform.position)));
            if(Vector3.Distance(transform.position, target.transform.position) < 4f)
            {
                
                animator.SetBool("IsAttack", true);
                if (!isAttack)
                {
                    StartCoroutine(AttackPlayer(target, attackAmount));
                }
            }
            else
            {
                animator.SetBool("IsAttack", false);
            }
        }
            
    }
    IEnumerator AttackPlayer(GameObject target,int attackAmount)
    {
        isAttack = true;
        target.gameObject.GetComponentInParent<Player>().decreaseHp(attackAmount);
        yield return new WaitForSeconds(3f);
        isAttack = false;
    }
    
    
    void UpdateHPBar()
    {
        float hpPercentage = enemyHp / maxHP;
        Vector3 newScale = HPBar.localScale;
        newScale.y = initialScaleY * hpPercentage;
        HPBar.localScale = newScale;
    }
    public void decreaseEnemyHp(int attack)
    {
        enemyHp -= attack;
        UpdateHPBar();
        if (enemyHp <= 0)
        {
            //zombieSound.PlayOneShot(zombieDead);
            isDead = true;
            // DataManager.Instance.UpdateExp(enemyExp);
            // DataManager.Instance.UpdateMoney(moneyDrop);
            animator.SetBool("IsDead", isDead);
            gameObject.GetComponent<Collider>().enabled = false;
            Destroy(this.gameObject, 2f);
            //GameManager.Instance.EnemyDefeated();
        }
    }
   
}
