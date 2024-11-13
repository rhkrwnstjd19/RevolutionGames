using System.Collections.Generic;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;


public class AdvDungeon : MonoBehaviour
{
    #region Dungeon Info
    public ScriptableDungeon dungeonInfo;
    public string dungeonName => dungeonInfo.dungeonName;
    public int dungeonLevel => dungeonInfo.level;
    public int maxHealth => dungeonInfo.maxHealth;
    public int currentHealth { get; private set; }
    public int moneyAmount => dungeonInfo.moneyAmount;
    public int expAmount => dungeonInfo.expAmount;
    #endregion

    [Header("Dungeon Props")]
    //캐릭터들이 때리고 있을 타겟 모양
    public GameObject TargetShape;
    //detailpanel에 전달해줄 Dungeon 확대용 카메라
    public CinemachineVirtualCamera targetAheadCamera;
    public float distanceFromDungeon=10f;

    private GameObject petObject;
    public bool isReadyToGetHit { get; private set; } = true;//캐릭터들이 공격 가능한지 여부를 판단하는 변수
    public bool isWorking { get; private set; }//현재 캐릭터들을 해당 탐험에 보냈는지 확인하는 변수
    private async void Start()
    {
        await CreateDungeon();

    }
    UnityAction<int,int> uiEvent;
    public async Task CreateDungeon()
    {
        Debug.Log("Start Creating Dungeon...");
        dungeonInfo = await AdvDungeonInfoList.GetRandomDungeonInfo();
        if (dungeonInfo != null)
        {
            Debug.Log($"{dungeonName} Loaded");
            currentHealth = maxHealth;
            isReadyToGetHit = true;
            isWorking = false;
        }
        else
        {
            Debug.LogError("Dungeon info loading failed.");
        }
    }

    public void StartDungeonHit(GameObject petAI,UnityAction<int,int> updateUI)
    {
        // 이미 작업 중인 던전이면 리턴
        if (isWorking) return;
        ClearUICallback();
        isWorking = true;
        isReadyToGetHit = true;
        uiEvent=updateUI;
        uiEvent?.Invoke(currentHealth, maxHealth);

        SpawnCharactersAroundDungeon(petAI);
    }
     public void ClearUICallback()
    {
        uiEvent = null;
    }

    public void ExitDungeonHit()
    {
        isWorking = false;
        isReadyToGetHit = false;
        ClearUICallback();
       // 생성된 모든 펫 캐릭터들을 제거
        if(petObject != null && petObject.GetComponent<Pet>().target == this)
        {
            Destroy(petObject);
            petObject = null;
        }
    }
    /// <summary>
    /// 던전에서 자체적으로 주변에 캐릭터를 생성하는 함수.
    /// 해당 캐릭터들은 초기화 이후 자동적으로 공격을 진행한다.
    /// </summary>
    private void SpawnCharactersAroundDungeon(GameObject petAI)
    {
        if (petAI == null)
        {
            Debug.LogError("Pet AI prefab is null!");
            return;
        }

        // 던전 주변에 랜덤한 위치 계산
        float radius = distanceFromDungeon; // 던전으로부터의 거리
        float randomAngle = Random.Range(0f, 360f);
        Vector3 randomPosition = transform.position + (Quaternion.Euler(0, randomAngle, 0) * Vector3.forward * radius);

        // 지면 높이에 맞추기
        randomPosition.y = transform.position.y;

        // 펫 오브젝트 생성
        GameObject petObject = Instantiate(petAI, randomPosition, Quaternion.identity);

        // 펫이 던전을 바라보도록 회전
        Vector3 directionToDungeon = (transform.position - randomPosition).normalized;
        petObject.transform.rotation = Quaternion.LookRotation(directionToDungeon);
        this.petObject=petObject;
        // Pet 컴포넌트 초기화
        Pet pet = petObject.GetComponent<Pet>();
        if (pet != null)
        {
            pet.SetTarget(this);  // 공격 대상 던전 설정
        }
        else
        {
            Debug.LogError("Spawned object doesn't have Pet component!");
        }
    }
    public void GetDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth=0;
            SetDie();
        }
        uiEvent?.Invoke(currentHealth, maxHealth);
    }
    private void SetRevive()
    {
        TargetShape.SetActive(true);
        //체력 초기화
        currentHealth = maxHealth;
        isReadyToGetHit = true;
    }
    private void SetDie()
    {
        //***TODO***
        //재화 보상 제공
        //간단한 죽을때의 연출
        TargetShape.SetActive(false);
        Invoke(nameof(SetRevive), 5f);
        isReadyToGetHit = false;
    }
}