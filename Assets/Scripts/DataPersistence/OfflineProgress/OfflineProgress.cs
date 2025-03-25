using BreakInfinity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OfflineProgress : MonoBehaviour, IDataPersistence
{
    public TimeSpan saveTime;
    public TimeSpan loadTime;
    private BigDouble offlineProd;
    private BigDouble offlineThor;
    public int differenceTime;
    public NoticeBox notice;
    public string startTime;

    void Awake()
    {

    }
    void Start()
    {
        double offlineBonus = UpsAndVars.Instance.offlineYield;
        if (LocationManager.Instance.activeLocation == 2 && AvailableUpgrades.Instance.locationBonuses)
        {
            offlineBonus *= (1 + UpsAndVars.Instance.locationBonus * LocationManager.Instance.locations[2].GetMult());
        }
        offlineProd = Vars.Instance.totalHps * differenceTime * offlineBonus;
        offlineThor = UpsAndVars.Instance.offlineThorium * (differenceTime / 3600.0f) * Vars.Instance.rads;
        offlineThor = BigDouble.Floor(offlineThor);

        LocationManager.Instance.cooldown -= differenceTime;

        for (int i = 0; i < 5; i++)
        {
            Abilities.Instance.abUpTime[i] -= differenceTime;
            Abilities.Instance.abDoneTime[i] -= differenceTime;
        }
        if (BigDouble.Round(offlineProd) != 0)
        {
            Vars.Instance.hits += offlineProd;
            Vars.Instance.totalHitCount += offlineProd;
            Vars.Instance.thorium += offlineThor;
            string displayPhrase = "You were away for " + ShortenedTime(differenceTime) + ". Your weapons worked at " + (UpsAndVars.Instance.offlineYield * 100) + "% efficiency to generate " + Vars.Instance.PriceAbbr(offlineProd);
            if (offlineThor != 0)
            {
                displayPhrase += (". You also earned " + Vars.Instance.TotalAbbr(offlineThor) + " <color=#00FFFF><sprite index=1>thorium</color>.");
            }
            SpeechBox.Instance.Say("System", displayPhrase);
        }
    }
    public string ShortenedTime(int time)
    {
        if (time < 60)
            return time + " seconds";
        else if (time < 3600)
            if( time / 60 == 1)
                return (time / 60) + " minute";
            else
                return (time / 60) + " minutes";
        else
            if (time / 3600 == 1)
            return (time / 3600) + " hour";
        else
            return (time / 3600) + " hours";
    }
    public void LoadData(GameData data)
    {
        loadTime = DateTime.Now.Subtract(new DateTime(2000, 1, 1, 0, 0, 0));
        differenceTime = (int)(loadTime.TotalSeconds - data.timeSaved);
    }

    public void SaveData(ref GameData data)
    {
        saveTime = DateTime.Now.Subtract(new DateTime(2000, 1, 1, 0, 0, 0));
        data.timeSaved = saveTime.TotalSeconds;
    }
}
