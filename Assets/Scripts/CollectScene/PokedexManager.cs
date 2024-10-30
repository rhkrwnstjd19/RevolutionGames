using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokedexManager : MonoBehaviour
{
    public List<MonsterData> monsters;  // 몬스터 정보를 저장하는 리스트
    public List<GameObject> monsterSlots;  // UI에서 각 몬스터 슬롯을 연결

    // 몬스터가 포획될 때 호출
    public void CaptureMonster(int monsterIndex)
    {
        if (monsterIndex < monsters.Count && !monsters[monsterIndex].isCaptured)
        {
            monsters[monsterIndex].isCaptured = true;
            UpdatePokedex(monsterIndex);
        }
    }

    // 도감 UI를 업데이트
    private void UpdatePokedex(int monsterIndex)
    {
        GameObject slot = monsterSlots[monsterIndex];
        MonsterData monster = monsters[monsterIndex];

        // 해당 몬스터의 이미지, 이름, 설명을 표시
        slot.transform.Find("MonsterImage").GetComponent<Image>().sprite = monster.monsterImage;
        slot.transform.Find("MonsterName").GetComponent<Text>().text = monster.monsterName;
        slot.transform.Find("MonsterDescription").GetComponent<Text>().text = monster.monsterDescription;

        // 몬스터 슬롯을 활성화
        slot.SetActive(true);
    }
}
