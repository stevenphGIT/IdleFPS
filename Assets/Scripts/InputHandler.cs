using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using BreakInfinity;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.EventSystems;
using Steamworks;
using TMPro.Examples;

public class InputHandler : MonoBehaviour
{
    //Base variables
    public Canvas canvas;
    private Camera mainCamera;
    private int textboxGunId;
    public UpgradeDatabase db;
    //private Tutorial tutorial;
    //GUI Objects
    public GameObject content;
    //Texts
    public Animator settingsBox;
    public Animator statsBox;
    public Animator crosshairBox;

    //For fun stuff
    public List<string> goodbyeMessages = new List<string> { "Leaving so soon?", "Quitter!", "Taking a break?", "Come back soon!", "Don't go!", "Headed out?", "Why not stay a little longer?", ":(", "Lazy!"};

    void Awake()
    {
        textboxGunId = -1;
        mainCamera = Camera.main;
    }
    private void Start()
    {
        //tutorial = GameObject.Find("TutorialHandler").GetComponent<Tutorial>();
    }
    // Update is called once per frame
    void Update()
    {
        if (SpeechBox.Instance.boxShowing) return;
        if (CutsceneHandler.Instance.inCutscene) return;
        if (Input.GetKey(KeyCode.Escape))
        {
            if (NoticeBox.Instance.activeBox)
            {
                NoticeBox.Instance.HideBox();
                NoticeBox.Instance.idNum = -1;
                HitSound.Instance.source.PlayOneShot(HitSound.Instance.closeMenu, 1f);
            }
            if (UserInputBox.Instance.Active())
            {
                UserInputBox.Instance.HideBox();
            }
        }
        var mouseOverTest = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Crosshair.Instance.crosshairPos));
        if (!mouseOverTest.collider || mouseOverTest.collider.CompareTag("OpaqueDialogueBox"))
        {
            Textbox.Instance.HideBox();
            textboxGunId = -1;
            return;
        }
        if (UpsAndVars.Instance.laserActive && Input.GetKey(KeyCode.Space))
        {
            if (mouseOverTest.collider.gameObject.CompareTag("Target"))
            {
                BoardHandler.Instance.GainHitsFromTarget(mouseOverTest.collider);
            }
            else if (mouseOverTest.collider.gameObject.CompareTag("SilverTarget"))
            {
                BoardHandler.Instance.GainHitsFromTarget(mouseOverTest.collider);
            }
            else if (mouseOverTest.collider.gameObject.CompareTag("GoldTarget"))
            {
                BoardHandler.Instance.GainHitsFromTarget(mouseOverTest.collider);
            }
            else if (mouseOverTest.collider.gameObject.CompareTag("PlatTarget"))
            {
                BoardHandler.Instance.GainHitsFromTarget(mouseOverTest.collider);
            }
            else if (mouseOverTest.collider.gameObject.CompareTag("OmegaTarget"))
            {
                BoardHandler.Instance.GainHitsFromTarget(mouseOverTest.collider);
            }
        }
        else if (mouseOverTest.collider.CompareTag("Ability"))
        {
            if (Textbox.Instance.shouldUpdate)
            {
                Abilities.Instance.SetShow(mouseOverTest.collider.GetComponent<indexNum>().index);
                Textbox.Instance.SetUpdated();
            }
        }
        /*else if (mouseOverTest.collider.CompareTag("Weather"))
        {
            if (Textbox.Instance.shouldUpdate)
            {
                LocationManager.Instance.SetShowMap();
                Textbox.Instance.SetUpdated();
            }
        }*/
        else if (mouseOverTest.collider.CompareTag("Upgrade"))
        {
            if (Textbox.Instance.shouldUpdate)
            {
                mouseOverTest.collider.gameObject.GetComponent<Upgrade>().ChangeVars();
                Textbox.Instance.SetUpdated();
            }
        }
        else if (mouseOverTest.collider.CompareTag("Gun"))
        {
            int gunI = mouseOverTest.collider.gameObject.GetComponent<indexNum>().index;
            if (gunI != textboxGunId)
            {
                if (Textbox.Instance.shouldUpdate && Gun.Instance.bought[gunI])
                {
                    Textbox.Instance.HideBox();
                    textboxGunId = gunI;
                    Gun.Instance.SetShow(textboxGunId);
                    Textbox.Instance.SetUpdated();
                }
                else
                {
                    Textbox.Instance.HideBox();
                    textboxGunId = -1;
                }
            }
        }
        else if (mouseOverTest.collider.CompareTag("PrestigeUpg"))
        {
            if (Textbox.Instance.shouldUpdate)
            {
                UpsAndVars.Instance.ShowTextbox(mouseOverTest.collider.gameObject.GetComponent<PrestigeUpgrade>().upgSlotNum);
                Textbox.Instance.SetUpdated();
            }
        }
        else if (mouseOverTest.collider.CompareTag("LocationIcon"))
        {
            if (Textbox.Instance.shouldUpdate)
            {
                LocationManager.Instance.SetShowLevel(mouseOverTest.collider.gameObject.GetComponent<indexNum>().index);
                Textbox.Instance.SetUpdated();
            }
        }
        else if (mouseOverTest.collider.name == "ThoriumPrestigeShop")
        {
            if (Textbox.Instance.shouldUpdate)
            {
                Textbox.Instance.SetTitleColor(Color.cyan);
                Textbox.Instance.SetTitleText("Thorium");
                Textbox.Instance.SetFullDescriptionColor(Color.white);
                Textbox.Instance.SetFullDescriptionText("<color=#00FFFF><sprite index=1>Thorium</color> is used to buy nuclear upgrades. When <color=#00FFFF><sprite index=1>thorium</color> is spent on those upgrades, it leaves behind one <color=#00FF00><sprite index=0>rad</color> per <color=#00FFFF><sprite index=1>thorium</color> spent.");
                Textbox.Instance.SetCostText("");
                Textbox.Instance.ShowBox();
                Textbox.Instance.SetUpdated();
            }
        }
        else if (mouseOverTest.collider.name == "RadPrestigeShop")
        {
            if (Textbox.Instance.shouldUpdate)
            {
                Textbox.Instance.SetTitleColor(Color.green);
                Textbox.Instance.SetTitleText("Rads");
                Textbox.Instance.SetFullDescriptionColor(Color.white);
                Textbox.Instance.SetFullDescriptionText("<color=#00FF00><sprite index=0>Rads</color> are produced by spending <color=#00FFFF><sprite index=1>thorium</color> on upgrades. Each <color=#00FF00><sprite index=0>rad</color> increases ALL hits gained by " + UpsAndVars.Instance.radMult * 100 + "%");
                Textbox.Instance.SetCostText("");
                Textbox.Instance.ShowBox();
                Textbox.Instance.SetUpdated();
            }
        }
        else
        {
            Textbox.Instance.HideBox();
            textboxGunId = -1;
        }
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        if (!context.started) return;

        var rayHit = Physics2D.GetRayIntersection(mainCamera.ScreenPointToRay(Crosshair.Instance.crosshairPos));
        if (rayHit.collider && rayHit.collider.CompareTag("Answer"))
        {
            SpeechBox.Instance.ChooseBranch(rayHit.collider.GetComponent<indexNum>().index);
            return;
        }
        else if (SpeechBox.Instance.boxShowing)
        {
            SpeechBox.Instance.NextLine();
            return;
        }
        if (CutsceneHandler.Instance.inCutscene) return;
        if (!rayHit.collider) return;
        //Alerts
        if (rayHit.collider.name == "SkipCutscene")
        {
            Prestige.Instance.RushCutscene();
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.click, 1f);
        }
        if (Prestige.Instance.inPrestigeAnim) return;

        if (rayHit.collider.name == "noticeDismiss")
        {
            NoticeBox.Instance.HideBox();
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.closeMenu, 1f);
        }
        else if (rayHit.collider.name == "noticeYes")
        {
            NoticeBox.Instance.HideBox();
            if (NoticeBox.Instance.idNum == 0)
                Prestige.Instance.Press();
            else if (NoticeBox.Instance.idNum == 1)
                Application.Quit();
            else if (NoticeBox.Instance.idNum == 2)
            {
                Debug.Log("Removed feature (DUELS) called.");
            }
            else if (NoticeBox.Instance.idNum == 3)
            {
                Prestige.Instance.ApplyPersistenceUpgrades();
                Prestige.Instance.PanHomeEndPrestige();
                HitSound.Instance.source.PlayOneShot(HitSound.Instance.closeMenu, 1f);
            }
            /*else if (notice.idNum == 100)
            {
                tutorial.tutorialStep = -1;
            }*/
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.closeMenu, 1f);
        }
        else if (rayHit.collider.name == "noticeNo")
        {
            NoticeBox.Instance.HideBox();
            NoticeBox.Instance.idNum = -1;
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.closeMenu, 1f);
        }
        if (NoticeBox.Instance.activeBox)
        {
            return;
        }
        if (rayHit.collider.gameObject.CompareTag("Cheat"))
        {
            for (int i = 0; i < db.Size(); i++)
            {
                AvailableUpgrades.Instance.GetComponent<AvailableUpgrades>().Unlock(db.findName(i));
            }
            Vars.Instance.hits *= 10;
        }
        else if (rayHit.collider.name == "returnButton")
        {
            if (Vars.Instance.thorium > 0 && Vars.Instance.rads == 0)
            {
                HitSound.Instance.source.PlayOneShot(HitSound.Instance.cantUse, 1f);
                NoticeBox.Instance.SetTitleColor(Color.white);
                NoticeBox.Instance.SetTitleText("Sorry!");
                NoticeBox.Instance.SetDescriptionText("You haven't spent any <color=#00FFFF><sprite index=1>thorium</color>, are you sure you'd like to return? <color=#FFFF00>If you return now, you will earn no bonuses from your nuclear reset.</color>");
                NoticeBox.Instance.SetChoosable(true);
                NoticeBox.Instance.idNum = 3;
                NoticeBox.Instance.ShowBox();
                return;
            }
            Prestige.Instance.ApplyPersistenceUpgrades();
            Prestige.Instance.PanHomeEndPrestige();
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.closeMenu, 1f);
        }
        else if (rayHit.collider.tag == "PrestigeButton")
        {
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.openMenu, 1f);
            if (Prestige.Instance.ThoriumToGain() > 0)
            {
                NoticeBox.Instance.SetTitleColor(Color.red);
                NoticeBox.Instance.SetTitleText("!WARNING!");
                NoticeBox.Instance.SetDescriptionText("This will reset all guns, hits, and bought upgrades. You would earn " + Vars.Instance.TotalAbbr(Prestige.Instance.ThoriumToGain()) + " <color=#00FFFF><sprite index=1>thorium</color>. Would you like to proceed?");
                NoticeBox.Instance.SetChoosable(true);
                NoticeBox.Instance.idNum = 0;
                NoticeBox.Instance.ShowBox();
            }
            else
            {
                NoticeBox.Instance.SetTitleColor(Color.blue);
                NoticeBox.Instance.SetTitleText("Sorry!");
                NoticeBox.Instance.SetDescriptionText("You do not have enough hits to earn thorium from resetting.");
                NoticeBox.Instance.SetChoosable(false);
                NoticeBox.Instance.ShowBox();
            }
        }
        else if (rayHit.collider.name == "ShootFrame")
        {
            if (Abilities.Instance.abilityActive[2])
            {
                HitSound.Instance.source.PlayOneShot(HitSound.Instance.standardHit, 1f);
                BoardHandler.Instance.BoardClick();
            }
            if (BoardHandler.Instance.comboCount > 4)
            {
                FloatingText.Instance.PopText("MISS!", Color.red, 0);
                HitSound.Instance.source.PlayOneShot(HitSound.Instance.comboEnd);
            }
            BoardHandler.Instance.EndCombo();
        }
        else if (rayHit.collider.gameObject.CompareTag("Close"))
        {
            PromptQuit();
        }
        else if (rayHit.collider.gameObject.name == "CreditsButton")
        {
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.openMenu, 1f);
            NoticeBox.Instance.SetTitleColor(Color.yellow);
            NoticeBox.Instance.SetTitleText("!Special Thanks!");
            string description = "Background Artist: Luke L" +
                "\n\nPlaytesters: Awesomaniac13, McCatter, Qellio, Xander H";
            if (SteamManager.Initialized)
            {
                description += "\n AND YOU! " + SteamFriends.GetPersonaName() + "!";
            }
            NoticeBox.Instance.SetDescriptionText(description);
            NoticeBox.Instance.SetChoosable(false);
            NoticeBox.Instance.ShowBox();
        }
        //Ability Click
        else if (rayHit.collider.gameObject.CompareTag("Ability"))
        {
            Abilities.Instance.UseSelectedAbility();
        }
        else if (rayHit.collider.gameObject.name == "AbilityBot")
        {
            Abilities.Instance.ToggleBot();
        }
        //Target Clicks
        else if (rayHit.collider.gameObject.CompareTag("Target"))
        {
            BoardHandler.Instance.GainHitsFromTarget(rayHit.collider);
            if (AvailableUpgrades.Instance.combosUnlocked)
                BoardHandler.Instance.IncrementCombo();
        }
        else if (rayHit.collider.gameObject.CompareTag("SilverTarget"))
        {
            BoardHandler.Instance.GainHitsFromTarget(rayHit.collider);
            if (AvailableUpgrades.Instance.combosUnlocked)
                BoardHandler.Instance.IncrementCombo();
        }
        else if (rayHit.collider.gameObject.CompareTag("GoldTarget"))
        {
            BoardHandler.Instance.GainHitsFromTarget(rayHit.collider);
            if (AvailableUpgrades.Instance.combosUnlocked)
                BoardHandler.Instance.IncrementCombo();
        }
        else if (rayHit.collider.gameObject.CompareTag("PlatTarget"))
        {
            BoardHandler.Instance.GainHitsFromTarget(rayHit.collider);
            if (AvailableUpgrades.Instance.combosUnlocked)
                BoardHandler.Instance.IncrementCombo();
        }
        else if (rayHit.collider.gameObject.CompareTag("OmegaTarget"))
        {
            BoardHandler.Instance.GainHitsFromTarget(rayHit.collider);
            if (AvailableUpgrades.Instance.combosUnlocked)
                BoardHandler.Instance.IncrementCombo();
        }
        //World Map Buttons
        else if (rayHit.collider.gameObject.CompareTag("LocationAction"))
        {
            LocationManager.Instance.ClickLocationButton(rayHit.collider.gameObject.GetComponent<indexNum>().index);
        }
        else if (rayHit.collider.name == "LeftLocationArrow")
        {
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.click);
            LocationManager.Instance.startIndex--;
            LocationManager.Instance.SetLocationTexts();
        }
        else if (rayHit.collider.name == "RightLocationArrow")
        {
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.click);
            LocationManager.Instance.startIndex++;
            LocationManager.Instance.SetLocationTexts();
        }
        else if (rayHit.collider.name == "MapIcon")
        {
            UserInputBox.Instance.ShowBox(4);
            LocationManager.Instance.SetLocationTexts();
        }
        //Card Buttons
        else if (rayHit.collider.name == "commonRoll")
        {
            PackHandler.Instance.OpenPack(0);
        }
        else if (rayHit.collider.name == "rareRoll")
        {
            PackHandler.Instance.OpenPack(1);
        }
        else if (rayHit.collider.name == "uberRareRoll")
        {
            PackHandler.Instance.OpenPack(2);
        }
        else if (rayHit.collider.name == "DismissCards")
        {
            PackHandler.Instance.CollectCards();
        }
        //Alley Interactions
        else if (rayHit.collider.name == "AlleyIcon")
        {
            Music.Instance.Pause();
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.openMenu);
            CameraViewHandler.Instance.MoveCamToPos("alley");
        }
        else if (rayHit.collider.CompareTag("NPC"))
        {
            rayHit.collider.gameObject.GetComponent<NPC>().Click();
        }
        //Crosshair Customize Buttons
        else if (rayHit.collider.name == "CloseMenu")
        {
            UserInputBox.Instance.HideBox();
        }
        else if (rayHit.collider.name == "CrosshairButton")
        {
            UserInputBox.Instance.ShowBox(2);
        }
        else if (rayHit.collider.name == "changeIconBox")
        {
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.click, 1f);
            CrosshairCustomize.Instance.IncrementCrosshairNum();
        }
        //Music Button
        else if (rayHit.collider.name == "MusicButton")
        {
            Music.Instance.SkipSong();
        }
        //Settings Buttons
        else if (rayHit.collider.name == "SettingsIcon")
        {
            UserInputBox.Instance.ShowBox(0);
        }
        else if (rayHit.collider.name == "fullscreenCheck")
        {
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.click, 1f);
            Options.Instance.fs = !Options.Instance.fs;
            Options.Instance.SetCheckboxSprites();
        }
        else if (rayHit.collider.name == "vSyncCheck")
        {
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.click, 1f);
            Options.Instance.vs = !Options.Instance.vs;
            Options.Instance.SetCheckboxSprites();
        }
        else if (rayHit.collider.name == "targetExplodeCheck")
        {
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.click, 1f);
            Options.Instance.targetExplode = !Options.Instance.targetExplode;
            Options.Instance.SetCheckboxSprites();
        }
        else if (rayHit.collider.name == "applyChangesButton")
        {
            Options.Instance.ApplyGraphics();
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.upgradeBuy, 1f);
        }
        //Statistics Buttons
        else if (rayHit.collider.name == "StatsIcon")
        {
            UserInputBox.Instance.ShowBox(1);
        }
        //Gun Shop Clicks
        else if (rayHit.collider.gameObject.CompareTag("Gun"))
        {
            Gun.Instance.ShopClick(rayHit.collider.GetComponent<indexNum>().index);
        }

        //Upgrade Shop Clicks
        else if (rayHit.collider.name == "ShopIcon")
        {
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.openMenu);
            CameraViewHandler.Instance.MoveCamToPos("shop");
        }
        else if (rayHit.collider.name == "BuyAllButton")
        {
            AvailableUpgrades.Instance.BuyAll();
        }
        else if (rayHit.collider.CompareTag("Upgrade"))
        {
            rayHit.collider.gameObject.GetComponent<Upgrade>().BuyUpgrade();
        }
        else if (rayHit.collider.CompareTag("PrestigeUpg"))
        {
            UpsAndVars.Instance.BuyUpgrade(rayHit.collider.gameObject.GetComponent<PrestigeUpgrade>().upgSlotNum);
        }
        else if (rayHit.collider.CompareTag("LocationIcon"))
        {
            LocationManager.Instance.ClickIcon(rayHit.collider.gameObject.GetComponent<indexNum>().index);
        }
        else if (rayHit.collider.name == "DeckIcon")
        {
            Music.Instance.Pause();
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.deckOpen);
            CameraViewHandler.Instance.MoveCamToPos("card");
        }
        else if (rayHit.collider.name == "HomeIcon")
        {
            Music.Instance.Resume();
            HitSound.Instance.source.PlayOneShot(HitSound.Instance.deckClose);
            CameraViewHandler.Instance.MoveCamToPos("home");
        }
        //Fun stuff
        else if (rayHit.collider.name == "Snowman")
        {
            Snowman.Instance.Click();
        }
    }

    public void PromptQuit()
    {
        HitSound.Instance.source.PlayOneShot(HitSound.Instance.openMenu, 1f);
        NoticeBox.Instance.SetTitleColor(Color.red);
        NoticeBox.Instance.SetTitleText(goodbyeMessages[UnityEngine.Random.Range(0, goodbyeMessages.Count)]);
        NoticeBox.Instance.SetDescriptionText("Are you sure you want to quit? Your progress will be saved.");
        NoticeBox.Instance.SetChoosable(true);
        NoticeBox.Instance.idNum = 1;
        NoticeBox.Instance.ShowBox();
    }
}
