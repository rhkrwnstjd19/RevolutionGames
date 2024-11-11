using System.Collections.Generic;
using System.Threading.Tasks;
using Cinemachine;
using UnityEngine;
using UnityEngine.AddressableAssets;
public static class AdvDungeonInfoList
{
    private static List<ScriptableDungeon> dungeonInfos = new();
    public static async Task LoadDungeonInfos()
    {
        if (dungeonInfos.Count == 0) // 데이터가 비어 있을 때만 로드
        {
            dungeonInfos = await GetInfosFromAddressable();
        }
    }

    public static async Task<ScriptableDungeon> GetRandomDungeonInfo()
    {
        // dungeonInfos가 비어 있을 때 데이터를 로드
        if (dungeonInfos.Count == 0)
        {
            Debug.Log("Dungeon infos are empty. Loading data from Addressables...");
            await LoadDungeonInfos();

            // 데이터 로드 후에도 비어 있으면 에러 반환
            if (dungeonInfos.Count == 0)
            {
                Debug.LogError("Failed to load dungeon info data.");
                return null;
            }
        }

        // 랜덤 인덱스 추출 (범위: 0부터 시작)
        int input = Random.Range(0, dungeonInfos.Count);
        return dungeonInfos[input];
    }

    private static async Task<List<ScriptableDungeon>> GetInfosFromAddressable()
    {
        var result = new List<ScriptableDungeon>();
        result.AddRange(await Addressables.LoadAssetsAsync<ScriptableDungeon>("AdvDungeonInfo", null).Task);
        return result;
    }

}

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

    public bool isReadyToGetHit { get; private set; } = true;//캐릭터들이 공격 가능한지 여부를 판단하는 변수
    public bool isWorking { get; private set; }//현재 캐릭터들을 해당 탐험에 보냈는지 확인하는 변수
    private async void Start()
    {
        await CreateDungeon();
    }

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

    public void StartDungeonHit()
    {
        //***TODO***
        //AdventureDetailPanel에서 선택된 펫 리스트를 받아와야 한다.
        //던전이 공격받는 상태로 전환되며, 이후 해당 던전 클릭시 때리고 있는 UI로 확인될 예정
        //해당 던전을 때리고 있는 펫은 다른 던전에서 선택이 불가하다.

    }
    public void ExitDungeonHit()
    {
        //***TODO***
        //전투 상태를 벗어나 캐릭터 반환.
        //던전이 공격받는 상태로 전환되며, 이후 해당 던전 클릭시 때리고 있는 UI로 확인될 예정
    }
    /// <summary>
    /// 던전에서 자체적으로 주변에 캐릭터를 생성하는 함수.
    /// 해당 캐릭터들은 초기화 이후 자동적으로 공격을 진행한다.
    /// </summary>
    private void SpawnCharactersAroundDungeon()
    {
        //***TODO***
        //해당 캐릭터들의 아이디 값에 맞춰 프리팹을 생성한다. 

    }
    public void GetDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            SetDie();
        }
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

    }
}