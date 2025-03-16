using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GenerateDialogueIDs : EditorWindow
{
    [MenuItem("Tools/Set Dialogue Object IDs")]
    private static void AssignUniqueIDs()
    {
        TextHolder[] objects = Resources.LoadAll<TextHolder>("");
        HashSet<int> usedIDs = new HashSet<int>();
        int nextID = 1;

        foreach (TextHolder obj in objects)
        {
            if (obj.identifier > 0)
            {
                usedIDs.Add(obj.identifier);
            }
        }

        foreach (TextHolder obj in objects)
        {
            if (obj.identifier == 0)
            {
                while (usedIDs.Contains(nextID))
                {
                    nextID++;
                }

                obj.identifier = nextID;
                usedIDs.Add(nextID);

                EditorUtility.SetDirty(obj);
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log("ids saved!");
    }
}
