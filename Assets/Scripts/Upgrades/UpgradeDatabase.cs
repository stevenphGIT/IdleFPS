using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//[CreateAssetMenu]
public class UpgradeDatabase : ScriptableObject
{
    [SerializeField] private List<UpgradeObject> udb;

    public int Size()
    {
        return udb.Count;
    }

    [ContextMenu("Set IDs")]
    public void SetItemIDs()
    {
        udb = new List<UpgradeObject>();
        var foundUpgrades = Resources.LoadAll<UpgradeObject>("Upgrades").OrderBy(i => i.ID).ToList();

        var hasIDInRange = foundUpgrades.Where(i => i.ID != -1 && i.ID < foundUpgrades.Count).OrderBy(i => i.ID).ToList();
        var hasIDNotInRange = foundUpgrades.Where(i => i.ID != -1 && i.ID >= foundUpgrades.Count).OrderBy(i => i.ID).ToList();
        var noID = foundUpgrades.Where(i => i.ID <= -1).ToList();

        var index = 0;

        for(int i = 0; i < foundUpgrades.Count; i++)
        {
            UpgradeObject nextUpgrade;
            nextUpgrade = hasIDInRange.Find(d => d.ID == i);

            if(nextUpgrade != null)
            {
                udb.Add(nextUpgrade);
            }
            else if(index < noID.Count)
            {
                noID[index].ID = i;
                nextUpgrade = noID[index];
                index++;
                udb.Add(nextUpgrade);
            }
        }

        foreach(var upg in hasIDNotInRange)
        {
            udb.Add(upg);
        }
    }

    public void Buy(int i)
    {
        udb.RemoveAt(i);
    }

    public UpgradeObject findName(int i)
    {
        foreach(var upg in udb)
        {
            if(upg.pin == i)
            {
                return upg;
            }
        }
        return null;
    }
}
