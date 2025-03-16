using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAchievements : MonoBehaviour
{
    [SerializeField]
    private bool reset;
    void Start()
    {
        if (!SteamManager.Initialized) { return; }

        if (reset)
        {
            SteamUserStats.ResetAllStats(true);
        }
    }
}
