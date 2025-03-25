using BreakInfinity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Prestige : MonoBehaviour, IDataPersistence
{
    public static Prestige Instance;

    public PlayableDirector director;
    public Music music;
    public Camera main;
    public InputHandler targetSpawner;
    public GameObject radsText;
    public GameObject weaponDisplay;
    public GameObject skipButton;
    public Animator whiteToFade;
    public Animator noticeBox;
    public UpsAndVars prestigeShop;
    public UpgradeDatabase udb;
    public bool inPrestigeAnim = false;
    public bool inPrestigeShop = false;
    void Awake()
    {
        if (Instance == null) { Instance = this;  }
    }
    void Start()
    {
        if (Vars.Instance.rads > 0)
        {
            radsText.SetActive(true);
        }
        if (inPrestigeShop)
        {
            weaponDisplay.SetActive(false);
        }
    }
    public void Press()
    {
        inPrestigeAnim = true;
        if (prestigeShop.skipCutsceneUnlocked)
        {
            skipButton.SetActive(true);
        }
        music.StopSounds();
        director.Play();
        StartCoroutine(nameof(WaitToReset));
        
    }
    public void RushCutscene()
    {
        StopCoroutine(nameof(WaitToReset));
        PreinitShop();
        InitializeShop();
    }
    public BigDouble ThoriumToGain()
    {
        BigDouble returnVal = BigDouble.Floor(BigDouble.Pow(((Vars.Instance.totalTotalHitCount + Vars.Instance.totalHitCount) / (1000000000000.0 / prestigeShop.thoriumBonus)), (0.5))) - (Vars.Instance.rads + Vars.Instance.thorium);
        if (LocationManager.Instance.activeLocation == 6 && AvailableUpgrades.Instance.locationBonuses)
        {
            returnVal *= (1 + (LocationManager.Instance.locations[6].GetMult() * UpsAndVars.Instance.locationBonus));
        }
        if (returnVal > 0)
            return returnVal;
        return
            0;
    }

    private void VarReset()
    {
        BigDouble earnedThorium = ThoriumToGain();

        Vars.Instance.totalTotalHitCount += Vars.Instance.totalHitCount;
        Vars.Instance.totalHitCount = 0;

        Vars.Instance.totalClickTracker += Vars.Instance.clickTracker;
        Vars.Instance.clickTracker = 0;

        Vars.Instance.totalSilverClickTracker += Vars.Instance.silverClickTracker;
        Vars.Instance.silverClickTracker = 0;

        Vars.Instance.totalGoldClickTracker += Vars.Instance.goldClickTracker;
        Vars.Instance.goldClickTracker = 0;

        Vars.Instance.totalPlatClickTracker += Vars.Instance.platClickTracker;
        Vars.Instance.platClickTracker = 0;

        Vars.Instance.totalOmegaClickTracker += Vars.Instance.omegaClickTracker;
        Vars.Instance.omegaClickTracker = 0;

        Vars.Instance.hits = 0;
        Vars.Instance.silverHpsSecs = 1;
        Vars.Instance.goldHpsSecs = 15;
        Vars.Instance.platHpsSecs = 60;
        //Fix any old/broken saves
        while (Vars.Instance.hps.Count > Gun.Instance.gunNames.Count)
        {
            Vars.Instance.hps.RemoveAt(0);
        }

        while (Gun.Instance.targetPowers.Count > Gun.Instance.gunNames.Count)
        {
            Gun.Instance.targetPowers.RemoveAt(0);
        }
        while (Gun.Instance.bought.Count > Gun.Instance.gunNames.Count)
        {
            Gun.Instance.bought.RemoveAt(0);
        }
        while (AvailableUpgrades.Instance.multipliers.Count > Gun.Instance.gunNames.Count)
        {
            AvailableUpgrades.Instance.multipliers.RemoveAt(0);
        }

        for (int i = 0; i < Vars.Instance.hps.Count; i++)
        {
            Vars.Instance.hps[i] = 0;
        }

        LocationManager.Instance.activeLocation = 0;
        Gun.Instance.ResetAll();
        AvailableUpgrades.Instance.ResetAll();
        Abilities.Instance.ResetAbilities();
        


        DeleteGameObjectList(GameObject.FindGameObjectsWithTag("Target"));
        DeleteGameObjectList(GameObject.FindGameObjectsWithTag("SilverTarget"));
        DeleteGameObjectList(GameObject.FindGameObjectsWithTag("GoldTarget"));
        DeleteGameObjectList(GameObject.FindGameObjectsWithTag("PlatTarget"));
        DeleteGameObjectList(GameObject.FindGameObjectsWithTag("OmegaTarget"));
        BoardHandler.Instance.activeTargetCount = 0;

        BoardHandler.Instance.PopulateBoard();
        Vars.Instance.thorium += earnedThorium;
    }

    IEnumerator WaitToReset()
    {
        yield return new WaitForSeconds(18.23f);
        PreinitShop();
        yield return new WaitForSeconds(1f);
        InitializeShop();
    }
    public void PreinitShop()
    {
        if (prestigeShop.skipCutsceneUnlocked)
        {
            skipButton.SetActive(false);
        }
        this.inPrestigeShop = true;
        VarReset();
        //CameraViewHandler.Instance.MoveCamToPos("prestige");
    }
    public void InitializeShop()
    {
        if (prestigeShop.upgradeStatus[0] == 0)
            prestigeShop.upgradeStatus[0] = 1;
        if (prestigeShop.upgradeStatus[37] == 0)
            prestigeShop.upgradeStatus[37] = 1;
        prestigeShop.SetDisplay();
        noticeBox.SetBool("InPrestige", inPrestigeShop);
        whiteToFade.SetBool("inPrestige", inPrestigeShop);
        inPrestigeAnim = false;
        weaponDisplay.SetActive(false);
        CameraViewHandler.Instance.MoveCamToPos("prestige");
    }
    public void PanHomeEndPrestige()
    {
        inPrestigeShop = false;
        noticeBox.SetBool("InPrestige", inPrestigeShop);
        CameraViewHandler.Instance.MoveCamToPos("home");
        whiteToFade.SetBool("inPrestige", inPrestigeShop);
    }

    public void DeleteGameObjectList(List<GameObject> objects)
    {
        for (int i = 0; i < objects.Count; i++)
        {
            Destroy(objects[i]);
        }
    }
    public void DeleteGameObjectList(GameObject[] objects)
    {
        for (int i = 0; i < objects.Length; i++)
        {
            Destroy(objects[i]);
        }
    }

    public void ApplyPersistenceUpgrades()
    {
        if (Vars.Instance.rads > 0)
        {
            radsText.SetActive(true);
        }
        weaponDisplay.SetActive(true);


        if (prestigeShop.permault)
        {
            AvailableUpgrades.Instance.targetRarity = 5;
            AvailableUpgrades.Instance.bought.Add(udb.findName(9026));
            AvailableUpgrades.Instance.bought.Add(udb.findName(9027));
            AvailableUpgrades.Instance.bought.Add(udb.findName(9028));
            AvailableUpgrades.Instance.bought.Add(udb.findName(9029));
            AvailableUpgrades.Instance.SortAndDisplay();
        }
        if (prestigeShop.permabilities)
        {
            AvailableUpgrades.Instance.abilitiesUnlocked = true;
            AvailableUpgrades.Instance.bought.Add(udb.findName(2001));
            AvailableUpgrades.Instance.ActivateAllObjects();
            AvailableUpgrades.Instance.SortAndDisplay();
        }
        if (prestigeShop.starterPackLevel > 0)
        {
            Gun.Instance.bought[0] = true;
            if (prestigeShop.starterPackLevel > 2)
            {
                Gun.Instance.bought[1] = true;
                Gun.Instance.targetPowers[0] = 25;
                Gun.Instance.targetPowers[1] = 10;
            }
            else if (prestigeShop.starterPackLevel > 1)
            {
                Gun.Instance.targetPowers[0] = 10;
            }
            else
            {
                Gun.Instance.targetPowers[0] = 5;
            }
        }
        Gun.Instance.UpdatePrices();
        LocationManager.Instance.CheckForLocationReveal();
    }

    public void LoadData(GameData data)
    {
        this.inPrestigeShop = data.inPrestigeShop;
        noticeBox.SetBool("InPrestige", this.inPrestigeShop);
        whiteToFade.SetBool("inPrestige", this.inPrestigeShop);
        if (this.inPrestigeShop)
        {
            CameraViewHandler.Instance.MoveCamToPos("prestige");
        }
    }

    public void SaveData(ref GameData data)
    {
        data.inPrestigeShop = this.inPrestigeShop;
    }
}
