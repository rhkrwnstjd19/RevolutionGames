using UnityEngine;
using GoShared;
using GoMap;
using System.Collections.Generic;



public class ReplaceBuildings : MonoBehaviour
{
    public List<Mesh> mesh;
    public Material material;
    public GameObject BossDungeon;
    void Start(){
        Invoke("ReplaceMesh", 3);
    }
    public void ReplaceMesh(){
        Debug.Log("Building Counts = " + BuildingList.Buildings.Count);
        for(int i = 0 ; i < BuildingList.Buildings.Count; i++){
            int randomNum = Random.Range(0,8);

            //Debug.Log($"Building {i} : {BuildingList.buildingFeature[i].name}");
            if(BuildingList.Buildings[i].GetComponent<GOFeatureBehaviour>().goFeature.name == "building" && Random.Range(0,3) <1){
                BuildingList.Buildings[i].GetComponent<MeshFilter>().mesh = mesh[randomNum];
                BuildingList.Buildings[i].gameObject.transform.position =findCenter(BuildingList.Buildings[i].GetComponent<GOFeatureBehaviour>().goFeature.convertedGeometry);
                BuildingList.Buildings[i].gameObject.transform.rotation = Quaternion.Euler(0,-7,0);
                //Debug.Log("Building Position : " + BuildingList.Buildings[i].gameObject.transform.position);
                //BuildingList.buildingList[i].gameObject.transform.position = BuildingList.buildingPosition[i][0];
            }
            else if(randomNum > 7){
                Vector3 DungeonPos = BuildingList.Buildings[i].gameObject.transform.position =findCenter(BuildingList.Buildings[i].GetComponent<GOFeatureBehaviour>().goFeature.convertedGeometry);
                var a = Instantiate(BossDungeon, DungeonPos, Quaternion.Euler(0,0,0));
                Destroy(BuildingList.Buildings[i].gameObject);
            }
            else if(BuildingList.Buildings[i].GetComponent<GOFeatureBehaviour>().goFeature.name == "building"){
                BuildingList.Buildings[i].GetComponent<MeshRenderer>().material = material;
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
