using BreakInfinity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AbilityBonuses : MonoBehaviour
{
    public static AbilityBonuses Instance;

    //Access Variables
    //Commons
    public bool osuControls, noCombo, foolery;
    private double coffeeMult = 1, dualMult = 1, focusedIdle = 1, focusedActive = 1, taxed = 1;
    //Uncommons
    public bool oneTarget;
    public float easyMode = 1;
    public int scatterTarget = 0;
    //Rares
    public bool silverFloor;
    //Epics
    public bool boardClickable, goldFloor, foursMult, lightning;
    public int lightningCount = 0;
    public GameObject lightningPrefab;
    //Legendaries
    public bool nirvana, superCombo;
    //Mythics
    public bool dragon;
    public bool ultFloor;
    public GameObject autoCursor;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public double GetLocationMultiplier()
    {
        double locationMultiplier = 1;
        return locationMultiplier;
    }
    public BigDouble GetClickMultiplier()
    {
        BigDouble clickMultiplier = 1;
        clickMultiplier *= dualMult;
        clickMultiplier *= focusedActive;
        if (noCombo)
            clickMultiplier *= 5;
        if (foolery)
            clickMultiplier *= 2;
        if (oneTarget)
            clickMultiplier *= 15;
        return clickMultiplier;
    }

    public BigDouble GetIdleMultiplier()
    {
        BigDouble idleMultiplier = 1;
        idleMultiplier *= coffeeMult;
        idleMultiplier *= dualMult;
        idleMultiplier *= focusedIdle;
        return idleMultiplier;
    }
    public double GetPriceMultiplier()
    {
        double priceMultiplier = 1;
        priceMultiplier *= taxed;
        return priceMultiplier;
    }
    public int GetBonusTargets()
    {
        int bonusTargets = 0;
        bonusTargets += scatterTarget;
        return bonusTargets;
    }

    public void SetAbilityEffects(string id, bool activated, bool reapply = false)
    {
        switch (id)
        {
            //Common
            case "cff":
                if (activated)
                    coffeeMult = 1.25;
                else
                    coffeeMult = 1;
                break;
            case "dwl":
                if (activated)
                    dualMult = 1.3;
                else
                    dualMult = 1;
                break;
            case "tax":
                if (activated)
                    taxed = 0.95;
                else
                    taxed = 1;
                Gun.Instance.UpdatePrices();
                break;
            case "ffr":
                if (activated)
                {
                    focusedActive = 10;
                    focusedIdle = 0.5;
                }
                else
                {
                    focusedActive = 1;
                    focusedIdle = 1;
                }
                break;
            case "osu":
                osuControls = activated;
                break;
            case "rlx":
                noCombo = activated;
                break;
            case "tom":
                foolery = activated;
                break;
            case "egg":
                if (activated && !reapply)
                {
                    Vars.Instance.eggUses++;
                    if (Vars.Instance.eggUses >= 3)
                    {
                        if (Abilities.Instance.ownedAbilities.IndexOf(Abilities.Instance.GetAbilityOfID("egg")) != -1)
                            Abilities.Instance.ownedAbilities[Abilities.Instance.ownedAbilities.IndexOf(Abilities.Instance.GetAbilityOfID("egg"))] = Abilities.Instance.GetAbilityOfID("dra");
                        if (System.Array.IndexOf(Abilities.Instance.slottedAbilities, Abilities.Instance.GetAbilityOfID("egg")) != -1)
                            Abilities.Instance.slottedAbilities[System.Array.IndexOf(Abilities.Instance.slottedAbilities, Abilities.Instance.GetAbilityOfID("egg"))] = Abilities.Instance.GetAbilityOfID("dra");
                        Abilities.Instance.DisplayBinder();
                        SetAbilityEffects("dra", true);
                    }
                }
                break;
            case "swt":
                if (activated)
                {
                    BigDouble gain = Vars.Instance.FindHPS() * 1200;
                    Vars.Instance.AddHits(gain);
                    FloatingText.Instance.PopText(gain, Color.white, 1);
                }
                break;
            //Uncommon
            case "one":
                oneTarget = activated;
                break;
            case "esy":
                if (activated)
                    easyMode = 0.7f;
                else
                    easyMode = 1;
                break;
            case "sca":
                if (activated)
                    scatterTarget = 3;
                else
                    scatterTarget = 0;
                break;
            //Rare
            case "sfl":
                silverFloor = activated;
                break;
            //Epic
            case "gfl":
                goldFloor = activated;
                break;
            case "bar":
                boardClickable = activated;
                break;
            case "frs":
                foursMult = activated;
                break;
            case "stc":
                lightning = activated;
                break;
            //Legendary
            case "nir":
                nirvana = activated;
                break;
            case "twp":
                if (activated)
                    Abilities.Instance.ReduceAllCooldowns(true);
                break;
            case "exp":
                if (activated)
                    BoardHandler.Instance.maxCombo = 100;
                else
                    BoardHandler.Instance.maxCombo = 50;
                break;
            //Mythic
            case "dra":
                dragon = activated;
                break;
            case "ufl":
                ultFloor = activated;
                break;
            case "scr":
                if (activated && !reapply)
                    if (Vars.Instance.autoCursorSpeed < 100)
                        Vars.Instance.autoCursorSpeed += 1f;
                autoCursor.SetActive(activated);
                break;
                
        }
    }

    //Ability Methods
    public void Lightning(GameObject clickedTarget)
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Target");
        foreach (GameObject target in targets)
        {
            if (target == clickedTarget)
                continue;
            Destroy(Instantiate(lightningPrefab, target.transform), 0.333f);
            BigDouble gain = BoardHandler.Instance.ClickAmount(target.GetComponent<indexNum>().index) / 10.0;
            Vars.Instance.AddHits(gain);
            FloatingText.Instance.PopText(gain, Color.yellow, 1f);
        }
    }
}
