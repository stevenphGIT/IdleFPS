using BreakInfinity;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocationManager : MonoBehaviour, IDataPersistence
{
    public static LocationManager Instance;

    public LocationHolder[] holders;
    public Location[] locations = {
        new(0, "City", "Boosts idle hits by ", 0, new Color(0.1f, 0f, 0.36f), new Color(0.55f, 0.55f, 0.55f), true, true, 1),
        new(1, "Forest", "Boosts active hits by ", 500, new Color(0f, 0.5f, 0f), new Color(0f, 0.5f, 0f), true, false, 10),
        new(2, "Tundra", "Boosts hits gained while offline by ", 500, new Color(0.5f, 0.8f, 1f), new Color(0f, 0.51f, 0.59f), 5),
        new(3, "Ocean", "Reduces cost of weapons by ", 500, new Color(0f, 0f, 1f), new Color(0f, 0f, 1f), 0.2),
        new(4, "Sahara", "Increases duration of abilities by ", 500, new Color(1f, 0.78f, 0f), new Color(1f, 0.52f, 0.42f),0.8),
        new(5, "Space", "Combo hit bonus percentages are increased by ", 500, new Color(0.3f, 0.3f, 0.3f), new Color(0.3f, 0.3f, 0.3f),10),
        new(6, "Blossoms", "Thorium gained on nuclear reset is increased by ", 500, new Color(1f, 0.5f, 1f), new Color(1f, 0.5f, 1f),1),
        new(7, "Headquarters", "Your final challenge...", 500, new Color(1f, 0f, 0f), new Color(1f, 1f, 1f),1)
    };
    public int startIndex = 0;
    public double percentBonus = 0;
    public int activeLocation;

    public float cooldown = 0;
    public float cooldownMax = 300;
    public TMP_Text cooldownText;
    private float timer = 10;
    public GameObject leftArrow, rightArrow;

    public SpriteRenderer background;
    public GameObject snowman;

    public Sprite[] sprites;

    public Sprite up, notUp, max;

    public SpriteRenderer[] primaryColors;
    public SpriteRenderer[] secondaryColors;
    public SpriteRenderer[] darkColors;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        SetBackground();
        SetLocationPrices();
        SetLocationTexts();
        CheckForLocationReveal();
    }
    private void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            cooldownText.text = "Cooldown:\n" + Mathf.Floor(cooldown) + " seconds";
        }
        else
        {
            cooldownText.text = "";
        }
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = 10;
            SetLocationPrices();
            SetLocationTexts();
            CheckForLocationReveal();
        }
    }
    public void CheckForLocationReveal()
    {
        if (Vars.Instance.totalHps > 550)
        {
            locations[2].SetRevealed(true);
        }
        if (Gun.Instance.TotalCount() >= 100)
        {
            locations[3].SetRevealed(true);
        }
        if (Vars.Instance.abilitiesUsed >= 20)
        {
            locations[4].SetRevealed(true);
        }
        if (Vars.Instance.comboRecord >= 35)
        {
            locations[5].SetRevealed(true);
        }
        if (Vars.Instance.rads >= 100)
        {
            locations[6].SetRevealed(true);
        }
        SetLocationTexts();
    }
    private bool CanLevel(int id)
    {
        int currentLevel = locations[id].GetLevel() + 1;

        if (currentLevel >= 11)
        {
            return false;
        }

        if (!locations[id].IsOwned())
        {
            return false;
        }

        if(id == 7)
        {
            return false;
        }

        return true;
    }
    public BigDouble DistanceToLevelUp(int id)
    {
        BigDouble distance = -1;
        int currentLevel = locations[id].GetLevel() + 1;
        
        if (id == 0)
        {
            distance = (50 * currentLevel * Math.Pow(15, currentLevel)) - (Vars.Instance.totalHps);
        }
        else if (id == 1)
        {
            distance = (100 * currentLevel * Math.Pow(1.5, currentLevel)) - (Vars.Instance.totalClickTracker + Vars.Instance.clickTracker);
        }
        else if (id == 2)
        {
            distance = (100 * currentLevel * Math.Pow(1.5, currentLevel)) - (Vars.Instance.totalClickTracker + Vars.Instance.clickTracker);
        }
        else if (id == 3)
        {
            distance = (100 * currentLevel * Math.Pow(1.5, currentLevel)) - (Vars.Instance.totalClickTracker + Vars.Instance.clickTracker);
        }
        else if (id == 4)
        {
            distance = (45 + ((currentLevel - 1) * 10)) - (Vars.Instance.abilitiesUsed);
        }
        else if (id == 5)
        {
            distance = (35 + (currentLevel * 15)) - (Vars.Instance.comboRecord);
        }
        else if (id == 6)
        {
            distance = (100 * currentLevel * Math.Pow(8, currentLevel)) - (Vars.Instance.rads);
        }
        return distance;
    }

    public void SetLocationTexts()
    {
        if (startIndex == locations.Length - 3)
        {
            rightArrow.SetActive(false);
        }
        else
        {
            rightArrow.SetActive(true);
        }
        if (startIndex == 0)
        {
            leftArrow.SetActive(false);
        }
        else
        {
            leftArrow.SetActive(true);
        }

        for (int i = 0; i < holders.Length; i++)
        {
            int index = startIndex + i;
            if (locations[index].ShouldReveal())
            {
                if (!locations[index].IsOwned())
                {
                    holders[i].upgradeIcon.gameObject.SetActive(false);
                }
                else
                {
                    holders[i].upgradeIcon.gameObject.SetActive(true);
                }
                if (CanLevel(index))
                {
                    if (DistanceToLevelUp(index) <= 0)
                        holders[i].upgradeIcon.sprite = up;
                    else
                        holders[i].upgradeIcon.sprite = notUp;
                }
                else
                {
                    holders[i].upgradeIcon.sprite = max;
                }
                
                percentBonus = PercentBonus(index);
                holders[i].locationTitle.text = locations[index].GetName();
                holders[i].locationDescription.text = "<color=#FF0000>Location Effect:</color>\n" + locations[index].GetDesc() + PercentBonus(index) + "%";
                holders[i].locationLevelText.text = "Level " + (locations[index].GetLevel() + 1);
                holders[i].holderColor.color = locations[index].GetColor();
                if (locations[index].IsOwned() && activeLocation != startIndex + i)
                {
                    holders[i].locationButtonText.text = "Travel to " + locations[index].GetName() + "?";
                    holders[i].locationButtonText.color = Color.white;
                }
                else if (locations[index].IsOwned())
                {
                    holders[i].locationButtonText.text = "You are here!";
                    holders[i].locationButtonText.color = Color.green;
                }
                else
                {
                    holders[i].locationButtonText.text = "Buy for " + Vars.Instance.Abbr(locations[index].GetCost()) + "?";
                    holders[i].locationButtonText.color = Color.yellow;
                }
            }
            else
            {
                holders[i].locationTitle.text = "???????";
                holders[i].locationDescription.text = "<color=#FF0000>Location Effect:</color>\n??????????";
                holders[i].locationLevelText.text = "";
                switch (index)
                {
                    case 2:
                        holders[i].locationButtonText.text = "Reach 550 hits per second to reveal";
                        break;
                    case 3:
                        holders[i].locationButtonText.text = "Own 100 total weapons to reveal";
                        break;
                    case 4:
                        holders[i].locationButtonText.text = "Use 20 abilities to reveal";
                        break;
                    case 5:
                        holders[i].locationButtonText.text = "Achieve a 35-target combo to reveal";
                        break;
                    case 6:
                        holders[i].locationButtonText.text = "Reach 100 <color=#00FF00><sprite index=0>rads</color> to reveal";
                        break;
                    case 7:
                        holders[i].locationButtonText.text = "?????";
                        break;
                }
                holders[i].locationButtonText.color = Color.white;
                holders[i].holderColor.color = new Color(0.5f, 0.5f, 0.5f);
                holders[i].upgradeIcon.gameObject.SetActive(false);
            }
        }
    }
    public void ClickLocationButton(int buttonId)
    {
        int id = (startIndex + buttonId);
        if (!locations[id].ShouldReveal())
        {
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.cantUse);
        }
        else if (!locations[id].IsOwned())
        {
            if (locations[id].GetCost() < Vars.Instance.hits)
            {
                locations[id].SetOwned(true);
                Vars.Instance.hits -= locations[id].GetCost();
                HitSound.Instance.source.PlayOneShot(HitSound.Instance.upgradeBuy);
                SetLocationPrices();
                SetLocationTexts();
            }
            else
            {
                HitSound.Instance.source.PlayOneShot(HitSound.Instance.cantUse);
            }
        }
        else if (locations[id].IsOwned() && activeLocation != id)
        {
            Travel(id);
        }
    }
    private void SetLocationPrices()
    {
        foreach (Location loc in locations)
        {
            loc.SetCost(500 * Math.Pow(10, (OwnedCount() - 1) * 2));
        }
    }
    private int OwnedCount()
    {
        int count = 0;
        foreach (Location loc in locations) 
        {
            if(loc.IsOwned())
                count++;
        }
        return count;
    }
    public void Travel(int id)
    {
        if (cooldown > 0)
        {
            FloatingText.Instance.PopText("You can't change locations for\n " + Math.Ceiling(cooldown) + " seconds", new Color32(255, 0, 0, 255), 0);
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.cantUse);
        }
        else
        {
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.click);
            activeLocation = id;
            SetLocationTexts();
            Music.Instance.StopSounds();
            Music.Instance.ChooseSong();
            if (UpsAndVars.Instance.locationTimeReduced)
                cooldown = 20;
            else
                cooldown = 300;
        }
        Gun.Instance.UpdatePrices();
        SetBackground();
    }
    public void SetShowMap()
    {
        Textbox.Instance.SetTitleText(locations[activeLocation].GetName());
        Textbox.Instance.SetTitleColor(locations[activeLocation].GetColor());
        Textbox.Instance.SetFullDescriptionText("<color=#FF0000>Location Effect:</color>\n<color=#FFFFFF>" + locations[activeLocation].GetDesc() + PercentBonus(activeLocation) + "%</color>");
        Textbox.Instance.SetCostText("");
        Textbox.Instance.ShowBox(-1.5f, -1.5f);
    }
    public void SetShowLevel(int holderId)
    {
        int locId = holderId + startIndex;
        BigDouble distance = DistanceToLevelUp(locId);
        if (CanLevel(locId))
        {
            if (distance <= 0)
            {
                Textbox.Instance.SetTitleText("Level Up?");
                Textbox.Instance.SetTitleColor(Color.green);
                Textbox.Instance.SetFullDescriptionColor(Color.white);
                Textbox.Instance.SetFullDescriptionText((0.1 * locations[locId].GetLevel() + 1).ToString("F1") + "x" + " ----> " + (0.1 * (locations[locId].GetLevel() + 1) + 1).ToString("F1") + "x" + "\n\nPermanent upgrade!");
                Textbox.Instance.SetCostColor(Color.cyan);
                Textbox.Instance.SetCostText((0.1 * locations[locId].GetLevel() + 1).ToString("F1") + "x");
            }
            else
            {
                Textbox.Instance.SetTitleText("Can't Level");
                Textbox.Instance.SetTitleColor(Color.red);
                Textbox.Instance.SetFullDescriptionColor(Color.white);
                Textbox.Instance.SetFullDescriptionText(ProgressText(distance, locId));
                Textbox.Instance.SetCostColor(Color.cyan);
                Textbox.Instance.SetCostText((0.1 * locations[locId].GetLevel() + 1).ToString("F1") + "x");
            }
        }
        else
        {
            Textbox.Instance.SetTitleText("Max Level");
            Textbox.Instance.SetTitleColor(Color.cyan);
            Textbox.Instance.SetFullDescriptionColor(Color.white);
            Textbox.Instance.SetFullDescriptionText("This location can not be upgraded further.");
            Textbox.Instance.SetCostColor(Color.cyan);
            Textbox.Instance.SetCostText((0.1 * locations[locId].GetLevel() + 1).ToString("F1") + "x");
        }
        Textbox.Instance.ShowBox();
    }
    private string ProgressText(BigDouble dist, int id)
    {
        switch (id)
        {
            case 0:
                return "You need to have " + Vars.Instance.TotalAbbr(dist) + " more hits per second to level up this location";
            case 1:
                return "You need to click " + Vars.Instance.TotalAbbr(dist) + " more targets to level up this location";
            case 2:
                return "You need to click " + Vars.Instance.TotalAbbr(dist) + " more targets to level up this location";
            case 3:
                return "You need to click " + Vars.Instance.TotalAbbr(dist) + " more targets to level up this location";
            case 4:
                return "You need to use " + Vars.Instance.TotalAbbr(dist) + " more abilities to level up this location";
            case 5:
                return "You need to reach a " + Vars.Instance.TotalAbbr(dist + Vars.Instance.comboRecord) + "-target combo to level up this location";
            case 6:
                return "You need to reach " + Vars.Instance.TotalAbbr(dist) + " rads to level up this location";
            default:
                return "ERROR: Progress text not defined in LocationManager.cs";
        }
    }
    
    public void ClickIcon(int iconId)
    {
        int id = startIndex + iconId;
        if (CanLevel(id) && DistanceToLevelUp(id) <= 0)
        {
            IncrementLevel(id);
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.upgradeBuy);
        }
        else
        {
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.cantUse);
        }
        SetLocationTexts();
    }
    void IncrementLevel(int id)
    {
        locations[id].SetLevel(locations[id].GetLevel() + 1);
    }
    double PercentBonus(int id)
    {
        return Math.Floor(UpsAndVars.Instance.locationBonus * locations[id].GetMult() * 100);
    }
    public void SetBackground()
    {
        background.sprite = sprites[activeLocation];
        snowman.SetActive(activeLocation == 2);
        

        foreach (SpriteRenderer pc in primaryColors)
        {
            pc.color = locations[activeLocation].GetColor();
        }
        foreach (SpriteRenderer sc in secondaryColors)
        {
            sc.color = locations[activeLocation].GetSecondColor();
        }
        foreach (SpriteRenderer sc in darkColors)
        {
            sc.color = DarkenColor(locations[activeLocation].GetSecondColor());
        }
    }
    private Color DarkenColor(Color c)
    {
        Color bgColor = c / 3;
        bgColor.a = 1;
        return bgColor;
    }
    public void LoadData(GameData data)
    {
        this.activeLocation = data.activeLocation;
        this.cooldown = data.locationCooldown;
        foreach (int locId in data.ownedLocations)
        {
            locations[locId].SetOwned(true);
        }
        foreach (int locId in data.revealedLocations)
        {
            locations[locId].SetRevealed(true);
        }
        for (int i = 0; i < data.locationLevels.Count; i++)
        {
            locations[i].SetLevel(data.locationLevels[i]);
        }
    }

    public void SaveData(ref GameData data)
    {
        data.activeLocation = this.activeLocation;
        data.locationCooldown = this.cooldown;

        List<int> ownedLocs = new List<int> { };
        foreach (var location in locations)
        {
            if (location.IsOwned())
            {
                ownedLocs.Add(location.GetID());
            }
        }
        data.ownedLocations = ownedLocs;

        List<int> revLocs = new List<int> { };
        foreach (var location in locations)
        {
            if (location.ShouldReveal())
            {
                revLocs.Add(location.GetID());
            }
        }
        data.revealedLocations = revLocs;

        List<int> locationLevels = new List<int> { };
        foreach (var location in locations)
        {
            locationLevels.Add(location.GetLevel());
        }
        data.locationLevels = locationLevels;
    }
}
