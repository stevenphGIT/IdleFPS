using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Statistics : MonoBehaviour, IDataPersistence
{
    public TMP_Text hText, tText, sText, gText, pText, uText, timeText, eTimeText, cmbText;
    public GameObject stats;
    public string timeStarted;

    // Update is called once per frame
    void Update()
    {
        hText.text = "Total Hits Generated: " + "\n\tThis prestige:" + Vars.Instance.TotalAbbr(Vars.Instance.totalHitCount) + "\n\tTotal:" + Vars.Instance.TotalAbbr(Vars.Instance.totalTotalHitCount + Vars.Instance.totalHitCount);
        tText.text = "Total Targets Clicked: " + "\n\tThis prestige:" + Vars.Instance.TotalAbbr(Vars.Instance.clickTracker) + "\n\tTotal:" + Vars.Instance.TotalAbbr(Vars.Instance.totalClickTracker + Vars.Instance.clickTracker);
        sText.text = "Silver Targets Clicked: " + "\n\tThis prestige:" + Vars.Instance.TotalAbbr(Vars.Instance.silverClickTracker) + "\n\tTotal:" + Vars.Instance.TotalAbbr(Vars.Instance.totalSilverClickTracker + Vars.Instance.silverClickTracker);
        gText.text = "Gold Targets Clicked: " + "\n\tThis prestige:" + Vars.Instance.TotalAbbr(Vars.Instance.goldClickTracker) + "\n\tTotal:" + Vars.Instance.TotalAbbr(Vars.Instance.totalGoldClickTracker + Vars.Instance.goldClickTracker);
        pText.text = "Platinum Targets Clicked: " + "\n\tThis prestige:" + Vars.Instance.TotalAbbr(Vars.Instance.platClickTracker) + "\n\tTotal:" + Vars.Instance.TotalAbbr(Vars.Instance.totalPlatClickTracker + Vars.Instance.platClickTracker);
        uText.text = "Ultimate Targets Clicked: " + "\n\tThis prestige:" + Vars.Instance.TotalAbbr(Vars.Instance.omegaClickTracker) + "\n\tTotal:" + Vars.Instance.TotalAbbr(Vars.Instance.totalOmegaClickTracker + Vars.Instance.omegaClickTracker);
        cmbText.text = "<color=#FF00FF>Combo Record: </color>" + Vars.Instance.comboRecord;
        timeText.text = "You started the game on " + timeStarted;
    }
    public void LoadData(GameData data)
    {
        if (data.timeStartedString.Equals(string.Empty))
        {
            string month = "";
            if (DateTime.Now.Month == 1)
                month = "January";
            else if (DateTime.Now.Month == 2)
                month = "February";
            else if (DateTime.Now.Month == 3)
                month = "March";
            else if (DateTime.Now.Month == 4)
                month = "April";
            else if (DateTime.Now.Month == 5)
                month = "May";
            else if (DateTime.Now.Month == 6)
                month = "June";
            else if (DateTime.Now.Month == 7)
                month = "July";
            else if (DateTime.Now.Month == 8)
                month = "August";
            else if (DateTime.Now.Month == 9)
                month = "September";
            else if (DateTime.Now.Month == 10)
                month = "October";
            else if (DateTime.Now.Month == 11)
                month = "November";
            else if (DateTime.Now.Month == 12)
                month = "December";

            this.timeStarted = month + " " + DateTime.Now.Day + ", " + DateTime.Now.Year;
        }
        else
            this.timeStarted = data.timeStartedString;
    }

    public void SaveData(ref GameData data)
    {
        data.timeStartedString = this.timeStarted;
    }

}
