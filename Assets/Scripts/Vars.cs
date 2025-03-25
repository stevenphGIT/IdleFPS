using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BreakInfinity;
using UnityEngine.UI;
using System.Threading;
using System;
using Steamworks;

public class Vars : MonoBehaviour, IDataPersistence
{
    public static Vars Instance;

    public Gun gunRef;
    public TMP_Text scoreText;
    public TMP_Text shopScoreText;
    public TMP_Text totalHitsText;
    public TMP_Text radsText;
    public int duelWins = 0;
    public BigDouble hits = 0;
    public BigDouble thorium = 0;
    public BigDouble rads = 0;
    public int tokens = 0;

    public List<int> addedDialogueIDs = new List<int>();

    public BigDouble totalHitCount = 0;

    public BigDouble totalHps = 0;
    public BigDouble nonHgHps = 0;

    public List<BigDouble> hps;
    //Target Vars
    public int clickTracker;
    public int silverClickTracker;
    public int goldClickTracker;
    public int platClickTracker;
    public int omegaClickTracker;
    public double silverHpsSecs;
    public double goldHpsSecs;
    public double platHpsSecs;
    public double omegaHpsSecs;
    public double luck;
    private float timer;
    //Stat Vars
    public BigDouble totalTotalHitCount;
    public int totalClickTracker;
    public int totalSilverClickTracker;
    public int totalGoldClickTracker;
    public int totalPlatClickTracker;
    public int totalOmegaClickTracker;

    public int comboRecord;
    public int abilitiesUsed;
    //Shop variables
    public InputSlider buyMultSlider;
    public int buyMultiplier;
    public TMP_Text oneTimes;
    public TMP_Text tenTimes;
    public TMP_Text hundTimes;
    //Fun
    public int snowmenSlain = 0;

    private List<string> nFormat = new List<string>();

    void Awake()
    {
        if (Instance == null) { Instance = this; }

        timer = 0;

        nFormat.Add("");
        nFormat.Add(" thousand");
        nFormat.Add(" million");
        nFormat.Add(" billion");
        nFormat.Add(" trillion");
        nFormat.Add(" quadrillion");
        nFormat.Add(" quintillion");
        nFormat.Add(" sextillion");
        nFormat.Add(" septillion");
        nFormat.Add(" octillion");
        nFormat.Add(" nonillion");
        nFormat.Add(" decillion");
        nFormat.Add(" undecillion");
        nFormat.Add(" duodecillion");
        nFormat.Add(" tredecillion");
        nFormat.Add(" quattuordecillion");
        nFormat.Add(" quindecillion");
        nFormat.Add(" sexdecillion");
        nFormat.Add(" septendecillion");
        nFormat.Add(" octodecillion");
        nFormat.Add(" novemdecillion");
        nFormat.Add(" vigintillion");
        nFormat.Add(" unvigintillion");
        nFormat.Add(" duovigintillion");
        nFormat.Add(" trevigintillion");
        nFormat.Add(" quattuorvigintillion");
        nFormat.Add(" quinvigintillion");
        nFormat.Add(" sexvigintillion");
        nFormat.Add(" septenvigintillion");
        nFormat.Add(" octovigintillion");
        nFormat.Add(" novemvigintillion");
        nFormat.Add(" trigintillion");
        nFormat.Add(" untrigintillion");
        nFormat.Add(" duotrigintillion");
        nFormat.Add(" tretrigintillion");
        nFormat.Add(" quatuortrigintillion");
        nFormat.Add(" quintrigintillion");
        nFormat.Add(" sextrigintillion");
        nFormat.Add(" septentrigintillion");
        nFormat.Add(" octotrigintillion");
        nFormat.Add(" novemtrigintillion");
        nFormat.Add(" Infinity");
    }

    void Start()
    {
        for (int i = 0; i < gunRef.gunVisuals.Count; i++)
        {
            hps.Add(0);
        }
        silverHpsSecs = 1;
        goldHpsSecs = 15;
        platHpsSecs = 60;
        omegaHpsSecs = 3600;

        UpdateVars();
    }

    void Update()
    {
        totalHps = FindHPS();
        scoreText.text = Abbr(hits);
        nonHgHps = totalHps - hps[0];
        totalHitsText.text = HpsAbbr(totalHps) + " hits/sec";
        radsText.text = TotalAbbr(rads) + " <color=#00FF00><sprite index=0>rads</color>";

        timer += Time.deltaTime;
        if (timer > 0.01f)
        {
            hits += totalHps / 100;
            totalHitCount += totalHps / 100;
            timer -= 0.01f;
        }

        if (buyMultSlider.valUpdated)
        {
            UpdateVars();
            buyMultSlider.valUpdated = false;
        }
    }
    public void UpdateVars()
    {
        if (buyMultSlider.sliderValue == 0)
        {
            buyMultiplier = 1;
            oneTimes.color = Color.yellow;
            tenTimes.color = Color.white;
            hundTimes.color = Color.white;
        }
        else if (buyMultSlider.sliderValue == 1)
        {
            buyMultiplier = 10;
            oneTimes.color = Color.white;
            tenTimes.color = Color.yellow;
            hundTimes.color = Color.white;
        }
        else if (buyMultSlider.sliderValue == 2)
        {
            buyMultiplier = 100;
            oneTimes.color = Color.white;
            tenTimes.color = Color.white;
            hundTimes.color = Color.yellow;
        }

        Gun.Instance.UpdatePrices();
    }
    public BigDouble FindHPS()
    {
        BigDouble total = 0;
        for (int i = 0; i < hps.Count; i++)
            total += hps[i];
        return total;
    }

