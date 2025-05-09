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
    public string[] slottedAbilityIDs;
    public List<string> ownedAbilityIDs;

    public List<float> remainingCooldown;
    public List<float> remainingDuration;
    public int abilitiesUsed;
    public int eggUses;
    public float autoCursorSpeed;
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
    public int comboRecord;
    //Crosshair
    public float redV, blueV, greenV;
    public int crosshairNum;
    public bool outline;
    public float outlineWidth;
    public float crosshairSize;
    public bool rainbow;
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
        this.outline = true;
        this.outlineWidth = 0.005f;
        this.crosshairSize = 250f;
        this.rainbow = false;
        //Offline Initialize
        this.timeStartedString = string.Empty;
        //Guns
        this.targetPowers = new List<BigDouble>() {};
        //Abilities
        this.remainingCooldown = new List<float>() {0, 0, 0, 0, 0};
        this.remainingDuration = new List<float>() {0, 0, 0, 0, 0};
        this.ownedAbilityIDs = new List<string>();
        this.slottedAbilityIDs = new string[5];
        this.abilitiesUsed = 0;
        this.eggUses = 0;
        this.autoCursorSpeed = 1f;
        //Weather
        this.locationCooldown = 0;
        this.activeLocation = 0;
        this.ownedLocations = new List<int> { };
        this.revealedLocations = new List<int> { };
        this.locationLevels = new List<int> { };
        //Other
        this.tutorialStep = 0;
        this.upgradeStatus = new List<int>(new int[75]);
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
