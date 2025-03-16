using BreakInfinity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Location
{
    private int locationID;
    private string locationName;
    private string locationDescription;
    private BigDouble locationCost;
    private Color locationColor;
    private Color locationSecondaryColor;
    private bool locationRevealed;
    private bool locationOwned;
    private double locationBonusMultiplier;
    private int locationLevel;
    public Location(int id, string name, string desc, BigDouble cost, Color lColor, Color lSecondColor, double mult)
    {
        locationID = id;
        locationName = name;
        locationDescription = desc;
        locationCost = cost;
        locationColor = lColor;
        locationSecondaryColor = lSecondColor;
        locationRevealed = false;
        locationOwned = false;
        locationBonusMultiplier = mult;
        locationLevel = 0;
    }
    public Location(int id, string name, string desc, BigDouble cost, Color lColor, Color lSecondColor, bool revealed, bool owned, double mult)
    {
        locationID = id;
        locationName = name;
        locationDescription = desc;
        locationCost = cost;
        locationColor = lColor;
        locationSecondaryColor = lSecondColor;
        locationRevealed = revealed;
        locationOwned = owned;
        locationBonusMultiplier = mult;
        locationLevel = 0;
    }
    public string GetName()
    {
        return locationName;
    }
    public string GetDesc()
    {
        return locationDescription;
    }
    public int GetID()
    {
        return locationID;
    }
    public BigDouble GetCost()
    {
        return locationCost;
    }
    public void SetCost(BigDouble cost)
    {
        locationCost = cost;
    }
    public Color GetColor()
    {
        return locationColor;
    }
    public Color GetSecondColor()
    {
        return locationSecondaryColor;
    }
    public bool ShouldReveal()
    {
        return locationRevealed;
    }
    public void SetRevealed(bool reveal)
    {
        locationRevealed = reveal;
    }
    public bool IsOwned()
    {
        return locationOwned;
    }
    public void SetOwned(bool owned)
    {
        locationOwned = owned;
    }
    public double GetMult()
    {
        return locationBonusMultiplier * (1 + ((locationLevel) * 0.1f));
    }

    public int GetLevel()
    {
        return locationLevel;
    }

    public void SetLevel(int level)
    {
        locationLevel = level;
    }
}
