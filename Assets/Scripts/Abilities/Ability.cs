using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BreakInfinity;

[CreateAssetMenu]
public class Ability : ScriptableObject
{
    public int id;
    public Sprite abilityIcon;
    public string abilityName;
    public string abilityDescription;
    public float abilityCooldown;
    public float abilityDuration;
    public int rarity;
    /*
     * 0 - Common
     * 1 - Uncommon
     * 2 - Rare
     * 3 - Epic
     * 4 - Legendary
     * 5 - Mythic
     */
}

