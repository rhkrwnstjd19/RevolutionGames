using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public struct DungeonInfo
{
    public (float latitude, float longitude) gps;
    public float spawnTime;
    public int dungeonLevel;
    public int monsterCount;
    public bool isEnableEntrance;
    // Constructor for easy initialization
    public DungeonInfo((float latitude, float longitude) gps, float time, int level, int count, bool enable)
    {
        this.gps = gps;
        spawnTime = time;
        dungeonLevel = level;
        monsterCount = count;
        isEnableEntrance = enable;
    }
}

public struct ShopInfo
{
    public (float latitude, float longitude) gps;
    public bool isShopEnable;

    public ShopInfo((float latitude, float longitude) gps, bool enable)
    {
        this.gps = gps;
        isShopEnable = enable;
    }
}



public class DummyManager : Singleton<DummyManager>
{
    public (float latitude, float longitude)[] gpsList;

    public DungeonInfo[] dungeon = new DungeonInfo[5];
    public ShopInfo SI;



    //���� ������ gps�� ����
    private float distance = 10;
    // Start is called before the first frame update
    void Start()
    {

        //SI = new ShopInfo((37.885991f,127.736678f));
        SI = new ShopInfo((37.88648f, 127.7358f), false);
        dungeon[0] = new DungeonInfo((37.791231f,127.123242f), 1, 0, 10,false);

        //0�� ����
        for (int i = 1; i < dungeon.Length; i++)
        {
            //�ణ�� ������ ������ ���� 0.0001�� ������
            dungeon[i] = new DungeonInfo((37.791231f+(float)0.0001*i, 127.123242f+ (float)0.0001 * i), 1, i, 10+5*i-1,false);
        }
    }

    


    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < dungeon.Length; i++)
        {
            double currentDistanceToDungeon = CalculateDistance(GPS_Manager.Instance.latitude, GPS_Manager.Instance.longitude,
            dungeon[i].gps.latitude, dungeon[i].gps.longitude);

            if (currentDistanceToDungeon < distance)
            {
                dungeon[i].isEnableEntrance = true;
            }

            //GPS�� �޾ƿ��� ���ϴ°��(�׽�Ʈ��)
            if (!GPS_Manager.Instance.receiveGPS)
            {
                dungeon[i].isEnableEntrance = true;
            }
        }

        double currentDistanceToShop = CalculateDistance(GPS_Manager.Instance.latitude, GPS_Manager.Instance.longitude,
            SI.gps.latitude, SI.gps.longitude);
        if (currentDistanceToShop < distance)
        {
            SI.isShopEnable = true;
        }
        //GPS�� �޾ƿ��� ���ϴ°��(�׽�Ʈ��)
        if (!GPS_Manager.Instance.receiveGPS)
        {
            SI.isShopEnable = true;
        }

    }
    public double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        var R = 6371e3; // ������ ������ (���� ����)
        var radLat1 = lat1 * Mathf.Deg2Rad; // ������ �������� ��ȯ
        var radLat2 = lat2 * Mathf.Deg2Rad;
        var deltaLat = (lat2 - lat1) * Mathf.Deg2Rad;
        var deltaLon = (lon2 - lon1) * Mathf.Deg2Rad;

        var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                Math.Cos(radLat1) * Math.Cos(radLat2) *
                Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        var distance = R * c; // ���� �Ÿ� (���� ����)
        return distance;
    }
}

