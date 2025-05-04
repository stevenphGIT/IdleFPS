using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using static ColorFunctions;
using JetBrains.Annotations;

public class Abilities : MonoBehaviour, IDataPersistence
{
    public static Abilities Instance;

    public Ability[] slottedAbilities;
    public List<Ability> ownedAbilities;

    public CardHolder[] holders = new CardHolder[5];

    public Ability[] allAbilities;

    public Sprite defaultHolder;
    public Sprite activeHolder;
    public Sprite lockedHolder;
    public List<Sprite> abilityIconActive;
    public List<Sprite> abilityIconUsed;
    public List<Color> abilityColor;

    public List<GameObject> abilitySlots;
    public List<SpriteRenderer> abilityRenderers;
    public List<SpriteRenderer> abilityHolderRenderers;
    public int[] abilityStage;
    public List<float> abDoneTime;
    public List<float> abUpTime;

    private int abilitySelected;

    public bool abBotOn;
    public Animator abilityBotAnim;

    public GameObject binder;
    public GameObject topObj;

    public GameObject cardPrefab;

    public GameObject top, bottom;
    public SliderRect slider; 

    public List<LiftableCard> abilityCards;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null) { Instance = this; }
            PopulateList();
        
        for (int i = 0; i < abilitySlots.Count; i++)
        {
            abilityRenderers.Add(abilitySlots[i].GetComponent<SpriteRenderer>());
        }
    }
    void Start()
    {
        DisplayBinder();
        abilityBotAnim.SetBool("botActive", abBotOn);
        for (int i = 0; i < 5; i++)
        {
            if (slottedAbilities[i] != null)
            {
                abilityHolderRenderers[i].color = abilityColor[slottedAbilities[i].rarity];
                abilityRenderers[i].sprite = slottedAbilities[i].abilityIcon;
                abilityRenderers[i].color = Color.white;
                abilityHolderRenderers[i].sprite = defaultHolder;
            }
            else
            {
                abilityHolderRenderers[i].sprite = lockedHolder;
                abilityHolderRenderers[i].color = Color.white;
                abilityRenderers[i].sprite = null;
                abilityStage[i] = -1;
            }      
        }
        SetAbilityStages();
    }

    // Update is called once per frame
    void Update()
    {
        SetAbilityStages();
    }
    public void SetAbilitySprites()
    {
        for (int i = 0; i < 5; i++)
        {
            if (slottedAbilities[i] == null)
            {
                abilityHolderRenderers[i].sprite = lockedHolder;
                abilityHolderRenderers[i].color = Color.white;
                abilityRenderers[i].sprite = null;
            }
        }
        SetAbilityStages();
    }
    void SetAbilityStages(bool forceUpdate = false)
    {
        for (int i = 0; i < 5; i++)
        {
            if (slottedAbilities[i] == null)
            {
                abilityStage[i] = -1;
                continue;
            }
            abilityRenderers[i].sprite = slottedAbilities[i].abilityIcon;
            if ((forceUpdate ||abilityStage[i] != 0) && Time.time > abUpTime[i])
            {
                abilityHolderRenderers[i].color = abilityColor[slottedAbilities[i].rarity];
                abilityHolderRenderers[i].sprite = defaultHolder;
                abilityRenderers[i].color = Color.white;
                abilityStage[i] = 0;
                if (abBotOn)
                {
                    UseAbility(i);
                }
            }
            else if ((forceUpdate || abilityStage[i] != 1) && abDoneTime[i] > Time.time)
            {
                abilityHolderRenderers[i].color = abilityColor[slottedAbilities[i].rarity];
                abilityHolderRenderers[i].sprite = activeHolder;
                abilityRenderers[i].color = Color.white;
                AbilityBonuses.Instance.SetAbilityEffects(slottedAbilities[i].id, true, true);
                abilityStage[i] = 1;
            }
            else if ((forceUpdate || abilityStage[i] != 2) && (Time.time > abDoneTime[i])) 
            {
                if (Time.time < abUpTime[i])
                    EndAbility(i);
                else
                    AbilityBonuses.Instance.SetAbilityEffects(slottedAbilities[i].id, false, true);
            }
        }
        
    }
    public void ToggleBot()
    {
        if (abBotOn)
            TurnBotOff();
        else
            TurnBotOn();
    }

    void TurnBotOn()
    {
        abilityBotAnim.SetBool("botActive", true);
        abBotOn = true;
    }

    void TurnBotOff()
    {
        abilityBotAnim.SetBool("botActive", false);
        abBotOn = false;
    }

    public void PopulateList()
    {
        //Int ID
        slottedAbilities = new Ability[5];

        abilityStage = new int[]{-1, -1, -1, -1, -1};
        for (int i = 0; i < 5; i++)
        {
            if (abUpTime.Count < 5)
                abUpTime.Add(0);
        }
        for (int i = 0; i < 5; i++)
        {
            if (abDoneTime.Count < 5)
                abDoneTime.Add(0);
        }
    }

    public void ReduceAllCooldowns(bool toZero = false)
    {
        for (int i = 0; i < 5; i++)
        {
            if (abilityStage[i] != 2)
                return;
            else
            {
                if (toZero)
                    abUpTime[i] = 0f;
                else
                    abUpTime[i] -= 10f;
            }
        }
    }

    public bool AbilityOffCooldown(int slot)
    {
        return (slottedAbilities[slot] != null 
            && (abilityStage[slot] != 1)
            && !(abUpTime[slot] > Time.time));
    }
    public void UseSelectedAbility(int a = -1)
    {
        int abID = -1;
        if (a != -1)
            abID = a;
        else
            abID = abilitySelected;
        if (abID == -1)
        {
            return;
        }
        else
        {
            if (slottedAbilities[abID] == null)
            {
                FloatingText.Instance.PopText("No ability equipped in this slot!", new Color32(255, 0, 0, 255), 0);
                HitSound.Instance.source.PlayOneShot(HitSound.Instance.cantUse);
                return;
            }
            if (Time.time > abUpTime[abID])
            {
                UseAbility(abID);
            }
            else
            {
                FloatingText.Instance.PopText("You can't use that right now...", new Color32(255, 0, 0, 255), 0);
                HitSound.Instance.source.PlayOneShot(HitSound.Instance.cantUse);
            }
        }
    }
    public float TrueDuration(float d)
    {
        double durationMult = 1;
        if (LocationManager.Instance.activeLocation == 4 && AvailableUpgrades.Instance.locationBonuses)
        {
            durationMult *= (1 + (LocationManager.Instance.locations[4].GetMult() * UpsAndVars.Instance.locationBonus));
        }
        durationMult *= UpsAndVars.Instance.duration;

        d *= (float)durationMult;
        return d;
    }
    public float TrueCooldown(float c)
    {
        c = (float)(c / UpsAndVars.Instance.cooldown);
        return c;
    }
    public void UseAbility(int i)
    {
        AbilityBonuses.Instance.SetAbilityEffects(slottedAbilities[i].id, true);
        abilityStage[i] = 1;
        abilityHolderRenderers[i].color = abilityColor[slottedAbilities[i].rarity];
        abilityHolderRenderers[i].sprite = activeHolder;
        HitSound.Instance.source.PlayOneShot(HitSound.Instance.abilitySounds[i]);
        abilityRenderers[i].color = Color.white;
        abUpTime[i] = Time.time + TrueDuration(slottedAbilities[i].abilityDuration) + TrueCooldown(slottedAbilities[i].abilityCooldown);
        abDoneTime[i] = Time.time + TrueDuration(slottedAbilities[i].abilityDuration);

        Gun.Instance.UpdatePrices();
        Vars.Instance.abilitiesUsed++;
        LocationManager.Instance.CheckForLocationReveal();
    }
    public void EndAbility(int i)
    {
        abilityHolderRenderers[i].color = DarkenColor(abilityColor[slottedAbilities[i].rarity], 4);
        abilityHolderRenderers[i].sprite = defaultHolder;
        abilityRenderers[i].color = DarkenColor(Color.white, 4);
        abilityStage[i] = 2;
        AbilityBonuses.Instance.SetAbilityEffects(slottedAbilities[i].id, false);
    }
    public void SetShow(int id)
    {
        abilitySelected = id;

        if (slottedAbilities[abilitySelected] == null)
        {
            Textbox.Instance.SetTitleText("Ability Locked!");
            Textbox.Instance.SetTitleColor(Color.red);
            Textbox.Instance.SetFullDescriptionText("Buy and equip an ability to this slot in order to use it.");
            Textbox.Instance.SetCostText(" ");
            Textbox.Instance.ShowBox();
        }
        else
        {
            Textbox.Instance.SetTitleText(slottedAbilities[abilitySelected].abilityName);
            Textbox.Instance.SetTitleColor(abilityColor[slottedAbilities[abilitySelected].rarity]);
            Textbox.Instance.SetFullDescriptionText(slottedAbilities[abilitySelected].abilityDescription);
            Textbox.Instance.SetCostColor(abilityColor[slottedAbilities[abilitySelected].rarity]);
            if (Time.time > abUpTime[abilitySelected])
            {
                Textbox.Instance.SetCostText("Ready!");
            }
            else if (Time.time < abDoneTime[abilitySelected])
            {
                Textbox.Instance.SetCostText("Active:" + ValToString.Instance.ShortenTime(abDoneTime[abilitySelected] - Time.time) + " remaining");
            }
            else
            {
                Textbox.Instance.SetCostText("On cooldown:" + ValToString.Instance.ShortenTime(abUpTime[abilitySelected] - Time.time));
            }
            Textbox.Instance.ShowBox();
        }
    }

    public Ability GetAbilityOfID(string id) {
        foreach (Ability ab in allAbilities)
        {
            if (ab.id == id)
            {
                return ab;
            }
        }
        return null;
    }

    public void ResetAbilities()
    {
        for (int i = 0; i < abUpTime.Count; i++)
        {
            abUpTime[i] = 0;
            abDoneTime[i] = 0;
            EndAbility(i);
        }
    }

    public void CollectAbility(Ability a)
    {
        bool egg = false;
        if (a.id == "egg")
            egg = true;
        foreach (Ability ab in ownedAbilities)
        {
            if (ab.id == a.id)
            {
                return;
            }
            if (egg)
            {
                if (ab.id == "dra")
                    return;
            }
        }
        ownedAbilities.Add(a);
        DisplayBinder();
    }

    public void DisplayBinder()
    {
        foreach (LiftableCard x in abilityCards)
        {
            Destroy(x.gameObject);
        }
        abilityCards.Clear();
        foreach (Ability ab in ownedAbilities)
        {
            GameObject c = Instantiate(cardPrefab, binder.transform);
            LiftableCard card = c.GetComponent<LiftableCard>();
            card.SetHeldAbility(ab);
            abilityCards.Add(card);
        }
        List<LiftableCard> toDisplay = abilityCards.OrderByDescending(lCard => lCard.heldAbility.rarity).ThenBy(lCard => lCard.heldAbility.abilityName).ToList();
        List<LiftableCard> toRemove = new();
        foreach (LiftableCard d in toDisplay)
        {
            int index = Array.FindIndex(slottedAbilities, s => s == d.heldAbility);

            if (index != -1)
            {
                holders[index].EquipCard(d);
                d.slot = holders[index];
                d.SetReturnPos(holders[index].transform.position);
                d.gameObject.transform.SetParent(topObj.transform);
                d.SetCover();
                toRemove.Add(d);
            }
        }
        foreach (LiftableCard d in toRemove)
        {
            toDisplay.Remove(d);
        }
        float hOff = binder.transform.position.x, vOff = binder.transform.position.y;
        float hInc = 2, vInc = 2.6f;
        int cardCount = 5;
        for (int i = 0; i < toDisplay.Count; i++)
        {
            toDisplay[i].slot = null;
            toDisplay[i].gameObject.transform.position = new Vector2(hOff + ((i % cardCount) * hInc), vOff - Mathf.Floor(i / cardCount) * vInc);
            toDisplay[i].SetReturnPos(toDisplay[i].gameObject.transform.position, false);
            toDisplay[i].gameObject.transform.SetParent(binder.transform);
            toDisplay[i].SetCover();
            if (i == 0)
            {
                top.transform.position = new Vector3(top.transform.position.x, toDisplay[i].gameObject.transform.position.y + toDisplay[i].gameObject.GetComponent<Collider2D>().bounds.extents.y, 0);
            }
            if (i == toDisplay.Count - 1)
            {
                bottom.transform.position = new Vector3(bottom.transform.position.x, toDisplay[i].gameObject.transform.position.y - toDisplay[i].gameObject.GetComponent<Collider2D>().bounds.extents.y, 0);
            }
        }
        slider.ResetSliderPos();
        SetAbilitySprites();
        SetAbilityStages(true);
    }
    public void LoadData(GameData data)
    {
        allAbilities = Resources.LoadAll<Ability>("Abilities/");
        PopulateList();

        this.abBotOn = data.abBotOn;

        for (int i = 0; i < 5; i++)
        {
            if (data.remainingCooldown[i] > 0)
            {
                abUpTime[i] = Time.time + data.remainingCooldown[i];
            }
            if (data.remainingDuration[i] > 0)
            {
                abDoneTime[i] = Time.time + data.remainingDuration[i];
            }
        }
            
        foreach (string id in data.ownedAbilityIDs)
        {
            Ability a = GetAbilityOfID(id);
            if (a != null)
            {
                ownedAbilities.Add(a);
            }
            else
            {
                Debug.LogError("ABILITY LOST DUE TO INVALID ID NUMBER (DID YOU SET ABILITY ID'S FIRST?)");
            }
        }

        for (int i = 0; i < 5; i++)
        {
            Ability a = GetAbilityOfID(data.slottedAbilityIDs[i]);
            slottedAbilities[i] = a;
        }
    }

    public void SaveData(ref GameData data)
    {
        data.abBotOn = this.abBotOn;

        for (int i = 0; i < 5; i++)
        {
            if (abUpTime[i] - Time.time > 0)
            {
                data.remainingCooldown[i] = abUpTime[i] - Time.time;
            }
            else
            {
                data.remainingCooldown[i] = 0;
            }
            if (abDoneTime[i] - Time.time > 0)
            {
                data.remainingDuration[i] = abDoneTime[i] - Time.time;
            }
            else
            {
                data.remainingDuration[i] = 0;
            }
        }

        data.ownedAbilityIDs = new List<string>();
        foreach (Ability ab in ownedAbilities)
        {
            data.ownedAbilityIDs.Add(ab.id);
        }

        for (int i = 0; i < 5; i++)
        {
            if (slottedAbilities[i] == null)
                data.slottedAbilityIDs[i] = null;
            else
                data.slottedAbilityIDs[i] = slottedAbilities[i].id;
        }
    }
}


