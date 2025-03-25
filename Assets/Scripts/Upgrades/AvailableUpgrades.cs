using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BreakInfinity;
using System.Linq;

public class AvailableUpgrades : MonoBehaviour, IDataPersistence
{
    public static AvailableUpgrades Instance;
    public UpgradeDatabase db;
    public GameObject upgradeObject;
    public GameObject upgradePrefab;
    public List<GameObject> spawnedObjects;
    public List<GameObject> activatables;

    [SerializeField] public List<UpgradeObject> available;
    [SerializeField] public List<UpgradeObject> bought;

    public List<int> availablePins;
    public List<int> boughtPins;
    //Generic Upgrades
    public BigDouble bonusMult = 1;

    public List<double> multipliers;
    //Handgun Upgrades
    public double targetLevel = 1;
    public double tgMult = 1;
    public double tgMultPercent = 0;
    public double tgHpsPercent = 0;
    public double hgCountMult = 0;
    //Weather Upgrades
    public bool combosUnlocked = false;

    public bool locationBonuses = false;
    public bool abilitiesUnlocked = false;
    public bool nukeUnlocked = false;
    public int targetRarity = 0;

    void Awake()
    {
        if (Instance == null) { Instance = this; }
    }
    void Start()
    {
        for (int i = multipliers.Count; i < Gun.Instance.gunVisuals.Count; i++)
        {
            multipliers.Add(1);
        }
        foreach (UpgradeObject obj in bought)
        {
            ApplyBonusForUpgrade(obj);
        }
        SortAndDisplay();
        ActivateAllObjects();
    }

    public void Unlock(UpgradeObject i)
    {
        if (i == null)
        {
            Debug.LogError("Attempted unlock of null object.");

        }
        else if (!(hasPIN(i.pin) || hasPINBought(i.pin)))
        { 
            available.Add(i);
            SortAndDisplay();
        }
    }

    public void SortAndDisplay()
    {
        for(int i = 0; i < available.Count; i++)
        {
            bool isSwapped = false;
            for(int j = 0; j < available.Count - 1; j++)
            {
                if(available[j].price > available[j + 1].price)
                {
                    var temp = available[j];
                    available[j] = available[j+1];
                    available[j+1] = temp;
                    isSwapped = true;
                }
            }
            if(!isSwapped)
            {
                break;
            }
        }
        BoardHandler.Instance.SetTargetOddsDisplay();
        IterateAndDisplay();
    }

