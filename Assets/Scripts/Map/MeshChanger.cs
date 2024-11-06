using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeshChanger : MonoBehaviour
{
    public Mesh newMesh; // 교체할 Mesh
    private MeshFilter meshFilter;
    private List<GameObject> buildingsList;

    void Awake()
    {
        // 리스트 사용 예시
        foreach (GameObject building in buildingsList)
        {
            Debug.Log(building.name);
        }

        StartCoroutine(FindBuildings());    
    }

    IEnumerator FindBuildings(){
        while(true){
            buildingsList = GameObject.FindGameObjectsWithTag("Buildings").ToList();
            yield return new WaitForSeconds(1f);
            if(buildingsList.Count > 0){
                ChangeMesh();
                break;
            }
        }
    }

    void ChangeMesh()
    {
        foreach (GameObject building in buildingsList)
        {
            building.GetComponent<MeshFilter>().mesh = newMesh;
        }   
    }
}
