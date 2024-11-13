using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
public static class PetObjectList
{
    public static async Task<List<GameObject>> LoadPetObjects()
    {
        var result = new List<GameObject>();
        result.AddRange(await Addressables.LoadAssetsAsync<GameObject>("PetObjects", null).Task);
        return result;
    }

}

public class Pet : MonoBehaviour
{
    public ScriptablePet petData;
    private AdvDungeon target;
    private Animator animator;
    private bool isAttack = false;

    void Start(){
        target = FindObjectOfType<AdvDungeon>();
        animator = GetComponent<Animator>();
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
                if(!isAttack && target.isReadyToGetHit)StartCoroutine(Attack(petData.attackVal));
            }
            else
            {
                animator.SetBool("Attack", false);
            }
        }
            
    }
    IEnumerator Attack(int damage)
    {
        isAttack = true;
        target.GetDamage(damage);
        yield return new WaitForSeconds(3f);
        isAttack = false;
    }
}
