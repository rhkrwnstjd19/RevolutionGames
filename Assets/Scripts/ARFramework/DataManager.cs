using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    //�÷��̾� ������ �����ϱ� ���� ������ �Ŵ���
    public int PlayerExp { get; private set; }
    public int PlayerLevel { get; private set; }
    public int PlayerMaxHp { get; private set; }
    public int Money { get; private set; }

    private int[] expMax = {10,30,60,100,150,210,280,360,450};
    private int[] hpByLevel = { 5, 10, 20, 25, 30, 35, 40, 45, 50 };
    void Start()
    {
        LoadData();
    }

    void LoadData()
    {
        Debug.Log("Succesfullt Loaded Data!");
        // ������ �ҷ�����
        PlayerExp = PlayerPrefs.GetInt("PlayerExp", 0);
        PlayerLevel = PlayerPrefs.GetInt("PlayerLevel", 1);
        PlayerMaxHp = PlayerPrefs.GetInt("PlayerMaxHp", 5);
        Money = PlayerPrefs.GetInt("Money", 0);
    }
    public void DeleteAllData()
    {
        PlayerPrefs.DeleteAll();
        LoadData();
        PlayerPrefs.Save();

    }
    public void SaveData()
    {
        // ������ �����ϱ�
        PlayerPrefs.SetInt("PlayerExp", PlayerExp);
        PlayerPrefs.SetInt("PlayerLevel", PlayerLevel);
        PlayerPrefs.SetInt("PlayerMaxHp", PlayerMaxHp);
        PlayerPrefs.SetInt("Money", Money);

        PlayerPrefs.Save();
    }

    // ������ ������Ʈ ���� �Լ�
    public void UpdateExp(int exp)
    {
        PlayerExp += exp;
        // �ʿ��� ��� �ٸ� ���� �߰�
        if(PlayerExp >= expMax[PlayerLevel - 1])
        {
            UpdateLevelUp();
            
        }
        SaveData();
    }
    public void UpdateLevelUp()
    {
        PlayerExp = PlayerExp - expMax[PlayerLevel - 1];
        PlayerMaxHp= hpByLevel[PlayerLevel - 1];
        PlayerLevel++;
    }

    public void UpdateLevel(int level)
    {
        PlayerLevel = level;
        SaveData();
    }

    public void UpdateMaxHp(int maxHp)
    {
        PlayerMaxHp = maxHp;
        SaveData();
    }

    public void UpdateMoney(int money)
    {
        Money += money;
        SaveData();
    }
    public int GetExpMax()
    {
        return expMax[PlayerLevel - 1];
    }
}
