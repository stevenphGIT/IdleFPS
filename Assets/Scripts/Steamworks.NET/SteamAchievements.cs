using BreakInfinity;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamAchievements : MonoBehaviour
{
    private Vars varRef;
    private float timer = 0;
    private CountAchievements[] countAchievements = new CountAchievements[] {
        new CountAchievements("ACH_ONE_TARGET", 1)
    };
    private void Start()
    {
        varRef = GameObject.Find("VarTracker").GetComponent<Vars>();

        if (!SteamManager.Initialized) { return; }
        SteamUserStats.RequestCurrentStats();
    }
    void Update()
    {
        if (!SteamManager.Initialized) {return;}

        timer += Time.deltaTime;

        if (timer > 5f)
        {
            for (int i = 0; i < countAchievements.Length; i++)
            {
                SteamUserStats.GetAchievement(countAchievements[i].GetName(), out bool achievementOwned);
                if (!achievementOwned && (varRef.totalClickTracker + varRef.clickTracker) >= countAchievements[i].GetCountThreshold())
                {
                    SteamUserStats.SetAchievement(countAchievements[i].GetName());
                    SteamUserStats.StoreStats();
                }
            }

            SteamAPI.RunCallbacks();
            timer = 0;
        }
    }

    private class CountAchievements
    {
        string name;
        BigDouble countThreshold;


        public CountAchievements(string n, BigDouble c)
        {
            name = n;
            countThreshold = c;
        }

        public string GetName()
        {
            return name;
        }

        public BigDouble GetCountThreshold()
        {
            return countThreshold;
        }
    }

}
