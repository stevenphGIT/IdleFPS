using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ReplaceText : EditorWindow
{
    private TMP_FontAsset newFont;

    [MenuItem("Tools/Replace TMP Fonts")]
    public static void ShowWindow()
    {
        GetWindow<ReplaceText>("TMP Font Replacer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Replace All TMP Fonts", EditorStyles.boldLabel);

        newFont = (TMP_FontAsset)EditorGUILayout.ObjectField("New TMP Font", newFont, typeof(TMP_FontAsset), false);

        if (GUILayout.Button("Replace in All Scenes and Prefabs"))
        {
            if (newFont == null)
            {
                Debug.LogError("Please assign a TMP Font Asset.");
                return;
            }

            ReplaceFontsInPrefabs();
            ReplaceFontsInScenes();
            AssetDatabase.SaveAssets();
            Debug.Log("Font replacement complete.");
        }
    }

    private void ReplaceFontsInScenes()
    {
        string[] scenePaths = Directory.GetFiles("Assets", "*.unity", SearchOption.AllDirectories);
        foreach (string scenePath in scenePaths)
        {
            var scene = EditorSceneManager.OpenScene(scenePath);
            ReplaceFontsInCurrentScene();
            EditorSceneManager.SaveScene(scene);
        }
    }

    private void ReplaceFontsInCurrentScene()
    {
        TextMeshProUGUI[] textsUGUI = GameObject.FindObjectsOfType<TextMeshProUGUI>(true);
        foreach (var tmp in textsUGUI)
        {
            tmp.font = newFont;
            EditorUtility.SetDirty(tmp);
        }

        TextMeshPro[] texts = GameObject.FindObjectsOfType<TextMeshPro>(true);
        foreach (var tmp in texts)
        {
            tmp.font = newFont;
            EditorUtility.SetDirty(tmp);
        }
    }

    private void ReplaceFontsInPrefabs()
    {
        string[] prefabPaths = Directory.GetFiles("Assets", "*.prefab", SearchOption.AllDirectories);
        foreach (string prefabPath in prefabPaths)
        {
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            var tmps = prefab.GetComponentsInChildren<TextMeshProUGUI>(true);
            var tmps3D = prefab.GetComponentsInChildren<TextMeshPro>(true);

            bool changed = false;

            foreach (var tmp in tmps)
            {
                tmp.font = newFont;
                EditorUtility.SetDirty(tmp);
                changed = true;
            }

            foreach (var tmp in tmps3D)
            {
                tmp.font = newFont;
                EditorUtility.SetDirty(tmp);
                changed = true;
            }

            if (changed)
            {
                PrefabUtility.SavePrefabAsset(prefab);
            }
        }
    }
}
