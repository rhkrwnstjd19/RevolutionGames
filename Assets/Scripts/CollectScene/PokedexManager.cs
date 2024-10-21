using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokedexManager : MonoBehaviour
{
    public GameObject pokedexPanel; // 도감 UI 패널
    public Transform pokedexContent; // 도감 내용이 표시될 위치
    public GameObject pokedexEntryPrefab; // 도감 항목 프리팹

    void Start()
    {
        pokedexPanel.SetActive(false); // 처음엔 도감이 보이지 않음
    }

    public void UpdatePokedex(List<Enemy> capturedMonsters)
    {
        foreach (Transform child in pokedexContent)
        {
            Destroy(child.gameObject); // 기존 항목 제거
        }

        foreach (Enemy enemy in capturedMonsters)
        {
            GameObject entry = Instantiate(pokedexEntryPrefab, pokedexContent);
            entry.transform.Find("MonsterName").GetComponent<Text>().text = enemy.enemyName;
            entry.transform.Find("MonsterDescription").GetComponent<Text>().text = enemy.enemyDescription;
            entry.transform.Find("MonsterImage").GetComponent<Image>().sprite = enemy.enemyImage;
        }
    }

    public void OpenPokedex()
    {
        pokedexPanel.SetActive(true);
        UpdatePokedex(GameManager.Instance.capturedMonsters);
    }

    public void ClosePokedex()
    {
        pokedexPanel.SetActive(false);
    }
}
