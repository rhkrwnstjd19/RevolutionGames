using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading;
public static class PetObjectList
{
   private static AsyncOperationHandle<IList<GameObject>> operationHandle;

   public static async Task<List<GameObject>> LoadPetObjects()
   {
       try
       {
           operationHandle = Addressables.LoadAssetsAsync<GameObject>("PetObjects", null);
           var loadedAssets = await operationHandle.Task;
           return new List<GameObject>(loadedAssets);
       }
       catch (System.Exception e)
       {
           Debug.LogError($"Failed to load pet objects: {e.Message}");
           return new List<GameObject>();
       }
   }

   public static void ReleasePetObjects()
   {
       if (operationHandle.IsValid())
       {
           Addressables.Release(operationHandle);
       }
   }
}

public class Pet : MonoBehaviour
{
    public ScriptablePet petData;
    public AdvDungeon target { get; private set; }
    private Animator animator;
    private bool isAttacking = false;
    private bool isInitialized = false;
    private Coroutine attackRoutine;

    void Start()
    {
        if (petData == null)
        {
            Debug.LogError("PetData is not assigned!");
            return;
        }

        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning($"Animator not found on pet {petData.petName}");
        }
        else
        {
            animator.SetBool("Attack", false);
        }

        isInitialized = true;
        Debug.Log($"Pet {petData.petName} initialized");
        
        if (target != null)
        {
            StartAttackProcess();
        }

        if(target==null) LookMainCamera();
    }

    private void LookMainCamera()
    {
        if (Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
        }
    }

    private void StartAttackProcess()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
        }
        attackRoutine = StartCoroutine(WaitForTargetAndAttack());
    }

    IEnumerator WaitForTargetAndAttack()
    {
        Debug.Log($"{petData.petName} waiting for target to be ready...");
        
        // 타겟이 준비될 때까지 대기
        while (target != null && !target.isReadyToGetHit)
        {
            yield return new WaitForSeconds(0.1f);
        }

        // 타겟이 있고 준비가 되었다면 공격 시작
        if (target != null)
        {
            attackRoutine = StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        Debug.Log($"{petData.petName} starting attack routine");
        
        while (target != null)
        {
            // 타겟이 준비되지 않았다면 다시 대기 상태로
            if (!target.isReadyToGetHit)
            {
                Debug.Log($"{petData.petName}'s target not ready, waiting...");
            yield return new WaitUntil(()=>target.isReadyToGetHit);
            }
            if (!isAttacking)
            {
                isAttacking = true;
                
                // 공격 애니메이션 시작
                if (animator != null)
                {
                    animator.SetBool("Attack", true);
                }
                
                // 애니메이션 시작 후 데미지 타이밍까지 대기
                yield return new WaitForSeconds(0.5f);
                
                if (target != null) // 데미지 주기 직전에 한번 더 체크
                {
                    Debug.Log($"{petData.petName} dealing {petData.attackVal} damage to {target.dungeonName}");
                    target.GetDamage(petData.attackVal);
                }
                
                // 공격 애니메이션 완료 대기
                yield return new WaitForSeconds(0.3f);
                
                // Idle 상태로 복귀
                if (animator != null)
                {
                    animator.SetBool("Attack", false);
                }
                
                // 다음 공격까지 쿨다운
                yield return new WaitForSeconds(1.5f);
                
                isAttacking = false;
                Debug.Log($"{petData.petName} attack cooldown finished");
            }
            yield return null;
        }

        ResetAttackState();
    }

    private void ResetAttackState()
    {
        if (animator != null)
        {
            animator.SetBool("Attack", false);
        }
        isAttacking = false;
    }

    public void SetTarget(AdvDungeon targetDungeon)
    {
        if (targetDungeon == null)
        {
            Debug.LogError("Attempted to set null target!");
            return;
        }

        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
        }
        
        ResetAttackState();

        target = targetDungeon;
        Debug.Log($"New target set for {petData.petName}: {targetDungeon.dungeonName}");
        
        if (isInitialized)
        {
            StartAttackProcess();
        }
    }

    private void OnDestroy()
    {
        Debug.Log($"Pet {petData.petName} destroyed");
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
        }
    }
}