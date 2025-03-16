using BreakInfinity;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;

public class UpsAndVars : MonoBehaviour, IDataPersistence
{
    public static UpsAndVars Instance;
    public PrestigeUpgradeDatabase pdb;

    //For the below list, 0 is locked, 1 is unbought, 2 is bought.
    public List<int> upgradeStatus;
    public List<GameObject> spriteHolderObjects;
    public List<SpriteRenderer> spriteHolders;
    public List<SpriteRenderer> upgradeSprites;

    public List<Sprite> sprites;

    public TMP_Text thoriumText;
    public TMP_Text radsText;

    public List<int> boughtPins;
    public List<int> revealedPins;

    public GameObject crosshairButton;
    public GameObject musicButton;
    public GameObject buyAllButton;
    public GameObject abilityBotButton;
    //Upgrade Vars
    public BigDouble prestigeTargetMultiplier;
    public BigDouble prestigeIdleMultiplier;
    public bool critsUnlocked;
    public bool thoriumCrits;
    public double thoriumBonus;
    public int bonusTargets;
    public bool permabilities;
    public double critOdds;
    public double critAmount;
    public double cooldown;
    public double duration;
    public bool cCustomUnlocked;
    public bool laserActive;
    public bool targetception;
    public bool criticalCooldowns;
    public bool permault;
    public double offlineYield;
    public double priceBonus;
    public double locationBonus;
    public int starterPackLevel;
    public double radMult;
    public double offlineThorium;
    public bool jukeboxUnlocked;
    public bool buyAllUnlocked;
    public bool locationTimeReduced;
    public bool priceUpgrades;
    public bool skipCutsceneUnlocked;
    public bool abilityBot;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        for (int i = 0; i < spriteHolderObjects.Count; i++)
        {
            spriteHolders.Add(spriteHolderObjects[i].GetComponent<SpriteRenderer>());
            upgradeSprites.Add(spriteHolderObjects[i].transform.GetChild(0).GetComponent<SpriteRenderer>());
        }
        SetAllUpgradeVars();
    }
    void Start()
    {
        SetDisplay();
    }

    public void SetUpgradeImages()
    {
        if (upgradeStatus[36] == 2 && upgradeStatus[73] == 2)
        {
            if (upgradeStatus[74] != 2)
            {
                upgradeStatus[74] = 1;
            }
            spriteHolderObjects[74].SetActive(true);
        }
        else
        {
            spriteHolderObjects[74].SetActive(false);
        }
        for (int i = 0; i < spriteHolderObjects.Count - 1; i++)
        {
            Sprite placeholder;
            int idleOrHit;
            if (upgradeStatus[i] == 0)
            {
                placeholder = sprites[0];
                spriteHolders[i].sprite = placeholder;
                upgradeSprites[i].enabled = false;
            }
            else
            {
                upgradeSprites[i].enabled = true;
                if (i <= 36)
                    idleOrHit = 0; //Hit
                else
                    idleOrHit = 2; //Idle

                spriteHolders[i].sprite = sprites[idleOrHit + upgradeStatus[i]];
                //PS Unity if you could make 2D arrays compatible with the editor it would have made this a lot easier, just so you know.
            }
        }
    }
    public void SetDisplay()
    {
        SetVarTexts();
        SetUpgradeImages();
        if (cCustomUnlocked)
            crosshairButton.SetActive(true);
        else
            crosshairButton.SetActive(false);

        if (jukeboxUnlocked)
            musicButton.SetActive(true);
        else
            musicButton.SetActive(false);

        if (buyAllUnlocked)
            buyAllButton.SetActive(true);
        else
            buyAllButton.SetActive(false);

        if (abilityBot)
            abilityBotButton.SetActive(true);
        else
            abilityBotButton.SetActive(false);
    }
    public void SetVarTexts()
    {
        radsText.text = Vars.Instance.TotalAbbr(Vars.Instance.rads) + " <color=#00FF00><sprite index=0>rads</color>";
        thoriumText.text = Vars.Instance.TotalAbbr(Vars.Instance.thorium) + " <color=#00FFFF><sprite index=1>thorium</color>";
    }
    public void ShowTextbox(int id)
    {
        if (upgradeStatus[id] == 0 || pdb.GetObjectByID(id) == null)
        {
            Textbox.Instance.HideBox();
        }
        else
        {
            if (id <= 36)
                Textbox.Instance.SetTitleColor(Color.green);
            else
                Textbox.Instance.SetTitleColor(Color.cyan);
            Textbox.Instance.SetTitleText(pdb.GetObjectByID(id).upgName);
            Textbox.Instance.SetFullDescriptionText(pdb.GetObjectByID(id).upgDescription);
            if (upgradeStatus[id] == 2)
                Textbox.Instance.SetCostText("Purchased!");
            else
                Textbox.Instance.SetCostText(Vars.Instance.TotalAbbr(pdb.GetObjectByID(id).thoriumPrice) + " thorium");
            Textbox.Instance.SetCostColor(Color.cyan);
            //textboxRef.SetFlavorText(pdb.GetObjectByID(id).upgLore);
            Textbox.Instance.ShowBox();
        }
    }
    public void BuyUpgrade(int id)
    {
        if (upgradeStatus[id] == 0 || upgradeStatus[id] == 2)
            return;
        if (pdb.GetObjectByID(id).thoriumPrice <= Vars.Instance.thorium)
        {
            Vars.Instance.thorium -= pdb.GetObjectByID(id).thoriumPrice;
            Vars.Instance.rads += pdb.GetObjectByID(id).thoriumPrice;
            for (int i = 0; i < pdb.GetObjectByID(id).upgradesUnlocked.Count; i++)
            {
                if (upgradeStatus[pdb.GetObjectByID(id).upgradesUnlocked[i]] == 0)
                    upgradeStatus[pdb.GetObjectByID(id).upgradesUnlocked[i]] = 1;
            }
            upgradeStatus[id] = 2;
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.abilitySounds[4], 1f);
            SetAllUpgradeVars();
            SetDisplay();
        }
        else
        {
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.cantUse, 1f);
        }
    }

    public void ResetUpgrades()
    {
        prestigeTargetMultiplier = 1f;
        prestigeIdleMultiplier = 1f;
        bonusTargets = 0;
        critsUnlocked = false;
        thoriumCrits = false;
        thoriumBonus = 1;
        permabilities = false;
        critOdds = 0;
        critAmount = 1;
        cooldown = 1;
        duration = 1;
        cCustomUnlocked = false;
        laserActive = false;
        targetception = false;
        criticalCooldowns = false;
        permault = false;
        offlineYield = 0.01;
        priceBonus = 1;
        locationBonus = 0.1;
        starterPackLevel = 0;
        radMult = 0.01;
        offlineThorium = 0;
        jukeboxUnlocked = false;
        buyAllUnlocked = false;
        locationTimeReduced = false;
        priceUpgrades = false;
        skipCutsceneUnlocked = false;
        abilityBot = false;
        SetDisplay();
    }
    public void SetAllUpgradeVars()
    {
        prestigeTargetMultiplier = 1f;
        prestigeIdleMultiplier = 1f;
        bonusTargets = 0;
        critsUnlocked = false;
        thoriumCrits = false;
        thoriumBonus = 1;
        permabilities = false;
        critOdds = 0;
        critAmount = 1;
        cooldown = 1;
        duration = 1;
        cCustomUnlocked = false;
        laserActive = false;
        targetception = false;
        criticalCooldowns = false;
        permault = false;
        offlineYield = 0.01;
        priceBonus = 1;
        locationBonus = 0.1;
        starterPackLevel = 0;
        radMult = 0.01;
        offlineThorium = 0;
        jukeboxUnlocked = false;
        buyAllUnlocked = false;
        locationTimeReduced = false;
        priceUpgrades = false;
        skipCutsceneUnlocked = false;
        abilityBot = false;

        

        //Row 1
        if (upgradeStatus[0] == 2)
            prestigeTargetMultiplier *= 5;
        if (upgradeStatus[1] == 2)
            critsUnlocked = true;
        if (upgradeStatus[2] == 2)
            thoriumCrits = true;
        if (upgradeStatus[3] == 2)
            thoriumBonus += 0.5;
        if (upgradeStatus[4] == 2)
            bonusTargets += 1;
        if (upgradeStatus[5] == 2)
            bonusTargets += 1;
        if (upgradeStatus[6] == 2)
            bonusTargets += 1;
        //Row 2
        if (upgradeStatus[7] == 2)
            permabilities = true;
        if (upgradeStatus[8] == 2)
            critOdds += 1;
        if (upgradeStatus[9] == 2)
            critAmount += 0.5;
        if (upgradeStatus[10] == 2)
            thoriumBonus += 1;
        if (upgradeStatus[11] == 2)
            cooldown += 0.5;
        if (upgradeStatus[12] == 2)
            cooldown += 1.5;
        if (upgradeStatus[13] == 2)
            cooldown += 3;
        //Row 3
        if (upgradeStatus[14] == 2)
            cCustomUnlocked = true;
        if (upgradeStatus[15] == 2)
            critOdds += 2;
        if (upgradeStatus[16] == 2)
            critAmount += 2;
        if (upgradeStatus[17] == 2)
            thoriumBonus += 1.5;
        if (upgradeStatus[18] == 2)
            prestigeTargetMultiplier *= 2;
        if (upgradeStatus[19] == 2)
            prestigeTargetMultiplier *= 3;
        if (upgradeStatus[20] == 2)
            duration += 1;
        //Row 4
        if (upgradeStatus[21] == 2)
            prestigeTargetMultiplier *= 2;
        if (upgradeStatus[22] == 2)
            critOdds += 3;
        if (upgradeStatus[23] == 2)
            critAmount += 3;
        if (upgradeStatus[24] == 2)
            thoriumBonus += 2;
        if (upgradeStatus[25] == 2)
            prestigeTargetMultiplier *= 2;
        if (upgradeStatus[26] == 2)
            prestigeTargetMultiplier *= 3;
        if (upgradeStatus[27] == 2)
            duration += 2;
        //Row 5
        if (upgradeStatus[28] == 2)
            laserActive = true;
        if (upgradeStatus[29] == 2)
            targetception = true;
        if (upgradeStatus[30] == 2)
            criticalCooldowns = true;
        if (upgradeStatus[31] == 2)
            prestigeTargetMultiplier *= 4;
        if (upgradeStatus[32] == 2)
            prestigeTargetMultiplier *= 3;
        //Row 6
        if (upgradeStatus[33] == 2)
            prestigeTargetMultiplier *= 4;
        if (upgradeStatus[34] == 2)
            permault = true;
        if (upgradeStatus[35] == 2)
            prestigeTargetMultiplier *= 4;
        //Row 7
        if (upgradeStatus[36] == 2)
            prestigeTargetMultiplier *= 10;
        //(Right Side)
        //Row 8
        if (upgradeStatus[37] == 2)
            prestigeIdleMultiplier *= 10;
        if (upgradeStatus[38] == 2)
            offlineYield += 0.24;
        if (upgradeStatus[39] == 2)
            offlineYield += 0.25;
        if (upgradeStatus[40] == 2)
            offlineYield += 0.25;
        if (upgradeStatus[41] == 2)
            priceBonus -= 0.05;
        if (upgradeStatus[42] == 2)
            priceBonus -= 0.10;
        if (upgradeStatus[43] == 2)
            priceBonus -= 0.15;
        //Row 9
        if (upgradeStatus[44] == 2)
            locationBonus *= 2;
        if (upgradeStatus[45] == 2)
            locationBonus *= 2;
        if (upgradeStatus[46] == 2)
            locationBonus *= 2;
        if (upgradeStatus[47] == 2)
            offlineYield += 0.25;
        if (upgradeStatus[48] == 2)
            starterPackLevel += 1;
        if (upgradeStatus[49] == 2)
            starterPackLevel += 1;
        if (upgradeStatus[50] == 2)
            starterPackLevel += 1;
        //Row 10
        if (upgradeStatus[51] == 2)
            jukeboxUnlocked = true;
        if (upgradeStatus[52] == 2)
            radMult += 0.005;
        if (upgradeStatus[53] == 2)
            locationBonus *= 2;
        if (upgradeStatus[54] == 2)
            offlineThorium = 0.001;
        if (upgradeStatus[55] == 2)
            prestigeIdleMultiplier *= 2;
        if (upgradeStatus[56] == 2)
            prestigeIdleMultiplier *= 2;
        if (upgradeStatus[57] == 2)
            prestigeIdleMultiplier *= 2;
        //Row 11
        if (upgradeStatus[58] == 2)
            buyAllUnlocked = true;
        if (upgradeStatus[59] == 2)
            radMult += 0.01;
        if (upgradeStatus[60] == 2)
        {
            locationBonus *= 2;
            locationTimeReduced = true;
        }
        if (upgradeStatus[61] == 2)
            offlineThorium *= 10;
        if (upgradeStatus[62] == 2)
            priceUpgrades = true;
        if (upgradeStatus[63] == 2)
            prestigeIdleMultiplier *= 3;
        if (upgradeStatus[64] == 2)
            prestigeIdleMultiplier *= 3;
        //Row 12
        if (upgradeStatus[65] == 2)
            skipCutsceneUnlocked = true;
        if (upgradeStatus[66] == 2)
            radMult += 0.015;
        if (upgradeStatus[67] == 2)
            abilityBot = true;
        if (upgradeStatus[68] == 2)
            prestigeIdleMultiplier *= 4;
        if (upgradeStatus[69] == 2)
            prestigeIdleMultiplier *= 3;
        if (upgradeStatus[70] == 2)
            prestigeIdleMultiplier *= 4;
        if (upgradeStatus[71] == 2)
            radMult += 0.02;
        if (upgradeStatus[72] == 2)
            prestigeIdleMultiplier *= 4;
        if (upgradeStatus[73] == 2)
            prestigeIdleMultiplier *= 10;
    }

    public void LoadData(GameData data)
    {
        for (int i = 0; i < data.upgradeStatus.Count; i++)
        {
            this.upgradeStatus[i] = data.upgradeStatus[i];
        }
    }

    public void SaveData(ref GameData data)
    {
        for (int i = 0; i < data.upgradeStatus.Count; i++)
        {
            data.upgradeStatus[i] = this.upgradeStatus[i];
        }
    }
}
