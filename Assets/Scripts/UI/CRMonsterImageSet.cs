using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CRMonsterImageSet : MonoBehaviour
{
    public Image resultImage; // 결과 UI에서 이미지를 표시할 Image 컴포넌트
    public TextMeshProUGUI resultText;
    public SpawnMonster spawnMonsterScript; // SpawnMonster 스크립트 참조

    private void Start()
    {
        if (spawnMonsterScript != null)
        {
            // 몬스터 선택이 완료되면 해당 몬스터의 이미지를 표시
            SetMonsterImage(spawnMonsterScript.selectedMonsterIndex);
        }
    }

    public void SetMonsterImage(int monsterIndex)
    {
        // 몬스터 프리팹의 이름과 일치하는 이미지 파일을 Resources에서 로드
        string monsterName = spawnMonsterScript.monsterPrefab[monsterIndex].name;
        Sprite monsterSprite = Resources.Load<Sprite>("UI/RouletteImg/" + monsterName);

        if (monsterSprite != null && resultImage != null)
        {
            // UI 이미지에 로드한 스프라이트 설정
            resultImage.sprite = monsterSprite;
        }
        else
        {
            Debug.LogError("몬스터 이미지 로드 실패: " + monsterName);
        }

        if (resultText != null)
        {
            string displayName = monsterName.Replace("Pet", "");
            resultText.text = displayName;
        }
        else
        {
            Debug.LogError("TMP 컴포넌트가 할당x");
        }
    }
}
