using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;

public class EffectPackageCreator : MonoBehaviour
{
    [System.Serializable]
    public class EffectData
    {
        public List<string> checkedEffectNames = new List<string>();
    }

    public void CreatePackageFromJSON(string jsonFilePath)
    {
        // 1. JSON 파일 읽기
        string json = File.ReadAllText(jsonFilePath);
        EffectData data = JsonUtility.FromJson<EffectData>(json);

        // 2. 선택된 에셋들의 경로 수집 및 의존성 체크
        HashSet<string> assetPaths = new HashSet<string>();
        foreach (string effectName in data.checkedEffectNames)
        {
            CollectAssetAndDependencies(effectName, assetPaths);
        }

        // 3. Unity Package 생성
        string packagePath = EditorUtility.SaveFilePanel("Save Package", "", "SelectedEffects", "unitypackage");
        if (!string.IsNullOrEmpty(packagePath))
        {
            AssetDatabase.ExportPackage(assetPaths.ToArray(), packagePath, ExportPackageOptions.Recurse | ExportPackageOptions.Interactive);
            Debug.Log("Package creation window opened with " + assetPaths.Count + " assets.");
        }
    }

    private void CollectAssetAndDependencies(string effectName, HashSet<string> assetPaths)
    {
        string[] guids = AssetDatabase.FindAssets(effectName);
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            if (!string.IsNullOrEmpty(assetPath))
            {
                assetPaths.Add(assetPath);
                CollectDependencies(assetPath, assetPaths);
            }
        }
    }

    private void CollectDependencies(string assetPath, HashSet<string> assetPaths)
    {
        string[] dependencies = AssetDatabase.GetDependencies(assetPath, true);
        foreach (string dependencyPath in dependencies)
        {
            if (assetPaths.Add(dependencyPath))
            {
                // 새로 추가된 의존성에 대해 재귀적으로 의존성 체크
                CollectDependencies(dependencyPath, assetPaths);
            }
        }
    }
}

[CustomEditor(typeof(EffectPackageCreator))]
public class EffectPackageCreatorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EffectPackageCreator creator = (EffectPackageCreator)target;

        if (GUILayout.Button("Create Package from JSON"))
        {
            string jsonPath = EditorUtility.OpenFilePanel("Select JSON file", "", "json");
            if (!string.IsNullOrEmpty(jsonPath))
            {
                creator.CreatePackageFromJSON(jsonPath);
            }
        }
    }
}
#endif