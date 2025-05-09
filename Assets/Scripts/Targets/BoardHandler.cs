using BreakInfinity;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class BoardHandler : MonoBehaviour
{
    public static BoardHandler Instance;

    double redOdds = 0;
    double silverOdds = 0;
    double goldOdds = 0;
    double platOdds = 0;
    double ultimateOdds = 0;

    public int maxCombo = 50;
    //Targets
    public GameObject redTargetPrefab;
    public GameObject silverTargetPrefab;
    public GameObject goldTargetPrefab;
    public GameObject platTargetPrefab;
    public GameObject omegaTargetPrefab;
    //DP Targets
    public GameObject targetParent;
    public GameObject redDP;
    public GameObject silverDP;
    public GameObject goldDP;
    public GameObject platDP;
    public GameObject omegaDP;
    public bool midHit;
    //GUI
    public GameObject visualHolder;
    public Vector3 targetSpawnPos;
    public TMP_Text[] targetOddsTexts;
    public GameObject timerBar;
    public GameObject timerBarBG;
    public TMP_Text comboText;
    public TMP_Text recordText;
    public Animator comboAnim;
    public GameObject fire;
    public GameObject superFire;
    private float comboTimer;
    private float comboTime = 0.4f;
    public int comboCount = 0;
    private float comboDisplayCooldown = -1f;
    //Target Variables
    public int activeTargetCount;
    public List<Vector3> posList = new();
    public GameObject[] targets;
    public Color[] targetColors;

    float speedMod = 1f;
    void Awake()
    {
        if (Instance == null)
            Instance = this;

        targetSpawnPos = visualHolder.transform.position;
        activeTargetCount = 0;

        float targetSpacing = 0.75f;
        for (int i = -2; i < 3; i++)
        {
            for (int j = -2; j < 3; j++)
            {
                posList.Add(new Vector3(targetSpawnPos.x + (i * targetSpacing), targetSpawnPos.y + (j * targetSpacing), 0));
            }
        }
    }

    void Start()
    {
        for (int i = activeTargetCount; i < TargetsToSpawn(); i++)
        {
            SpawnTarget();
        }

        comboText.text = "";
        recordText.text = "";

        fire.SetActive(false);
        superFire.SetActive(false);
    }

    private void Update()
    {
        if (comboDisplayCooldown > 0)
        {
            comboDisplayCooldown -= Time.deltaTime;
        }

        if (activeTargetCount < TargetsToSpawn())
        {
            SpawnTarget();
        }
        if (activeTargetCount > TargetsToSpawn())
        {
            RemoveTarget(GameObject.FindGameObjectWithTag("Target"));
        }

        if (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime * AbilityBonuses.Instance.easyMode;
            timerBar.transform.localScale = new Vector3(2 * (comboTimer / comboTime), 2, 1);
            if (comboTimer < 0)
            {
                if (comboCount > 4)
                {
                    FloatingText.Instance.PopText("Combo expired!", Color.red, 0);
                    HitSound.Instance.source.PlayOneShot(HitSound.Instance.comboEnd);
                }
                EndCombo();
            }
        }
    }
    public void SetTargetOddsDisplay()
    {
        redOdds = 0;
        silverOdds = 0;
        goldOdds = 0;
        platOdds = 0;
        ultimateOdds = 0;
        switch (AvailableUpgrades.Instance.targetRarity)
        {
            case 0:
                redOdds = 1;
                break;
            case 1:
                redOdds = 0.9;
                silverOdds = 0.1;
                break;
            case 2:
                redOdds = 0.8;
                silverOdds = 0.15;
                goldOdds = 0.05;
                break;
            case 3:
                redOdds = 0.7;
                silverOdds = 0.175;
                goldOdds = 0.1;
                platOdds = 0.025;
                break;
            case 4:
                redOdds = 0.600;
                silverOdds = 0.150;
                goldOdds = 0.1875;
                platOdds = 0.050;
                ultimateOdds = 0.0125;
                break;
            case 5:
                redOdds = 0.5;
                silverOdds = 0.125;
                goldOdds = 0.2;
                platOdds = 0.15;
                ultimateOdds = 0.025;
                break;
            default:
                redOdds = 1;
                break;
        }

        //Set Odds Texts
        if (ultimateOdds > 0)
            targetOddsTexts[0].text = "- " + (ultimateOdds * 100) + "%";
        else
            targetOddsTexts[0].text = "";
        if (platOdds > 0)
            targetOddsTexts[1].text = "- " + (platOdds * 100) + "%";
        else
            targetOddsTexts[1].text = "";
        if (goldOdds > 0)
            targetOddsTexts[2].text = "- " + (goldOdds * 100) + "%";
        else
            targetOddsTexts[2].text = "";
        if (silverOdds > 0)
            targetOddsTexts[3].text = "- " + (silverOdds * 100) + "%";
        else
            targetOddsTexts[3].text = "";
        if (redOdds < 1)
            targetOddsTexts[4].text = "- " + (redOdds * 100) + "%";
        else
            targetOddsTexts[4].text = "";
    }
    public void SpawnTarget()
    {
        int targetIndex = GetTargetPos();

        if (targetIndex == -1)
            return;
        //All odds in percentage form
        int selectedTarget = -1;
        SetTargetOddsDisplay();

        //Select Target
        int randomInt = UnityEngine.Random.Range(0, 1000);

        if (randomInt >= (1000 - (ultimateOdds * 1000)))
            selectedTarget = 0;
        else if (randomInt >= (1000 - (platOdds * 1000) - (ultimateOdds * 1000)))
            selectedTarget = 1;
        else if (randomInt >= (1000 - (platOdds * 1000) - (ultimateOdds * 1000) - (goldOdds * 1000)))
            selectedTarget = 2;
        else if (randomInt >= (1000 - (platOdds * 1000) - (ultimateOdds * 1000) - (goldOdds * 1000) - (silverOdds * 1000)))
            selectedTarget = 3;
        else
            selectedTarget = 4;
        GameObject targetToSpawn = null;
        if (AbilityBonuses.Instance.ultFloor)
        {
            targetToSpawn = omegaTargetPrefab;
        }
        else if (AbilityBonuses.Instance.goldFloor)
        {
            if (selectedTarget >= 2)
                targetToSpawn = goldTargetPrefab;
        }
        else if (AbilityBonuses.Instance.silverFloor)
        {
            if (selectedTarget >= 3)
                targetToSpawn = silverTargetPrefab;
        }
        if (targetToSpawn == null)
        {
            if (selectedTarget == 0)
                targetToSpawn = omegaTargetPrefab;
            else if (selectedTarget == 1)
                targetToSpawn = platTargetPrefab;
            else if (selectedTarget == 2)
                targetToSpawn = goldTargetPrefab;
            else if (selectedTarget == 3)
                targetToSpawn = silverTargetPrefab;
            else
                targetToSpawn = redTargetPrefab;
        }
        targets[targetIndex] = Instantiate(targetToSpawn, posList[targetIndex], Quaternion.identity, targetParent.transform);
        activeTargetCount++;
    }
    public void RemoveTarget(GameObject target)
    {
        double targetMax;
        targetMax = TargetsToSpawn();

        if (activeTargetCount - 1 < targetMax)
        {
            SpawnTarget();
        }
        activeTargetCount--;
        Destroy(target);
    }

    public double TargetsToSpawn()
    {
        if (AbilityBonuses.Instance.oneTarget)
            return 1;
        return (AvailableUpgrades.Instance.targetLevel + UpsAndVars.Instance.bonusTargets + AbilityBonuses.Instance.GetBonusTargets());
    }
    public BigDouble ClickAmount()
    {
        BigDouble amount;
        amount = AvailableUpgrades.Instance.GetComponent<AvailableUpgrades>().tgMult + (AvailableUpgrades.Instance.GetComponent<AvailableUpgrades>().tgMultPercent * (Gun.Instance.TotalCount() - Gun.Instance.targetPowers[0]));
        return amount;
    }
    public BigDouble ClickHPSBonus()
    {
        return (AvailableUpgrades.Instance.GetComponent<AvailableUpgrades>().tgHpsPercent * Vars.Instance.totalHps);
    }
    public BigDouble GlobalMultiplier()
    {
        BigDouble multiplier = 1;
        if (midHit)
            multiplier *= 10;
        multiplier *= AbilityBonuses.Instance.GetClickMultiplier();
        if (comboCount > 1)
        {
            double locationBonus = 1;
            if (LocationManager.Instance.activeLocation == 5 && AvailableUpgrades.Instance.locationBonuses)
            {
                locationBonus = 1 + (LocationManager.Instance.locations[5].GetMult() * UpsAndVars.Instance.locationBonus);
            }
            if (comboCount <= maxCombo)
                multiplier *= Math.Pow(1.05, comboCount - 1) * locationBonus;
            else
                multiplier *= Math.Pow(1.05, maxCombo) * locationBonus;
        }

        if (UpsAndVars.Instance.critsUnlocked && IsCrit())
        {
            multiplier *= CritMultiplier();
            FloatingText.Instance.PopText("Crit!", Color.green, 0.5f);
            if (UpsAndVars.Instance.criticalCooldowns)
            {
                Abilities.Instance.ReduceAllCooldowns();
            }
            if (UpsAndVars.Instance.thoriumCrits)
            {
                Vars.Instance.thorium += 1;
            }
        }

        return multiplier;
    }
    public BigDouble ClickMultiplier()
    {
        BigDouble multiplier = 1;
        if (LocationManager.Instance.activeLocation == 1 && AvailableUpgrades.Instance.locationBonuses)
            multiplier *= (1 + (UpsAndVars.Instance.locationBonus * LocationManager.Instance.locations[1].GetMult()));
        if (UpsAndVars.Instance.targetception)
            multiplier *= (1 + ((Vars.Instance.totalClickTracker + Vars.Instance.clickTracker) * 0.005));
        multiplier *= (1 + (Vars.Instance.rads * UpsAndVars.Instance.radMult));
        multiplier *= UpsAndVars.Instance.prestigeTargetMultiplier;
        if (AbilityBonuses.Instance.foursMult && ((DateTime.Now.Minute == 44 && DateTime.Now.Hour == 4) || (DateTime.Now.Minute == 44 && DateTime.Now.Hour == 16)))
            multiplier *= 444444;
        return multiplier;
    }
    public BigDouble TotalClickAmount()
    {
        return ClickAmount() * ClickMultiplier() + ClickHPSBonus();
    }
    public bool IsCrit()
    {
        float randForCrit = UnityEngine.Random.Range(0, 200);
        if (randForCrit >= (199 - (UpsAndVars.Instance.critOdds * 3)))
            return true;
        return false;
    }
    public double CritMultiplier()
    {
        return 2 * UpsAndVars.Instance.critAmount;
    }

    public int GetTargetPos()
    {
        List<int> availablePositions = new();
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] == null)
                availablePositions.Add(i);
        }
        if (availablePositions.Count > 0)
        {
            int choice = UnityEngine.Random.Range(0, availablePositions.Count);
            return availablePositions[choice];
        }
        return -1;
    }
    public void PopulateBoard()
    {
        for (int i = activeTargetCount; i < TargetsToSpawn(); i++)
        {
            SpawnTarget();
        }
    }

    public void IncrementCombo()
    {
        if (AbilityBonuses.Instance.noCombo)
            return;
        comboCount++;
        if (BossHandler.Instance.activeBoss)
        {
            if (comboCount == 10)
                BossHandler.Instance.activeBoss.Hurt(1.0, "10 combo!");
            else if (comboCount == 25)
                BossHandler.Instance.activeBoss.Hurt(3.0, "25 combo!");
            else if (comboCount == 50)
                BossHandler.Instance.activeBoss.Hurt(7.0, "50 combo!");
            else if (comboCount > 50 && comboCount % 5 == 0)
                BossHandler.Instance.activeBoss.Hurt(2.0, "Burning up!");
        }
        if (comboDisplayCooldown < 0)
        {
            if(!timerBarBG.activeSelf)
                timerBarBG.SetActive(true);
            comboAnim.SetTrigger("hit");
            comboText.text = comboCount + "";
            recordText.text = "Combo";
            Color streakColor;

            if (maxCombo >= 100 && comboCount >= 100)
            {
                streakColor = new Color(0, 1, 1);
                if (!superFire.activeSelf)
                {
                    HitSound.Instance.source.PlayOneShot(HitSound.Instance.fwoom);
                    superFire.SetActive(true);
                    fire.SetActive(false);
                }
            }
            else if (comboCount >= 50)
            {
                streakColor = new Color(1, 0, 1);
                if (!fire.activeSelf)
                {
                    HitSound.Instance.source.PlayOneShot(HitSound.Instance.fwoom);
                    fire.SetActive(true);
                    superFire.SetActive(false);
                }
            }
            else if (comboCount > 25)
            {
                streakColor = new Color(1, 1 * (1 - ((comboCount - (50 / 2.0f)) / (50 / 2.0f))), 0);
            }
            else
            {
                streakColor = new Color(1, 1, 1 * (1 - (comboCount / (50 / 2.0f))));
            }

            comboText.color = streakColor;
            recordText.color = streakColor;
        }

        comboTimer = comboTime;
    }

    public void CollectTarget(Collider2D tg)
    {
        if (AbilityBonuses.Instance.lightning)
        {
            AbilityBonuses.Instance.lightningCount++;
            if (AbilityBonuses.Instance.lightningCount >= 3)
            {
                AbilityBonuses.Instance.Lightning(tg.gameObject);
                AbilityBonuses.Instance.lightningCount = 0;
            }
        }
        int index = tg.gameObject.GetComponent<indexNum>().index;
        if (BossHandler.Instance.fighting)
        {
            BossHandler.Instance.storedDamage += 0.2;
        }
        BigDouble amountToGain = ClickAmount(index);
        FloatingText.Instance.TargetText(amountToGain, targetColors[index], tg.gameObject.transform.position);
        RemoveTarget(tg.gameObject);
        if (AbilityBonuses.Instance.foolery)
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.funnyTargets[UnityEngine.Random.Range(0, HitSound.Instance.funnyTargets.Length)], 1f);
        else
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.targetHits[index], 1f);
        Vars.Instance.hits += amountToGain;
        Vars.Instance.totalHitCount += amountToGain;
        Vars.Instance.clickTracker += 1;
        switch (index)
        {
            case 1:
                Vars.Instance.silverClickTracker += 1;
                break;
            case 2:
                Vars.Instance.goldClickTracker += 1;
                break;
            case 3:
                Vars.Instance.platClickTracker += 1;
                break;
            case 4:
                Vars.Instance.omegaClickTracker += 1;
                break;
        }
    }

    public BigDouble ClickAmount(int index)
    {
        BigDouble amountToGain = 0;
        switch (index) {
            case 0:
                amountToGain = TotalClickAmount() * GlobalMultiplier();
                break;
            case 1:
                amountToGain = (Vars.Instance.totalHps * Vars.Instance.silverHpsSecs + TotalClickAmount()) * GlobalMultiplier();
                break;
            case 2:
                amountToGain = (Vars.Instance.totalHps * Vars.Instance.goldHpsSecs + TotalClickAmount()) * GlobalMultiplier();
                break;
            case 3:
                amountToGain = (Vars.Instance.totalHps * Vars.Instance.platHpsSecs + TotalClickAmount()) * GlobalMultiplier();
                break;
            case 4:
                amountToGain = (Vars.Instance.totalHps * 3600 + TotalClickAmount()) * GlobalMultiplier();
                break;
        }
        return amountToGain;
    }
    public void BoardClick()
    {
        BigDouble amount = TotalClickAmount() * GlobalMultiplier();
        FloatingText.Instance.PopText(amount, new Color32(255, 255, 255, 255), speedMod);
        Vars.Instance.hits += amount;
        Vars.Instance.totalHitCount += amount;
    }

    public void EndCombo()
    {
        comboTimer = -1;
        timerBarBG.SetActive(false);
        timerBar.transform.localScale = new Vector3(0, 2, 1);
        fire.SetActive(false);
        superFire.SetActive(false);
        if (comboCount != 0)
        {
            if (comboCount >= 35)
            {
                LocationManager.Instance.CheckForLocationReveal();
            }
            bool yay = false;
            if (comboCount > Vars.Instance.comboRecord)
            {
                Vars.Instance.comboRecord = comboCount;
                yay = true;
            }
            if (yay)
            {
                comboAnim.SetTrigger("comboPR");
                comboDisplayCooldown = 1.15f;
                recordText.text = "New record!";
                recordText.color = new Color(1, 0, 1);
            }
            else
            {
                comboAnim.SetTrigger("comboEnd");
            }

            comboCount = 0;
        }
    }
}
