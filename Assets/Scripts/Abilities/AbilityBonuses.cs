using BreakInfinity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBonuses : MonoBehaviour
{
    public static AbilityBonuses Instance;

    private int bonusTargets = 0;
    private BigDouble clickMultiplier = 1;
    private BigDouble idleMultiplier = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public BigDouble GetClickMultiplier() 
    {
        return clickMultiplier;
    }

    public BigDouble GetIdleMultiplier()
    {
        return idleMultiplier;
    }

    public int GetBonusTargets()
    {
        return bonusTargets;
    }
}
