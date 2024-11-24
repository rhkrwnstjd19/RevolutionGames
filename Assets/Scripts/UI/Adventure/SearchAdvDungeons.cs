using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//생성된 Adv Dungeon을 감지한후 저장해놓기 위한 클래스
public static class AdvDungeonList
{
    private static List<AdvDungeon> advDungeons = new();
    public static List<AdvDungeon> GetAdvDungeons() => advDungeons;
    public static void SetAdvDungeons(List<AdvDungeon> list) => advDungeons = list;
    public static void ClearAdvDungeonList()=>advDungeons.Clear();
    public static void AddToList(AdvDungeon dungeon)=>advDungeons.Add(dungeon);
}
public class SearchAdvDungeons : MonoBehaviour
{
    public float searchRange = 5f;
    public float checkInterval=30f;

    void Start()
    {
        StartCoroutine(CheckAdvDungeons());
    }

    IEnumerator CheckAdvDungeons()
    {
        //초기화 완료 후 탐색을 보장하기 위한 오프셋
        yield return new WaitForSeconds(2f);
        Debug.Log("[SearchAdvDungeons] : Start Checking Adventure Dungeons...");
        while (true)
        {
            Collider[] hitColliders=Physics.OverlapSphere(gameObject.transform.position,searchRange);
            AdvDungeonList.ClearAdvDungeonList();
            // 범위 내의 AdvDungeon 객체 탐색
            foreach(var collider in hitColliders){
                if(collider.CompareTag("AdventureDungeon")){
                    AdvDungeon advDungeon=collider.GetComponent<AdvDungeon>();
                    AdvDungeonList.AddToList(advDungeon);
                }
            }
            Debug.Log($"Found {AdvDungeonList.GetAdvDungeons().Count} dungeons ");

            yield return new WaitForSeconds(checkInterval);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, searchRange);
    }
}
