using BreakInfinity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PrestigeUpgradeObject : ScriptableObject
{
    public int id;
    public BigDouble thoriumPrice;
    public string upgName;
    public string upgDescription;
    public string upgLore;
    public List<int> upgradesUnlocked;
}
