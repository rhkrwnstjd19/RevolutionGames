using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CRMonsterImageSet : MonoBehaviour
{
    public Image resultImage; // ��� UI���� �̹����� ǥ���� Image ������Ʈ
    public TextMeshProUGUI resultText;
    public SpawnMonster spawnMonsterScript; // SpawnMonster ��ũ��Ʈ ����

    private void Start()
    {
        if (spawnMonsterScript != null)
        {
            // ���� ������ �Ϸ�Ǹ� �ش� ������ �̹����� ǥ��
            SetMonsterImage(spawnMonsterScript.selectedMonsterIndex);
        }
    }

    public void SetMonsterImage(int monsterIndex)
    {
        // ���� �������� �̸��� ��ġ�ϴ� �̹��� ������ Resources���� �ε�
        string monsterName = spawnMonsterScript.monsterPrefab[monsterIndex].name;
        Sprite monsterSprite = Resources.Load<Sprite>("UI/RouletteImg/" + monsterName);

        if (monsterSprite != null && resultImage != null)
        {
            // UI �̹����� �ε��� ��������Ʈ ����
            resultImage.sprite = monsterSprite;
        }
        else
        {
            Debug.LogError("���� �̹��� �ε� ����: " + monsterName);
        }

        if (resultText != null)
        {
            string displayName = monsterName.Replace("Pet", "");
            resultText.text = displayName;
        }
        else
        {
            Debug.LogError("TMP ������Ʈ�� �Ҵ�x");
        }
    }
}
