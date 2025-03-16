using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BreakInfinity;

[CreateAssetMenu]
public class UpgradeObject : ScriptableObject
{
    public int ID = -1;
    public int pin = -1;
    public BigDouble price;
    public Sprite upgradeIcon;
    public string upgName;
    public string upgDescription;
    public string upgLore;
}
