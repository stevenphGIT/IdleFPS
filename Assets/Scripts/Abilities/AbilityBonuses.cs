using BreakInfinity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityBonuses : MonoBehaviour
{
    public static AbilityBonuses Instance;

    public int bonusTargets = 0;
    public BigDouble clickMultiplier = 1;
    public BigDouble idleMultiplier = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


}