    public string Abbr(BigDouble value)
    {
        if (value < 10000)
        {
            return value.Floor() + " hits";
        }
        int num = 0;
        while (value >= 1000)
        {
            num++;
            
            value /= 1000;
        }
       

        return OneDecimalBigDouble(value, 1) + "\r\n" + nFormat[num] + " hits";
    }
    public string TotalAbbr(BigDouble value)
    {
        if (value < 10000)
            return value.Floor() + "";
        int num = 0;
        while (value >= 1000)
        {
            num++;
            value /= 1000;
        }
        return OneDecimalBigDouble(value, 1) + nFormat[num];
    }
    public string HpsAbbr(BigDouble value)
    {
        int num = 0;
        while (value >= 1000)
        {
            num++;
            value /= 1000;
        }
        return OneDecimalBigDouble(value, 1) + nFormat[num];
    }

    public string PriceAbbr(BigDouble value)
    {
        if (value < 10000)
            return value.Floor() + " hits";
        int num = 0;
        while (value >= 1000)
        {
            num++;
            value /= 1000;
        }
        return OneDecimalBigDouble(value, 1) + nFormat[num] + " hits";
    }

    public string PopupAbbr(BigDouble value)
    {
        int num = 0;
        while (value >= 1000)
        {
            num++;
            value /= 1000;
        }
        return OneDecimalBigDouble(value, 1) + "\n" + nFormat[num];
    }

    public string OneDecimalBigDouble(BigDouble val, int decimalCount)
    {
        double returnDouble = val.ToDouble();
        returnDouble *= Math.Pow(10, decimalCount);
        returnDouble = Math.Floor(returnDouble);
        returnDouble /= Math.Pow(10, decimalCount);

        string returnString = "";
        if (returnDouble % 1 == 0)
        {
            returnString += ".0";
        }
        
        return returnDouble + returnString;
    }

    public void AddHits(BigDouble hits)
    {
        this.hits += hits;
    }

    public void LoadData(GameData data)
    {
        this.hits = data.targets;
        this.thorium = data.thorium;
        this.rads = data.rads;
        this.tokens = data.tokens;

        this.totalHitCount = data.totalHitCount;
        this.totalTotalHitCount = data.totalTotalHitCount;
        this.clickTracker = data.clickTracker;
        this.totalClickTracker = data.totalClickTracker;
        this.silverClickTracker = data.silverClickTracker;
        this.totalSilverClickTracker = data.totalSilverClickTracker;
        this.goldClickTracker = data.goldClickTracker;
        this.totalGoldClickTracker = data.totalGoldClickTracker;
        this.platClickTracker = data.platClickTracker;
        this.totalPlatClickTracker = data.totalPlatClickTracker;
        this.omegaClickTracker = data.omegaClickTracker;
        this.totalOmegaClickTracker = data.totalOmegaClickTracker;

        this.totalHps = data.totalHPS;
        this.duelWins = data.duelWins;

        this.comboRecord = data.comboRecord;
        this.abilitiesUsed = data.abilitiesUsed;

        this.snowmenSlain = data.snowmenSlain;

        this.addedDialogueIDs = new List<int>();
        foreach (int id in data.addedDialogueIDs)
        {
            this.addedDialogueIDs.Add(id);
        }
    }
    public void SaveData(ref GameData data)
    {
        data.targets = this.hits;
        data.thorium = this.thorium;
        data.rads = this.rads;
        data.tokens = this.tokens;

        data.totalHitCount = this.totalHitCount;
        data.totalTotalHitCount = this.totalTotalHitCount;
        data.clickTracker = this.clickTracker;
        data.totalClickTracker = this.totalClickTracker;
        data.silverClickTracker = this.silverClickTracker;
        data.silverClickTracker = this.silverClickTracker;
        data.goldClickTracker = this.goldClickTracker;
        data.totalGoldClickTracker = this.totalGoldClickTracker;
        data.platClickTracker = this.platClickTracker;
        data.totalPlatClickTracker = this.totalPlatClickTracker;
        data.omegaClickTracker = this.omegaClickTracker;
        data.totalOmegaClickTracker = this.omegaClickTracker;
        data.totalHPS = this.totalHps;

        data.duelWins = this.duelWins;

        data.comboRecord = this.comboRecord;
        data.abilitiesUsed = this.abilitiesUsed;

        data.snowmenSlain = this.snowmenSlain;

        data.addedDialogueIDs = new List<int>();
        foreach (int id in this.addedDialogueIDs)
        {
            data.addedDialogueIDs.Add(id);
        }
    }
}
