using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GenerateAbilityIDs : EditorWindow
{
    [MenuItem("Tools/Set Ability IDs")]
    private static void AssignUniqueIDs()
    {
        Ability[] objects = Resources.LoadAll<Ability>("Abilities/");
        HashSet<int> usedIDs = new HashSet<int>();
        int nextID = 1;

        foreach (Ability obj in objects)
        {
            if (obj.id > 0)
            {
                usedIDs.Add(obj.id);
            }
        }

        foreach (Ability obj in objects)
        {
            if (obj.id == 0)
            {
                while (usedIDs.Contains(nextID))
                {
                    nextID++;
                }

                obj.id = nextID;
                usedIDs.Add(nextID);

                EditorUtility.SetDirty(obj);
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log("ids saved!");
    }
}
