using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PrestigeUpgradeDatabase : ScriptableObject
{
    [SerializeField] private PrestigeUpgradeObject[] pdb;

    public int Size()
    {
        return pdb.Length;
    }

    [ContextMenu("Sort By IDs")]
    public void SortByItemIDs()
    {
        pdb = new PrestigeUpgradeObject[76];
        var foundUpgrades = Resources.LoadAll<PrestigeUpgradeObject>("PrestigeUpgrades").OrderBy(i => i.id).ToList();

        for (int i = 0; i < foundUpgrades.Count; i++)
        {
            pdb[foundUpgrades[i].id] = foundUpgrades[i]; 
        }
    }

    public PrestigeUpgradeObject GetObjectByID(int i)
    {
        return pdb[i];
    }
}
