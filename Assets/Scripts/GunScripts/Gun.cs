using BreakInfinity;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Gun : MonoBehaviour, IDataPersistence
{
    public static Gun Instance;

    public List<BigDouble> iPrices;
    public List<BigDouble> targetPowers;
    public List<BigDouble> prices;
    public List<GameObject> gunVisuals;
    public List<string> gunNames;
    public List<GameObject> buyButtons;
    public List<TMP_Text> nameTexts;
    public List<TMP_Text> priceTexts;
    public List<TMP_Text> levelTexts;
    public List<TMP_Text> hpsTexts;
    public List<bool> bought;
    public List<BigDouble> startingPerSec;
    public AvailableUpgrades upgradeHandler;
    public UpgradeDatabase db;
    public Sprite buyable;
    public Sprite unbuyable;
    public Sprite locked;
    public Vars varRef;
    public Abilities abilities;
    private HitSound hitSound;
    private Textbox textboxRef;
    private UpsAndVars prestigeUpgradeRef;
    //Gun IDs
    //Handgun - 0
    //Sniper - 1
    //LMG - 2
    //Shotgun - 3
    //Rifle - 4
    //Grenade Launcher - 5
    //Cryo Cannon - 6
    //Laser - 7
    //Acid - 8
    //Tank - 9
    //Plane - 10
    void Awake()
    {
        if (Instance == null) { Instance = this; }
        upgradeHandler = GameObject.Find("UpgradeHandler").GetComponent<AvailableUpgrades>();
        hitSound = GameObject.Find("HitAudio").GetComponent<HitSound>();
        varRef = GameObject.Find("VarTracker").GetComponent<Vars>();
        abilities = GameObject.Find("AbilityHandler").GetComponent<Abilities>();
        textboxRef = GameObject.Find("TextboxHandler").GetComponent<Textbox>();
        prestigeUpgradeRef = GameObject.Find("PrestigeHandler").GetComponent<UpsAndVars>();
        //GameObjects
        //Guns
        gunVisuals.Add(GameObject.Find("Handgun"));
        gunVisuals.Add(GameObject.Find("Sniper"));
        gunVisuals.Add(GameObject.Find("MachineGun"));
        gunVisuals.Add(GameObject.Find("Shotgun"));
        gunVisuals.Add(GameObject.Find("Rifle"));
        gunVisuals.Add(GameObject.Find("GrenadeLauncher"));
        gunVisuals.Add(GameObject.Find("CryoCannon"));
        gunVisuals.Add(GameObject.Find("Laser"));
        gunVisuals.Add(GameObject.Find("Acid"));
        gunVisuals.Add(GameObject.Find("Tank"));
        gunVisuals.Add(GameObject.Find("Plane"));
        gunVisuals.Add(GameObject.Find("Wand"));
        gunVisuals.Add(GameObject.Find("Katana"));
        gunVisuals.Add(GameObject.Find("Hands"));
        gunVisuals.Add(GameObject.Find("Game"));
        //Gun Names
        gunNames.Add("Handgun");
        gunNames.Add("Sniper");
        gunNames.Add("LMG");
        gunNames.Add("Shotgun");
        gunNames.Add("Rifle");
        gunNames.Add("Grenade Launcher");
        gunNames.Add("Cryo Cannon");
        gunNames.Add("Laser");
        gunNames.Add("Acid");
        gunNames.Add("Tank");
        gunNames.Add("Plane");
        gunNames.Add("Magic Wand");
        gunNames.Add("Katana");
        gunNames.Add("Bare Hands");
        gunNames.Add("Game Console");
        //Button Objects
        buyButtons.Add(GameObject.Find("hgButton"));
        buyButtons.Add(GameObject.Find("snButton"));
        buyButtons.Add(GameObject.Find("mgButton"));
        buyButtons.Add(GameObject.Find("sgButton"));
        buyButtons.Add(GameObject.Find("rfButton"));
        buyButtons.Add(GameObject.Find("glButton"));
        buyButtons.Add(GameObject.Find("ccButton"));
        buyButtons.Add(GameObject.Find("lrButton"));
        buyButtons.Add(GameObject.Find("acButton"));
        buyButtons.Add(GameObject.Find("tkButton"));
        buyButtons.Add(GameObject.Find("plButton"));
        buyButtons.Add(GameObject.Find("wnButton"));
        buyButtons.Add(GameObject.Find("ktButton"));
        buyButtons.Add(GameObject.Find("hnButton"));
        buyButtons.Add(GameObject.Find("gmButton"));
        //Text Objects
        //Name Texts
        nameTexts.Add(GameObject.Find("hgButtonNameText").GetComponent<TMP_Text>());
        nameTexts.Add(GameObject.Find("snButtonNameText").GetComponent<TMP_Text>());
        nameTexts.Add(GameObject.Find("mgButtonNameText").GetComponent<TMP_Text>());
        nameTexts.Add(GameObject.Find("sgButtonNameText").GetComponent<TMP_Text>());
        nameTexts.Add(GameObject.Find("rfButtonNameText").GetComponent<TMP_Text>());
        nameTexts.Add(GameObject.Find("glButtonNameText").GetComponent<TMP_Text>());
        nameTexts.Add(GameObject.Find("ccButtonNameText").GetComponent<TMP_Text>());
        nameTexts.Add(GameObject.Find("lrButtonNameText").GetComponent<TMP_Text>());
        nameTexts.Add(GameObject.Find("acButtonNameText").GetComponent<TMP_Text>());
        nameTexts.Add(GameObject.Find("tkButtonNameText").GetComponent<TMP_Text>());
        nameTexts.Add(GameObject.Find("plButtonNameText").GetComponent<TMP_Text>());
        nameTexts.Add(GameObject.Find("wnButtonNameText").GetComponent<TMP_Text>());
        nameTexts.Add(GameObject.Find("ktButtonNameText").GetComponent<TMP_Text>());
        nameTexts.Add(GameObject.Find("hnButtonNameText").GetComponent<TMP_Text>());
        nameTexts.Add(GameObject.Find("gmButtonNameText").GetComponent<TMP_Text>());
        //Price Texts
        priceTexts.Add(GameObject.Find("hgButtonPriceText").GetComponent<TMP_Text>());
        priceTexts.Add(GameObject.Find("snButtonPriceText").GetComponent<TMP_Text>());
        priceTexts.Add(GameObject.Find("mgButtonPriceText").GetComponent<TMP_Text>());
        priceTexts.Add(GameObject.Find("sgButtonPriceText").GetComponent<TMP_Text>());
        priceTexts.Add(GameObject.Find("rfButtonPriceText").GetComponent<TMP_Text>());
        priceTexts.Add(GameObject.Find("glButtonPriceText").GetComponent<TMP_Text>());
        priceTexts.Add(GameObject.Find("ccButtonPriceText").GetComponent<TMP_Text>());
        priceTexts.Add(GameObject.Find("lrButtonPriceText").GetComponent<TMP_Text>());
        priceTexts.Add(GameObject.Find("acButtonPriceText").GetComponent<TMP_Text>());
        priceTexts.Add(GameObject.Find("tkButtonPriceText").GetComponent<TMP_Text>());
        priceTexts.Add(GameObject.Find("plButtonPriceText").GetComponent<TMP_Text>());
        priceTexts.Add(GameObject.Find("wnButtonPriceText").GetComponent<TMP_Text>());
        priceTexts.Add(GameObject.Find("ktButtonPriceText").GetComponent<TMP_Text>());
        priceTexts.Add(GameObject.Find("hnButtonPriceText").GetComponent<TMP_Text>());
        priceTexts.Add(GameObject.Find("gmButtonPriceText").GetComponent<TMP_Text>());
        //Level Texts
        levelTexts.Add(GameObject.Find("hgButtonLevelText").GetComponent<TMP_Text>());
        levelTexts.Add(GameObject.Find("snButtonLevelText").GetComponent<TMP_Text>());
        levelTexts.Add(GameObject.Find("mgButtonLevelText").GetComponent<TMP_Text>());
        levelTexts.Add(GameObject.Find("sgButtonLevelText").GetComponent<TMP_Text>());
        levelTexts.Add(GameObject.Find("rfButtonLevelText").GetComponent<TMP_Text>());
        levelTexts.Add(GameObject.Find("glButtonLevelText").GetComponent<TMP_Text>());
        levelTexts.Add(GameObject.Find("ccButtonLevelText").GetComponent<TMP_Text>());
        levelTexts.Add(GameObject.Find("lrButtonLevelText").GetComponent<TMP_Text>());
        levelTexts.Add(GameObject.Find("acButtonLevelText").GetComponent<TMP_Text>());
        levelTexts.Add(GameObject.Find("tkButtonLevelText").GetComponent<TMP_Text>());
        levelTexts.Add(GameObject.Find("plButtonLevelText").GetComponent<TMP_Text>());
        levelTexts.Add(GameObject.Find("wnButtonLevelText").GetComponent<TMP_Text>());
        levelTexts.Add(GameObject.Find("ktButtonLevelText").GetComponent<TMP_Text>());
        levelTexts.Add(GameObject.Find("hnButtonLevelText").GetComponent<TMP_Text>());
        levelTexts.Add(GameObject.Find("gmButtonLevelText").GetComponent<TMP_Text>());
        //HPS Texts
        hpsTexts.Add(GameObject.Find("hgHPSText").GetComponent<TMP_Text>());
        hpsTexts.Add(GameObject.Find("snHPSText").GetComponent<TMP_Text>());
        hpsTexts.Add(GameObject.Find("mgHPSText").GetComponent<TMP_Text>());
        hpsTexts.Add(GameObject.Find("sgHPSText").GetComponent<TMP_Text>());
        hpsTexts.Add(GameObject.Find("rfHPSText").GetComponent<TMP_Text>());
        hpsTexts.Add(GameObject.Find("glHPSText").GetComponent<TMP_Text>());
        hpsTexts.Add(GameObject.Find("ccHPSText").GetComponent<TMP_Text>());
        hpsTexts.Add(GameObject.Find("lrHPSText").GetComponent<TMP_Text>());
        hpsTexts.Add(GameObject.Find("acHPSText").GetComponent<TMP_Text>());
        hpsTexts.Add(GameObject.Find("tkHPSText").GetComponent<TMP_Text>());
        hpsTexts.Add(GameObject.Find("plHPSText").GetComponent<TMP_Text>());
        hpsTexts.Add(GameObject.Find("wnHPSText").GetComponent<TMP_Text>());
        hpsTexts.Add(GameObject.Find("ktHPSText").GetComponent<TMP_Text>());
        hpsTexts.Add(GameObject.Find("hnHPSText").GetComponent<TMP_Text>());
        hpsTexts.Add(GameObject.Find("gmHPSText").GetComponent<TMP_Text>());
        //iPrices
        iPrices.Add(15); //0
        iPrices.Add(100); //1
        iPrices.Add(1500); //2
        iPrices.Add(12000); //3 
        iPrices.Add(230000); //4
        iPrices.Add(6500000); //5
        iPrices.Add(38000000); //6
        iPrices.Add(860000000); //7
        iPrices.Add(9200000000); //8
        iPrices.Add(97000000000); //9
        iPrices.Add(1000000000000); //10
        iPrices.Add(52400000000000); //11
        iPrices.Add(1200000000000000); //12
        iPrices.Add(78000000000000000); //13
        iPrices.Add(1000000000000000000); //14

        //Prices
        for (int i = 0; i < gunVisuals.Count; i++)
        {
            prices.Add(0);
        }
        startingPerSec.Add(0.2); //75 Seconds
        startingPerSec.Add(2); //50 Seconds
        startingPerSec.Add(10); //150 Seconds
        startingPerSec.Add(55); //300 Seconds
        startingPerSec.Add(383); //600 Seconds
        startingPerSec.Add(3611); //1800 Seconds
        startingPerSec.Add(7037); //5400 Seconds
        startingPerSec.Add(39814); //21600
        startingPerSec.Add(106481); //86400
        startingPerSec.Add(224530); //432000
        startingPerSec.Add(1000000); //1000000
        startingPerSec.Add(17400000); //3000000
        startingPerSec.Add(120000000); //10000000
        startingPerSec.Add(1560000000); //50000000
        startingPerSec.Add(6600000000); //150000000
    }
    void Start()
    {
        for (int i = bought.Count; i < gunVisuals.Count; i++)
        {
            bought.Add(false);
        }
        for (int i = targetPowers.Count; i < gunVisuals.Count; i++)
        {
            targetPowers.Add(0);
        }
        for (int i = 0; i < bought.Count; i++)
        {
            if (targetPowers[i] > 0)
                bought[i] = true;
        }
        UpdatePrices();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateFunctions();
        CheckUnlocked();
    }
    public void UpdatePrices()
    {
        if (varRef.buyMultiplier == 1)
        {
            for (int i = 0; i < gunVisuals.Count; i++)
                SinglePrice(i);
        }
        else if (varRef.buyMultiplier == 10)
        {
            for (int i = 0; i < gunVisuals.Count; i++)
                TenPrice(i);
        }
        else if (varRef.buyMultiplier == 100)
        {
            for (int i = 0; i < gunVisuals.Count; i++)
                HundPrice(i);
        }
    }
    public void shopClick(int id)
    {
        if (bought[id])
        {
            if (varRef.hits >= prices[id])
            {
                varRef.hits -= prices[id];
                hitSound.source.PlayOneShot(hitSound.click, 1f);
                if (varRef.buyMultiplier == 1)
                    targetPowers[id] += 1;
                else if (varRef.buyMultiplier == 10)
                    targetPowers[id] += 10;
                else if (varRef.buyMultiplier == 100)
                    targetPowers[id] += 100;
            }
        }
        else if (varRef.hits >= prices[id])
        {
            varRef.hits -= prices[id];
            hitSound.source.PlayOneShot(hitSound.click, 1f);
            bought[id] = true;
            if (varRef.buyMultiplier == 1)
                targetPowers[id] += 1;
            else if (varRef.buyMultiplier == 10)
                targetPowers[id] += 10;
            else if (varRef.buyMultiplier == 100)
                targetPowers[id] += 100;
            gunVisuals[id].SetActive(true);
        }
        LocationManager.Instance.CheckForLocationReveal();
        UpdatePrices();
        UpdateFunctions();
        testForUnlock(id);
        SetShow(id);
    }
    public void CheckUnlocked()
    {
        if (!bought[0])
        {
            gunVisuals[0].SetActive(false);
            hpsTexts[0].text = "";
        }
        else
        {
            gunVisuals[0].SetActive(true);
            hpsTexts[0].text = varRef.HpsAbbr(HandgunPlusCount()) + " hits/second";
            varRef.hps[0] = HandgunPlusCount();
        }
        for (int id = 1; id < gunVisuals.Count; id++)
        {
            if (!bought[id])
            {
                gunVisuals[id].SetActive(false);
                hpsTexts[id].text = "";
            }
            else
            {
                gunVisuals[id].SetActive(true);
                hpsTexts[id].text = varRef.HpsAbbr(PlusCount(id)) + " hits/second";
                varRef.hps[id] = PlusCount(id);
            }
        }
    }
    public BigDouble PlusCount(int id)
    {
        BigDouble amount;
        amount = targetPowers[id] * (upgradeHandler.bonusMult) * (1 + (varRef.rads * prestigeUpgradeRef.radMult));
        if (!abilities.abilityActive[4])
        {
            amount *= (startingPerSec[id] * upgradeHandler.multipliers[id]);
        }
        else
        {
            int bestId = bought.LastIndexOf(true);
            amount *= (startingPerSec[bestId] * upgradeHandler.multipliers[bestId]);
        }

        if (LocationManager.Instance.activeLocation == 0 && upgradeHandler.locationBonuses)
        {
            amount *= (1 + prestigeUpgradeRef.locationBonus * LocationManager.Instance.locations[0].GetMult());
        }

        amount *= prestigeUpgradeRef.prestigeIdleMultiplier;

        return amount;
    }
    public BigDouble HandgunPlusCount()
    {
        BigDouble amount;
        amount = (targetPowers[0] + (upgradeHandler.hgCountMult * TotalCount()) * (upgradeHandler.bonusMult) * (1 + (varRef.rads * 0.01)));
        if (!abilities.abilityActive[4])
        {
            amount *= (startingPerSec[0] * upgradeHandler.multipliers[0]);
        }
        else
        {
            int bestId = bought.LastIndexOf(true);
            amount *= (startingPerSec[bestId] * upgradeHandler.multipliers[bestId]);
        }

        if (LocationManager.Instance.activeLocation == 0 && upgradeHandler.locationBonuses)
        {
            amount *= (1 + prestigeUpgradeRef.locationBonus * LocationManager.Instance.locations[1].GetMult());
        }

        amount *= prestigeUpgradeRef.prestigeIdleMultiplier;

        return amount;
    }
    public BigDouble TotalCount()
    {
        BigDouble total = 0;
        for (int i = 0; i < gunVisuals.Count; i++)
        {
            total += targetPowers[i];
        }
        return total;
    }
    public void UpdateFunctions()
    {
        for (int id = 0; id < gunVisuals.Count; id++)
        {
            if (varRef.totalHitCount < iPrices[id])
            {
                buyButtons[id].GetComponent<SpriteRenderer>().sprite = locked;
            }
            else if (varRef.hits < prices[id])
            {
                buyButtons[id].GetComponent<SpriteRenderer>().sprite = unbuyable;
            }
            else
            {
                buyButtons[id].GetComponent<SpriteRenderer>().sprite = buyable;
            }
            if (varRef.totalHitCount >= iPrices[id])
            {
                nameTexts[id].text = gunNames[id];
                priceTexts[id].text = varRef.PriceAbbr(prices[id]) + "";
                levelTexts[id].text = targetPowers[id] + "";
            }
            else
            {
                nameTexts[id].text = "??????";
                priceTexts[id].text = "???";
                levelTexts[id].text = "";
            }
        }
    }
    public void SinglePrice(int id)
    {
        BigDouble priceMult = BigDouble.Pow(1.2, targetPowers[id]) * prestigeUpgradeRef.priceBonus;
        if (LocationManager.Instance.activeLocation == 3 && AvailableUpgrades.Instance.locationBonuses)
        {
            priceMult *= (1 - (LocationManager.Instance.locations[3].GetMult() * UpsAndVars.Instance.locationBonus));
        }
        if (prestigeUpgradeRef.priceUpgrades)
        {
            priceMult *= (1 - (upgradeHandler.bought.Count * 0.0006));
        }

        prices[id] = iPrices[id] * priceMult;
    }
    public void TenPrice(int id)
    {
        prices[id] = 0;
        for (int i = 0; i <= 9; i++)
        {
            BigDouble priceMult = BigDouble.Pow(1.2, targetPowers[id] + i) * prestigeUpgradeRef.priceBonus;
            if (LocationManager.Instance.activeLocation == 3 && AvailableUpgrades.Instance.locationBonuses)
            {
                priceMult *= (1 - (LocationManager.Instance.locations[3].GetMult() * UpsAndVars.Instance.locationBonus));
            }
            if (prestigeUpgradeRef.priceUpgrades)
            {
                priceMult *= (1 - (upgradeHandler.bought.Count * 0.0006));
            }

            prices[id] += iPrices[id] * priceMult;
        }
        
    }
    public void HundPrice(int id)
    {
        prices[id] = 0;
        for (int i = 0; i <= 99; i++)
        {
            BigDouble priceMult = BigDouble.Pow(1.2, targetPowers[id] + i) * prestigeUpgradeRef.priceBonus;
            if (LocationManager.Instance.activeLocation == 3 && AvailableUpgrades.Instance.locationBonuses)
            {
                priceMult *= (1 - (LocationManager.Instance.locations[3].GetMult() * UpsAndVars.Instance.locationBonus));
            }
            if (prestigeUpgradeRef.priceUpgrades)
            {
                priceMult *= (1 - (upgradeHandler.bought.Count * 0.0006));
            }

            prices[id] += iPrices[id] * priceMult;
        }
    }
    public void testForUnlock(int id)
    {
        if (id != 0)
        {
            if (targetPowers[id] > 0)
            {
                upgradeHandler.Unlock(db.findName((id * 10)));
            }
            if (targetPowers[id] >= 5)
            {
                upgradeHandler.Unlock(db.findName((id * 10) + 1));
            }
            if (targetPowers[id] >= 25)
            {
                upgradeHandler.Unlock(db.findName((id * 10) + 2));
            }
            if (targetPowers[id] >= 50)
            {
                upgradeHandler.Unlock(db.findName((id * 10) + 3));
            }
            if (targetPowers[id] >= 100)
            {
                upgradeHandler.Unlock(db.findName((id * 10) + 4));
            }
            if (targetPowers[id] >= 150)
            {
                upgradeHandler.Unlock(db.findName((id * 10) + 5));
            }
            if (targetPowers[id] >= 200)
            {
                upgradeHandler.Unlock(db.findName((id * 10) + 6));
            }
            if (targetPowers[id] >= 250)
            {
                upgradeHandler.Unlock(db.findName((id * 10) + 7));
            }
            if (targetPowers[id] >= 300)
            {
                upgradeHandler.Unlock(db.findName((id * 10) + 8));
            }
            if (targetPowers[id] >= 350)
            {
                upgradeHandler.Unlock(db.findName((id * 10) + 9));
            }
        }
        else
        {
            if (targetPowers[id] > 0)
            {
                upgradeHandler.Unlock(db.findName((id * 10)));
                upgradeHandler.Unlock(db.findName((id * 10) + 1));
            }
            if (targetPowers[id] >= 10)
            {
                upgradeHandler.Unlock(db.findName((id * 10) + 2));
            }
            if (targetPowers[id] >= 25)
            {
                upgradeHandler.Unlock(db.findName((id * 10) + 3));
            }
            if (targetPowers[id] >= 50)
            {
                upgradeHandler.Unlock(db.findName((id * 10) + 4));
            }
            if (targetPowers[id] >= 100)
            {
                upgradeHandler.Unlock(db.findName((id * 10) + 5));
            }
            if (targetPowers[id] >= 150)
            {
                upgradeHandler.Unlock(db.findName((id * 10) + 6));
            }
            if (targetPowers[id] >= 200)
            {
                upgradeHandler.Unlock(db.findName((id * 10) + 7));
            }
            if (targetPowers[id] >= 250)
            {
                upgradeHandler.Unlock(db.findName((id * 10) + 8));
            }
            if (targetPowers[id] >= 300)
            {
                upgradeHandler.Unlock(db.findName((id * 10) + 9));
            }
        }
    }
    public void SetShow(int i)
    {
        textboxRef.SetTitleText(gunNames[i]);
        textboxRef.SetTitleColor(Color.white);
        
        if(i == 0)
            textboxRef.SetFullDescriptionText(targetPowers[i] + " handguns are generating " + varRef.HpsAbbr((HandgunPlusCount() / targetPowers[i])) + " hits/second each, totaling " + varRef.HpsAbbr(HandgunPlusCount()) + " hits/second");
        else
            textboxRef.SetFullDescriptionText(targetPowers[i] + " " + gunNames[i] + "s are generating " + varRef.HpsAbbr((PlusCount(i) / targetPowers[i])) + " hits/second each, totaling " + varRef.HpsAbbr(PlusCount(i)) + " hits/second");
        textboxRef.SetFullDescriptionColor(new Color(0.2f,0.2f,0.2f));
        if (i != 0)
            textboxRef.SetCostText(BigDouble.Round((PlusCount(i) / Vars.Instance.totalHps * 10000)) / 100.0 + "% of total HPS");
        else
            textboxRef.SetCostText(BigDouble.Round((HandgunPlusCount() / Vars.Instance.totalHps * 10000)) / 100.0 + "% of total HPS");
        textboxRef.SetCostColor(Color.white);
        textboxRef.ShowBox();
    }
    public void ResetAll()
    {
        for (int i = 0; i < targetPowers.Count; i++)
        {
            targetPowers[i] = 0;
            
        }
        for (int i = 0; i < bought.Count; i++)
        {
            bought[i] = false;
        }
        for (int i = 0; i < gunVisuals.Count; i++)
        {
            gunVisuals[i].SetActive(false);
        }
        for (int i = 0; i < buyButtons.Count; i++)
        {
            buyButtons[i].GetComponent<SpriteRenderer>().sprite = locked;
        }
    }
    public void LoadData(GameData data)
    {
        this.targetPowers = data.targetPowers;
        for (int i = 0; i < bought.Count; i++)
        {
            if (targetPowers[i] > 0)
                bought[i] = true;
        }
    }

    public void SaveData(ref GameData data)
    {
        data.targetPowers = this.targetPowers;
    }
}
