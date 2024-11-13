using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
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
           // 시작시 Idle 상태로 설정
           animator.SetBool("Attack", false);
       }

       isInitialized = true;
       Debug.Log($"Pet {petData.petName} initialized");
       
       if (target != null && target.isReadyToGetHit)
       {
           StartCoroutine(AttackRoutine());
       }
   }

   IEnumerator AttackRoutine()
   {
       while (target != null && target.isReadyToGetHit)
       {
           if (!isAttacking)
           {
               // 공격 시작 - 애니메이션 트리거
               isAttacking = true;
               if (animator != null)
               {
                   animator.SetBool("Attack", true);
               }
               
               // 애니메이션이 시작되고 실제 데미지가 들어가기까지 약간의 딜레이
               yield return new WaitForSeconds(0.5f);  // 애니메이션 타이밍에 맞게 조절 필요
               
               Debug.Log($"{petData.petName} dealing {petData.attackVal} damage to {target.dungeonName}");
               target.GetDamage(petData.attackVal);
               
               // 공격 애니메이션이 끝나기를 기다림
               yield return new WaitForSeconds(.3f);  // 애니메이션 타이밍에 맞게 조절 필요
               
               // Idle 상태로 돌아감
               if (animator != null)
               {
                   animator.SetBool("Attack", false);
               }
               
               // 다음 공격까지 대기
               yield return new WaitForSeconds(1.5f);  // 총 3초의 공격 주기를 맞추기 위한 나머지 시간
               
               isAttacking = false;
               Debug.Log($"{petData.petName} attack cooldown finished");
           }
           yield return null;
       }

       if (animator != null)
       {
           animator.SetBool("Attack", false);
       }
   }

   public void SetTarget(AdvDungeon targetDungeon)
   {
       if (targetDungeon == null)
       {
           Debug.LogError("Attempted to set null target!");
           return;
       }

       StopAllCoroutines(); // 기존 공격 루틴 중지
       
       if (animator != null)
       {
           animator.SetBool("Attack", false); // Idle 상태로 리셋
       }

       target = targetDungeon;
       Debug.Log($"New target set for {petData.petName}: {targetDungeon.dungeonName}");
       
       if (isInitialized && target.isReadyToGetHit)
       {
           StartCoroutine(AttackRoutine());
       }
   }

   private void OnDestroy()
   {
       Debug.Log($"Pet {petData.petName} destroyed");
       StopAllCoroutines();
   }
}