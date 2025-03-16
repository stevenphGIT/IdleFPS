using BreakInfinity;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Animations;
using UnityEngine.UI;


public class Abilities : MonoBehaviour, IDataPersistence
{
    public static Abilities Instance;

    public List<Ability> slottedAbilities;

    public List<Ability> commonAbilities;
    public List<Ability> uncommonAbilities;
    public List<Ability> rareAbilities;
    public List<Ability> epicAbilities;
    public List<Ability> legendaryAbilities;
    public List<Ability> mythicAbilities;

    public List<Sprite> abilityIconActive;
    public List<Sprite> abilityIconUsed;
    public List<Color> abilityColor;

    public List<GameObject> abilitySlots;
    public List<SpriteRenderer> abilityRenderers;
    public List<bool> abilityUnlocked;
    public List<bool> abilityActive;
    public List<bool> abilityOnCD;
    public List<float> abDoneTime;
    public List<float> abUpTime;

    private int abilitySelected;

    public bool abBotOn;
    public Animator abilityBotAnim;

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
        TestForAbilityUnlock();
        abilityBotAnim.SetBool("botActive", abBotOn);
        for (int i = 0; i < slottedAbilities.Count; i++)
        {
            if (Time.time >= abUpTime[i] && abilityUnlocked[i])
            {
                EndAbility(i);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < slottedAbilities.Count; i++)
        {
            if (!abilityUnlocked[i])
                break;
            if (Time.time >= abUpTime[i])
            {
                abilityRenderers[i].sprite = slottedAbilities[i].abilityIcon;
                if (abBotOn)
                {
                    abilityActive[i] = true;
                    UseAbility(i);
                }
            }
            else if (Time.time <= abDoneTime[i])
            {
                abilityRenderers[i].sprite = abilityIconActive[slottedAbilities[i].rarity];
            }
            else
            {
                abilityRenderers[i].sprite = abilityIconUsed[slottedAbilities[i].rarity];
                EndAbility(i);
            }
        }
        //if (!abilityUnlocked[4])
        //    TestForAbilityUnlock();
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

    void TestForAbilityUnlock()
    {
        for (int i = 0; i < abilitySlots.Count; i++)
        {
            if (UpsAndVars.Instance.permabilities)
                Unlock(i);
            else if (Vars.Instance.totalHitCount >= 100000 * BigDouble.Pow(10, i) && !abilityUnlocked[i])
            {
                Unlock(i);
            }
        }
    }

    public void PopulateList()
    {
        //Booleans
        for (int i = 0; i < slottedAbilities.Count; i++)
        {
            abilityUnlocked.Add(false);
        }
        for (int i = 0; i < slottedAbilities.Count; i++)
        {
            abilityActive.Add(false);
        }
        for (int i = 0; i < slottedAbilities.Count; i++)
        {
            abilityOnCD.Add(false);
        }

        for (int i = 0; i < slottedAbilities.Count; i++)
        {
            abUpTime.Add(0);
        }
        for (int i = 0; i < slottedAbilities.Count; i++)
        {
            abDoneTime.Add(0);
        }
    }

    public void ReduceAllCooldowns()
    {
        for (int i = 0; i < slottedAbilities.Count; i++)
        {
            if (abilityActive[i] || abUpTime[i] < Time.time)
                return;
            else
                abUpTime[i] -= 10f;
        }
    }
    public void Unlock(int i)
    {
        abilitySlots[i].GetComponent<SpriteRenderer>().sprite = slottedAbilities[i].abilityIcon;
        abilityUnlocked[i] = true;
    }

    public void UseSelectedAbility()
    {
        if (abilitySelected == -1)
        {
            return;
        }
        else
        {
            for (int i = 0; i < slottedAbilities.Count; i++)
            {
                if (abilitySelected == i)
                {
                    if (!abilityUnlocked[abilitySelected])
                    {
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
    public void UseAbility(int i)
    {
        HitSound.Instance.source.PlayOneShot(HitSound.Instance.abilitySounds[i]);
        double durationMult = UpsAndVars.Instance.duration;
        if (LocationManager.Instance.activeLocation == 4 && AvailableUpgrades.Instance.locationBonuses)
        {
            durationMult *= (1 + (LocationManager.Instance.locations[4].GetMult() * UpsAndVars.Instance.locationBonus));
            durationMult *= UpsAndVars.Instance.duration;
        }
        abUpTime[i] = Time.time + (float)((slottedAbilities[i].abilityCooldown / UpsAndVars.Instance.cooldown) + (slottedAbilities[i].abilityDuration * durationMult));
        abDoneTime[i] = Time.time + (float)(slottedAbilities[i].abilityDuration * durationMult);

        Gun.Instance.UpdatePrices();
        Vars.Instance.abilitiesUsed++;
        LocationManager.Instance.CheckForLocationReveal();
    }
    public void EndAbility(int id)
    {
        if (abilityActive.Count > id)
            abilityActive[id] = false;
    }
    public void SetShow(string name)
    {
        if (name == "FocusedAbility")
        {
            abilitySelected = 0;
        }
        else if (name == "DeadlyAbility")
        {
            abilitySelected = 1;
        }
        else if (name == "BarrageAbility")
        {
            abilitySelected = 2;
        }
        else if (name == "ScatterAbility")
        {
            abilitySelected = 3;
        }
        else if (name == "NirvanaAbility")
        {
            abilitySelected = 4;
        }
        else
        {
            abilitySelected = -1;
            Textbox.Instance.HideBox();
            return;
        }

        if (!abilityUnlocked[abilitySelected])
        {
            Textbox.Instance.SetTitleText("Ability Locked!");
            Textbox.Instance.SetTitleColor(Color.red);
            Textbox.Instance.SetFullDescriptionText("Earn " + (Vars.Instance.TotalAbbr((100000 * Mathf.Pow(10, abilitySelected)) - Vars.Instance.totalHitCount)) + " more total hits to unlock this ability.");
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

    public void ResetAbilities()
    {
        for (int i = 0; i < abUpTime.Count; i++)
        {
            abUpTime[i] = 0;
            abDoneTime[i] = 0;
            EndAbility(i);
        }
    }

    public void LoadData(GameData data)
    {
        PopulateList();
        this.abilityActive = data.abilityActive;

        this.abBotOn = data.abBotOn;

        for (int i = 0; i < slottedAbilities.Count; i++)
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
    }

    public void SaveData(ref GameData data)
    {
        data.abilityActive = this.abilityActive;

        data.abBotOn = this.abBotOn;

        for (int i = 0; i < slottedAbilities.Count; i++)
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
    }
}


