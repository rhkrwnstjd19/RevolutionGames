using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float impulsePower = 10f;
    public int enemyExp = 10;
    public int moneyDrop = 3;
    
    Rigidbody rb;
    GameObject target;
    Vector3 direction;
    [SerializeField]
    private float enemyHp = 3;
    private int attackAmount = 2;
    private bool isDead;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            direction = (target.transform.position - transform.position).normalized;
            rb.MovePosition(transform.position + direction * moveSpeed * Time.deltaTime);
        }
        if (isDead)
        {
            DataManager.Instance.UpdateExp(enemyExp);
            DataManager.Instance.UpdateMoney(moneyDrop);
            GameManager.Instance.EnemyDefeated();
            Destroy(this.gameObject);
        }
    }
    public void decreaseEnemyHp(float attack)
    {
        enemyHp -= attack;
        if (enemyHp <= 0)
        {
            isDead = true;
        }
    }
    private void OnTriggerEnter(Collider collider)
    {
        
        if (collider.gameObject.CompareTag("Player"))
        {
            collider.gameObject.GetComponent<Player>().decreaseHp(attackAmount);
            Destroy(this.gameObject);
        }
    }
}
