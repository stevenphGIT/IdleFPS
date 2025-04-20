using BreakInfinity;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    private float comboTimer;
    private float comboTime = 0.4f;
    public int comboCount = 0;
    private float comboDisplayCooldown = -1f;
    //Target Variables
    public int activeTargetCount;
    public List<Vector3> posList;

    float speedMod = 1f;
    void Awake()
    {
        if (Instance == null)
            Instance = this;

        targetSpawnPos = visualHolder.transform.position;
        activeTargetCount = 0;
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

        if (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;
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
    public bool PosUsed(Vector3 testPos)
    {
        foreach (var p in posList)
        {
            if (p == testPos)
            {
                return true;
            }
        }
        return false;
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
        Vector3 targetPos = GetTargetPos();
        while (PosUsed(targetPos))
        {
            targetPos = GetTargetPos();
        }
        posList.Add(targetPos);

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
        if (selectedTarget == 0)
        {
            if (!Abilities.Instance.abilityActive[1])
                Instantiate(omegaTargetPrefab, targetPos, Quaternion.identity);
            else
                Instantiate(omegaDP, targetPos, Quaternion.identity);
        }
        else if (selectedTarget == 1)
        {
            if (!Abilities.Instance.abilityActive[1])
                Instantiate(platTargetPrefab, targetPos, Quaternion.identity);
            else
                Instantiate(platDP, targetPos, Quaternion.identity);
        }
        else if (selectedTarget == 2)
        {
            if (!Abilities.Instance.abilityActive[1])
                Instantiate(goldTargetPrefab, targetPos, Quaternion.identity);
            else
                Instantiate(goldDP, targetPos, Quaternion.identity);
        }
        else if (selectedTarget == 3)
        {
            if (!Abilities.Instance.abilityActive[1])
                Instantiate(silverTargetPrefab, targetPos, Quaternion.identity);
            else
                Instantiate(silverDP, targetPos, Quaternion.identity);
        }
        else
        {
            if (!Abilities.Instance.abilityActive[1])
                Instantiate(redTargetPrefab, targetPos, Quaternion.identity);
            else
                Instantiate(redDP, targetPos, Quaternion.identity);
        }
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
        posList.Remove(target.transform.position);
        Destroy(target);
    }

    public double TargetsToSpawn()
    {
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

    public Vector3 GetTargetPos()
    {
        float targetSpacing = 0.75f;
        return new Vector3(targetSpawnPos.x + (Mathf.Round(UnityEngine.Random.Range(-2f, 2f)) * targetSpacing), targetSpawnPos.y + (Mathf.Round(UnityEngine.Random.Range(-2f, 2f)) * targetSpacing), 0);
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

            if (comboCount >= maxCombo)
            {
                streakColor = new Color(1, 0 , 1);
                if (!fire.activeSelf)
                {
                    HitSound.Instance.source.PlayOneShot(HitSound.Instance.fwoom);
                    fire.SetActive(true);
                }
            }
            else if (comboCount > (maxCombo / 2))
            {
                streakColor = new Color(1, 1 * (1 - ((comboCount - (maxCombo / 2.0f)) / (maxCombo / 2.0f))), 0);
            }
            else
            {
                streakColor = new Color(1, 1, 1 * (1 - (comboCount / (maxCombo / 2.0f))));
            }

            comboText.color = streakColor;
            recordText.color = streakColor;
        }

        comboTimer = comboTime;
    }

    public void GainHitsFromTarget(Collider2D tg)
    {
        if (tg.gameObject.GetComponent<indexNum>().index == 0)
        {
            if (tg.name == "rMid")
            {
                midHit = true;
                RemoveTarget(tg.gameObject.transform.parent.gameObject);
            }
            else
            {
                midHit = false;
                RemoveTarget(tg.gameObject);
            }
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.standardHit, 1f);
            BigDouble amountToGain = TotalClickAmount() * GlobalMultiplier();
            FloatingText.Instance.PopText(amountToGain, new Color32(255, 255, 255, 255), speedMod);
            Vars.Instance.hits += amountToGain;
            Vars.Instance.totalHitCount += amountToGain;
            //Vars.Instance.targets *= 10;
            Vars.Instance.clickTracker += 1;
        }
        else if (tg.gameObject.GetComponent<indexNum>().index == 1)
        {
            if (tg.name == "sMid")
            {
                midHit = true;
                RemoveTarget(tg.gameObject.transform.parent.gameObject);
            }
            else
            {
                midHit = false;
                RemoveTarget(tg.gameObject);
            }
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.silverHit, 1f);
            BigDouble amountToGain = (Vars.Instance.totalHps * Vars.Instance.silverHpsSecs + TotalClickAmount()) * GlobalMultiplier();
            FloatingText.Instance.PopText(amountToGain, new Color32(200, 200, 200, 255), speedMod);
            Vars.Instance.hits += amountToGain;
            Vars.Instance.totalHitCount += amountToGain;
            Vars.Instance.clickTracker += 1;
            Vars.Instance.silverClickTracker += 1;
        }
        else if (tg.gameObject.GetComponent<indexNum>().index == 2)
        {
            if (tg.name == "gMid")
            {
                midHit = true;
                RemoveTarget(tg.gameObject.transform.parent.gameObject);
            }
            else
            {
                midHit = false;
                RemoveTarget(tg.gameObject);
            }
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.goldHit, 1f);
            BigDouble amountToGain = (Vars.Instance.totalHps * Vars.Instance.goldHpsSecs + TotalClickAmount()) * GlobalMultiplier();
            FloatingText.Instance.PopText(amountToGain, new Color32(255, 255, 0, 255), speedMod);
            Vars.Instance.hits += amountToGain;
            Vars.Instance.totalHitCount += amountToGain;
            Vars.Instance.clickTracker += 1;
            Vars.Instance.goldClickTracker += 1;
        }
        else if (tg.gameObject.GetComponent<indexNum>().index == 3)
        {
            if (tg.name == "pMid")
            {
                midHit = true;
                RemoveTarget(tg.gameObject.transform.parent.gameObject);
            }
            else
            {
                midHit = false;
                RemoveTarget(tg.gameObject);
            }
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.platHit, 1f);
            BigDouble amountToGain = (Vars.Instance.totalHps * Vars.Instance.platHpsSecs + TotalClickAmount()) * GlobalMultiplier();
            FloatingText.Instance.PopText(amountToGain, new Color32(0, 214, 186, 255), speedMod);
            Vars.Instance.platClickTracker += 1;
        }
        else if (tg.gameObject.GetComponent<indexNum>().index == 4)
        {
            if (tg.name == "uMid")
            {
                midHit = true;
                RemoveTarget(tg.gameObject.transform.parent.gameObject);
            }
            else
            {
                midHit = false;
                RemoveTarget(tg.gameObject);
            }
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.omegaHit, 0.5f);
            BigDouble amountToGain = (Vars.Instance.totalHps * 3600 + TotalClickAmount()) * GlobalMultiplier();
            FloatingText.Instance.PopText(amountToGain, new Color32(255, 0, 0, 255), speedMod);
            Vars.Instance.hits += amountToGain;
            Vars.Instance.totalHitCount += amountToGain;
            Vars.Instance.clickTracker += 1;
            Vars.Instance.omegaClickTracker += 1;
        }
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
