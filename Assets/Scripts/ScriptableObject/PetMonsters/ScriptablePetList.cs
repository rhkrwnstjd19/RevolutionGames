using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PetSOList", menuName = "ScriptableObject/PetSOList", order = 1)]
public class ScriptablePetList : ScriptableObject
{
    public List<ScriptablePet> petList = new();
    
}
