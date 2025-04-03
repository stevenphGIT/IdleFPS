using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using static ColorFunctions;

public class Abilities : MonoBehaviour, IDataPersistence
{
    public static Abilities Instance;

    public Ability[] slottedAbilities;
    public List<Ability> ownedAbilities;

    public CardHolder[] holders = new CardHolder[5];

    public Ability[] allAbilities;

    public Sprite defaultHolderSprite;
    public Sprite abilityIconLocked;
    public List<Sprite> abilityIconActive;
    public List<Sprite> abilityIconUsed;
    public List<Color> abilityColor;

    public List<GameObject> abilitySlots;
    public List<SpriteRenderer> abilityRenderers;
    public List<SpriteRenderer> abilityHolderRenderers;
    public List<bool> abilityActive;
    public List<bool> abilityOnCD;
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
            if (Time.time >= abUpTime[i] && slottedAbilities[i] != null)
            {
                EndAbility(i);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 5; i++)
        {
            if (slottedAbilities[i] == null)
            {
                abilityHolderRenderers[i].sprite = abilityIconLocked;
                abilityHolderRenderers[i].color = Color.white;
                abilityRenderers[i].sprite = null;
                continue;
            }
            else
            {
                abilityHolderRenderers[i].sprite = defaultHolderSprite;
            }
            if (Time.time >= abUpTime[i])
            {
                abilityHolderRenderers[i].color = abilityColor[slottedAbilities[i].rarity];
                abilityRenderers[i].sprite = slottedAbilities[i].abilityIcon;
                abilityRenderers[i].color = Color.white;
                if (abBotOn)
                {
                    UseAbility(i);
                }
            }
            else if (Time.time <= abDoneTime[i])
            {
                abilityHolderRenderers[i].color = DarkenColor(abilityColor[slottedAbilities[i].rarity], 2);
                abilityRenderers[i].color = DarkenColor(abilityRenderers[i].color, 2);
            }
            else
            {
                abilityHolderRenderers[i].color = DarkenColor(abilityColor[slottedAbilities[i].rarity], 4);
                abilityRenderers[i].color = DarkenColor(abilityRenderers[i].color, 4);
                EndAbility(i);
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
        //Booleans
        for (int i = 0; i < 5; i++)
        {
            abilityActive.Add(false);
        }
        for (int i = 0; i < 5; i++)
        {
            abilityOnCD.Add(false);
        }

        for (int i = 0; i < 5; i++)
        {
            abUpTime.Add(0);
        }
        for (int i = 0; i < 5; i++)
        {
            abDoneTime.Add(0);
        }
    }

    public void ReduceAllCooldowns()
    {
        for (int i = 0; i < 5; i++)
        {
            if (abilityActive[i] || abUpTime[i] < Time.time)
                return;
            else
                abUpTime[i] -= 10f;
        }
    }

    public void UseSelectedAbility()
    {
        if (abilitySelected == -1)
        {
            return;
        }
        else
        {
            for (int i = 0; i < 5; i++)
            {
                if (abilitySelected == i)
                {
                    if (slottedAbilities[i] == null)
                    {
                        FloatingText.Instance.PopText("No ability equipped in this slot!", new Color32(255, 0, 0, 255), 0);
                        HitSound.Instance.source.PlayOneShot(HitSound.Instance.cantUse);
                        return;
                    }
                    if (Time.time > abUpTime[i])
                    {
                        abilityActive[i] = true;
                        UseAbility(i);
                    }
                    else
                    {
                        FloatingText.Instance.PopText("You can't use that right now...", new Color32(255, 0, 0, 255), 0);
                        HitSound.Instance.source.PlayOneShot(HitSound.Instance.cantUse);
                    }
                }
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
        HitSound.Instance.source.PlayOneShot(HitSound.Instance.abilitySounds[i]);
        
        abUpTime[i] = Time.time + TrueDuration(slottedAbilities[i].abilityDuration) + TrueCooldown(slottedAbilities[i].abilityCooldown);
        abDoneTime[i] = Time.time + TrueDuration(slottedAbilities[i].abilityDuration);

        Gun.Instance.UpdatePrices();
        Vars.Instance.abilitiesUsed++;
        LocationManager.Instance.CheckForLocationReveal();
    }
    public void EndAbility(int id)
    {
        abilityActive[id] = false;
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

    public Ability GetAbilityOfID(int id) {
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
        foreach (Ability ab in ownedAbilities)
        {
            if (ab.id == a.id)
            {
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
        List<LiftableCard> toRemove = new List<LiftableCard>();
        foreach (LiftableCard d in toDisplay)
        {
            int index = Array.FindIndex(slottedAbilities, s => s == d.heldAbility);

            if (index != -1)
            {
                holders[index].EquipCard(d);
                d.slot = holders[index];
                d.gameObject.transform.position = holders[index].transform.position;
                d.SetReturnPos(d.gameObject.transform.position);
                d.gameObject.transform.SetParent(topObj.transform);
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
            toDisplay[i].SetReturnPos(toDisplay[i].gameObject.transform.position);
            toDisplay[i].gameObject.transform.SetParent(binder.transform);
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
    }
    public void LoadData(GameData data)
    {
        allAbilities = Resources.LoadAll<Ability>("Abilities/");
        PopulateList();
        this.abilityActive = data.abilityActive;

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
            
        foreach (int id in data.ownedAbilityIDs)
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
        data.abilityActive = this.abilityActive;

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

        data.ownedAbilityIDs = new List<int>();
        foreach (Ability ab in ownedAbilities)
        {
            data.ownedAbilityIDs.Add(ab.id);
        }

        for (int i = 0; i < 5; i++)
        {
            if (slottedAbilities[i] == null)
                data.slottedAbilityIDs[i] = -1;
            else
                data.slottedAbilityIDs[i] = slottedAbilities[i].id;
        }
    }
}