    public void BuyAll()
    {
        bool playSound = false;
        SortAndDisplay();
        while(available.Count !=0 && Vars.Instance.hits >= available[0].price)
        {
            playSound = true;
            Buy(available[0]);   
        }
        if(playSound)
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.upgradeBuy, 1f);
        else
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.cantUse, 1f);

    }

    void IterateAndDisplay()
    {
        while (spawnedObjects.Count > available.Count)
        {
            Destroy(spawnedObjects.ElementAt(spawnedObjects.Count - 1));
            spawnedObjects.Remove(spawnedObjects.ElementAt(spawnedObjects.Count - 1));
        }
        for (int i = spawnedObjects.Count; i < 12; i++)
        {
            if (i > 12)
            {
                break;
            }
            if (i >= available.Count)
            {
                break;
            }
            GameObject obj = Instantiate(upgradePrefab, new Vector3(3.5f + ((i % 2) * 1f), 3f - Mathf.Floor(i / 2.0f), 0f), Quaternion.identity, upgradeObject.transform);
            obj.GetComponent<Upgrade>().upgSlotNum = i;
            spawnedObjects.Add(obj);
        }
    }

    public UpgradeObject GetNextUpgrade(int id)
    {
        return available[id];
    }

    public bool hasPIN(int i)
    {
        foreach(var u in available)
        {
            if(u.pin == i)
            {
                return true;
            }
        }
        return false;
    }

    public bool hasPINBought(int i)
    {
        foreach(var u in bought)
        {
            if(u.pin == i)
            {
                return true;
            }
        }
        return false;
    }

    public void Buy(UpgradeObject i)
    {
        bought.Add(i);
        ApplyBonusForUpgrade(i);
        available.Remove(i);
        Destroy(spawnedObjects.ElementAt(spawnedObjects.Count - 1));
        spawnedObjects.Remove(spawnedObjects.ElementAt(spawnedObjects.Count - 1));
        SortAndDisplay();

        Gun.Instance.UpdatePrices();
    }

    public void ApplyBonusForUpgrade(UpgradeObject i)
    {
        //Gun Upgrades
        for (int j = 1; j < Gun.Instance.gunVisuals.Count; j++)
        {
            if (i.pin / 10 == j)
            {
                if (i.pin % 10 == 9)
                    multipliers[j] *= 20;
                else
                    multipliers[j] *= 2;
            }
        }
        //Handgun/Target Upgrades
        if (i.pin == 0 || i.pin == 1 || i.pin == 2)
        {
            tgMult *= 2;
            multipliers[0] *= 2;
        }
        else if (i.pin == 3)
        {
            tgMultPercent = 1;
            hgCountMult = 1;
        }
        else if (i.pin == 4)
        {
            tgMultPercent *= 2;
            hgCountMult *= 2;
        }
        else if (i.pin == 5)
        {
            tgMultPercent *= 2;
            hgCountMult *= 2;
        }
        else if (i.pin == 6)
        {
            tgMultPercent *= 2;
            hgCountMult *= 2;
        }
        else if (i.pin == 7)
        {
            tgMultPercent *= 2;
            hgCountMult *= 2;
        }
        else if (i.pin == 8)
        {
            tgMultPercent *= 2;
            hgCountMult *= 2;
        }
        else if (i.pin == 9)
        {
            tgMultPercent *= 50;
            hgCountMult *= 50;
        }
        //Bullet Upgrades
        else if (i.pin == 1000 || i.pin == 1001 || i.pin == 1002)
        {
            bonusMult += 0.01;
        }
        else if (i.pin == 1003 || i.pin == 1004 || i.pin == 1005 || i.pin == 1006 || i.pin == 1007 || i.pin == 1008 || i.pin == 1009)
        {
            bonusMult += 0.02;
        }
        //World Map Upgrades
        else if (i.pin == 2000)
        {
            locationBonuses = true;
            ActivateAllObjects();
        }
        else if (i.pin == 2001)
        {
            abilitiesUnlocked = true;
            ActivateAllObjects();
        }
        else if (i.pin == 5555)
        {
            combosUnlocked = true;
        }
        //Target Upgrades
        else if (i.pin == 9000 || i.pin == 9001 || i.pin == 9002 || i.pin == 9003)
        {
            targetLevel++;
        }
        else if (i.pin == 9004 || i.pin == 9005 || i.pin == 9006 || i.pin == 9007)
        {
            Vars.Instance.silverHpsSecs += 0.25;
        }
        else if (i.pin == 9008 || i.pin == 9009 || i.pin == 9010 || i.pin == 9011)
        {
            Vars.Instance.goldHpsSecs += 5;
        }
        else if (i.pin == 9012 || i.pin == 9013 || i.pin == 9014 || i.pin == 9015)
        {
            Vars.Instance.platHpsSecs += 30;
        }
        else if (i.pin == 9016)
        {
            tgHpsPercent = 0.01;
        }
        else if (i.pin == 9017)
        {
            tgHpsPercent = 0.02;
        }
        else if (i.pin == 9018)
        {
            tgHpsPercent = 0.05;
        }
        else if (i.pin == 9019)
        {
            tgHpsPercent = 0.08;
        }
        else if (i.pin == 9020)
        {
            tgHpsPercent = 0.11;
        }
        else if (i.pin == 9021)
        {
            tgHpsPercent = 0.15;
        }
        else if (i.pin == 9022)
        {
            tgHpsPercent = 0.18;
        }
        else if (i.pin == 9023)
        {
            tgHpsPercent = 0.22;
        }
        else if (i.pin == 9024)
        {
            tgHpsPercent = 0.25;
        }
        else if (i.pin == 9025)
        {
            tgHpsPercent = 0.30;
        }
        else if (i.pin == 9026)
        {
            targetRarity++;
        }
        else if (i.pin == 9027)
        {
            targetRarity++;
        }
        else if (i.pin == 9028)
        {
            targetRarity++;
        }
        else if (i.pin == 9029)
        {
            targetRarity++;
        }
        else if (i.pin == 9030)
        {
            targetRarity++;
        }
        else if (i.pin == 9999)
        {
            nukeUnlocked = true;
            ActivateAllObjects();
        }
    }

    public void ActivateAllObjects()
    {
        activatables[0].GetComponent<ToggleableButton>().SetLocked(!locationBonuses);
        activatables[1].GetComponent<ToggleableButton>().SetLocked(!abilitiesUnlocked);
        activatables[2].SetActive(nukeUnlocked);
    }

    public void ResetAll()
    {
        targetLevel = 1;
        tgMult = 1;
        tgMultPercent = 0;
        tgHpsPercent = 0;
        hgCountMult = 0;
        bonusMult = 1;
        targetRarity = 0;
        abilitiesUnlocked = false;
        locationBonuses = false;
        nukeUnlocked = false;
        combosUnlocked = false;
        bought = new List<UpgradeObject> { };
        available = new List<UpgradeObject> { };

        activatables[0].GetComponent<ToggleableButton>().SetLocked(true);
        activatables[1].GetComponent<ToggleableButton>().SetLocked(true);
        activatables[2].SetActive(nukeUnlocked);

        for (int i = 0; i < multipliers.Count; i++)
        {
            multipliers[i] = 1;
        }
    }

    public void LoadData(GameData data)
    {
        ResetAll();
        //Upgrades
        for(int i = 0; i < data.availablePins.Count; i++)
        {
            this.available.Add(db.findName(data.availablePins[i]));
        }
        for(int i = 0; i < data.boughtPins.Count; i++)
        {
            this.bought.Add(db.findName(data.boughtPins[i]));
        }
        ActivateAllObjects();
    }

    public void SaveData(ref GameData data)
    {
        //Upgrades
        data.availablePins = new List<int>();
        for(int i = 0; i < available.Count; i++)
        {
            data.availablePins.Add(available[i].pin);
        }
        data.boughtPins = new List<int>();
        for(int i = 0; i < bought.Count; i++)
        {
            data.boughtPins.Add(bought[i].pin);
        }
    }


}
