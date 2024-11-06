using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    public SkillScript skill;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<ZombieScript>().decreaseEnemyHp(skill.damage);
            Destroy(gameObject);
        }
    }


}
