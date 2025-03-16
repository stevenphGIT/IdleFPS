using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DuelReset : MonoBehaviour
{
    public static DuelReset Instance;

    private void Awake()
    {
        if(Instance == null) { Instance = this; }
    }

    public void ResetAll()
    {
        ResetVars();
        Gun.Instance.ResetAll();
        AvailableUpgrades.Instance.ResetAll();
        AvailableUpgrades.Instance.SortAndDisplay();
        Abilities.Instance.ResetAbilities();
        LocationManager.Instance.activeLocation = 0;
        UpsAndVars.Instance.ResetUpgrades();
        List<GameObject> targetsToDelete = new List<GameObject>();

        AddTagToList(targetsToDelete, "Target");
        AddTagToList(targetsToDelete, "SilverTarget");
        AddTagToList(targetsToDelete, "GoldTarget");
        AddTagToList(targetsToDelete, "PlatTarget");
        AddTagToList(targetsToDelete, "OmegaTarget");
        
        foreach (GameObject obj in targetsToDelete)
        {
            Destroy(obj);
        }
        BoardHandler.Instance.activeTargetCount = 0;
        BoardHandler.Instance.posList.Clear();
        targetsToDelete = new List<GameObject>();
    }

    private void AddTagToList(List<GameObject> objects, string tag)
    {
        GameObject[] targetsToAdd = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject obj in targetsToAdd)
        {
            objects.Add(obj);
        }
    }

    private void ResetVars()
    {
        Vars.Instance.hits = 0;
        Vars.Instance.clickTracker = 0;
        Vars.Instance.totalClickTracker = 0;
        Vars.Instance.silverClickTracker = 0;
        Vars.Instance.totalSilverClickTracker = 0;
        Vars.Instance.goldClickTracker = 0;
        Vars.Instance.totalGoldClickTracker = 0;
        Vars.Instance.platClickTracker = 0;
        Vars.Instance.totalPlatClickTracker = 0;
        Vars.Instance.omegaClickTracker = 0;
        Vars.Instance.totalOmegaClickTracker = 0;

        Vars.Instance.rads = 0;
        Vars.Instance.thorium = 0;
        Vars.Instance.totalHitCount = 0;
        Vars.Instance.totalTotalHitCount = 0;
        Vars.Instance.totalHps = 0;
        Vars.Instance.nonHgHps = 0;

        Vars.Instance.silverHpsSecs = 1;
        Vars.Instance.goldHpsSecs = 15;
        Vars.Instance.platHpsSecs = 60;

        for (int i = 0; i < Vars.Instance.hps.Count; i++)
        {
            Vars.Instance.hps[i] = 0;
        }
    }
}
