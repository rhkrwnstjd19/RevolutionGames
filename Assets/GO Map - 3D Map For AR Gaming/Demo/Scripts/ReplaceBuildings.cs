using UnityEngine;
using GoShared;
using GoMap;
using System.Collections.Generic;
using System;
using Math = UnityEngine.Random;


public class ReplaceBuildings : MonoBehaviour
{
    public List<Mesh> mesh;
    public Material material;

    public List<GameObject> SnowHouse = new();
    public GameObject BossDungeon;
    private List<String> buildingName = new();
    private GOPOIKind currentBuildingKind;

    void Start(){
        Invoke("ReplaceMesh", 3);
        buildingName.Add("Restaurant");
        buildingName.Add("School");
        buildingName.Add("Hospital");
        buildingName.Add("Cafe");
        buildingName.Add("Bank");
        buildingName.Add("Pharmacy");
        buildingName.Add("University");
        buildingName.Add("Convenience");

    }
    public void ReplaceMesh(){
        Debug.Log("Building Counts = " + BuildingList.Buildings.Count);
        for(int i = 0 ; i < BuildingList.Buildings.Count; i++){
            int randomNum = Math.Range(0,8);
            currentBuildingKind = BuildingList.Buildings[i].GetComponent<GOFeatureBehaviour>().goFeature.poiKind;
            if(buildingName.Contains(currentBuildingKind.ToString())) continue;
            if(BuildingList.Buildings[i].GetComponent<GOFeatureBehaviour>().goFeature.name == "building" && Math.Range(0,3) <1){
                BuildingList.Buildings[i].GetComponent<MeshFilter>().mesh = mesh[randomNum];
                BuildingList.Buildings[i].gameObject.transform.position =findCenter(BuildingList.Buildings[i].GetComponent<GOFeatureBehaviour>().goFeature.convertedGeometry);
                BuildingList.Buildings[i].gameObject.transform.rotation = Quaternion.Euler(0,-14,0);
                //Debug.Log("Building Position : " + BuildingList.Buildings[i].gameObject.transform.position);
                //BuildingList.buildingList[i].gameObject.transform.position = BuildingList.buildingPosition[i][0];
            }
            else if(randomNum > 7){
                Vector3 DungeonPos = BuildingList.Buildings[i].gameObject.transform.position =findCenter(BuildingList.Buildings[i].GetComponent<GOFeatureBehaviour>().goFeature.convertedGeometry);
                var a = Instantiate(BossDungeon, DungeonPos, Quaternion.Euler(0,-7,0));
                Destroy(BuildingList.Buildings[i].gameObject);
            }
            else if(BuildingList.Buildings[i].GetComponent<GOFeatureBehaviour>().goFeature.name == "building"){
                Vector3 HousePos = BuildingList.Buildings[i].gameObject.transform.position =findCenter(BuildingList.Buildings[i].GetComponent<GOFeatureBehaviour>().goFeature.convertedGeometry);
                var a = Instantiate(SnowHouse[Math.Range(0,2)], HousePos, Quaternion.Euler(0,-14,0));
                Destroy(BuildingList.Buildings[i].gameObject);
            }


        }
        BuildingList.Buildings.Clear();
    }

    private Vector3 findCenter(List<Vector3> vertices){
        float BigX= -9999;
        float SmallX= 9999;
        float BigZ= -9999;
        float SmallZ= 9999;

        foreach(Vector3 vertex in vertices){
            if(vertex.x>BigX){
                BigX = vertex.x;
            }
            if(vertex.x<SmallX){
                SmallX = vertex.x;
            }
            if(vertex.y>BigZ){
                BigZ = vertex.z;
            }
            if(vertex.y<SmallZ){
                SmallZ = vertex.z;
            }
        }
        return new Vector3((BigX+SmallX)/2,0,  (BigZ+SmallZ)/2 -6);
    }
}
