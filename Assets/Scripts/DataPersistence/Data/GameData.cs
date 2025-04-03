using System.Collections.Generic;
using BreakInfinity;

[System.Serializable]
public class GameData
{
    public BigDouble targets;
    public BigDouble thorium;
    public BigDouble rads;
    public int tokens;
    public BigDouble totalHitCount;
    public List<int> availablePins;
    public List<int> boughtPins;
    public List<BigDouble> targetPowers;
    public BigDouble bonusMult;
    public int clickTracker;
    public int silverClickTracker;
    public int goldClickTracker;
    public int platClickTracker;
    public int omegaClickTracker;
    public BigDouble totalHPS;
    //Stat Vars
    public BigDouble totalTotalHitCount;
    public int totalClickTracker;
    public int totalSilverClickTracker;
    public int totalGoldClickTracker;
    public int totalPlatClickTracker;
    public int totalOmegaClickTracker;
    //Dialogue Tracker
    public List<int> addedDialogueIDs;
    //Abilities
    public int[] slottedAbilityIDs;
    public List<int> ownedAbilityIDs;

    public List<float> remainingCooldown;
    public List<float> remainingDuration;
    public List<bool> abilityActive;
    public int abilitiesUsed;
    //Offline Progress Vars
    public double timeSaved;
    public string timeStartedString;
    //Settings Vars
    public bool fs;
    public bool vs;
    public bool targetExplode;
    public float masterV, musicV, effectsV, sensV;
    //Weather
    public int activeLocation;
    public List<int> ownedLocations;
    public List<int> revealedLocations;
    public List<int> locationLevels;
    public float locationCooldown;
    //Other
    public int tutorialStep;
    public bool inPrestigeShop;
    public List<int> upgradeStatus;
    public int duelWins;
    public int comboRecord;
    //Crosshair
    public float redV, blueV, greenV;
    public int crosshairNum;
    //AbBot On
    public bool abBotOn;
    //Fun
    public int snowmenSlain;

    public GameData()
    {
        //Generic Initialize
        this.availablePins = new List<int>();
        this.boughtPins = new List<int>();
        this.targets = 0;
        this.totalHitCount = 0;
        this.clickTracker = 0;
        //Stat Vars
        this.totalTotalHitCount = 0;
        this.totalClickTracker = 0;
        this.totalSilverClickTracker = 0;
        this.totalGoldClickTracker = 0;
        this.totalPlatClickTracker = 0;
        this.totalOmegaClickTracker = 0;

        this.silverClickTracker = 0;
        this.goldClickTracker = 0;
        this.platClickTracker = 0;
        this.omegaClickTracker = 0;
        this.totalHPS = 0;
        //Settings Initialize
        this.fs = true;
        this.vs = true;
        this.targetExplode = true;
        this.masterV = 1;
        this.musicV = 1; 
        this.effectsV = 1;
        this.sensV = 1;
        //Crosshair Initialize
        this.redV = 1;
        this.blueV = 1;
        this.greenV = 1;
        this.crosshairNum = 0;
        //Offline Initialize
        this.timeStartedString = string.Empty;
        //Guns
        this.targetPowers = new List<BigDouble>() {};
        //Abilities
        this.remainingCooldown = new List<float>() {0, 0, 0, 0, 0};
        this.remainingDuration = new List<float>() {0, 0, 0, 0, 0};
        this.abilityActive = new List<bool>() {false, false, false, false, false};
        this.ownedAbilityIDs = new List<int>();
        this.slottedAbilityIDs = new int[5];
        this.abilitiesUsed = 0;
        //Weather
        this.locationCooldown = 0;
        this.activeLocation = 0;
        this.ownedLocations = new List<int> { };
        this.revealedLocations = new List<int> { };
        this.locationLevels = new List<int> { };
        //Other
        this.tutorialStep = 0;
        this.upgradeStatus = new List<int>(new int[75]);
        this.duelWins = 0;
        this.comboRecord = 0;

        //Prestige
        this.inPrestigeShop = false;
        this.thorium = 0;
        this.rads = 0;

        //Token Stuff
        this.tokens = 0;

        this.abBotOn = false;
        //Dialogue tracker
        this.addedDialogueIDs = new List<int>();
        //Fun
        this.snowmenSlain = 0;
    }
}
