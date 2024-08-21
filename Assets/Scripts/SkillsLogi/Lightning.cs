
using UnityEngine;
using DigitalRuby.LightningBolt;
public class Lightning : MonoBehaviour
{
    public SkillScript skill;
    private LightningBoltScript lightning;
    public GameObject targetEnemy;
    // Start is called before the first frame update
    void Start()
    {
        
        lightning.StartObject.transform.position = Camera.main.transform.position;
        targetEnemy.GetComponent<ZombieScript>().decreaseEnemyHp(skill.damage);
    }
    private void Update()
    {
        lightning.EndObject.transform.position = targetEnemy.transform.position;
    }

}
