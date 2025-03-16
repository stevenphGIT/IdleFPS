using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Upgrade : MonoBehaviour
{
    private Image upgradeIconSlot;
    public int upgSlotNum = -1;
    private UpgradeObject displayedUpgrade;
    private Sprite nothing;
    private Vars vars;
    private Textbox textboxRef;
    private AvailableUpgrades upgradeHandler;
    private HitSound hitSound;

    void Start()
    {
        upgradeHandler = GameObject.Find("UpgradeHandler").GetComponent<AvailableUpgrades>();
        vars = GameObject.Find("VarTracker").GetComponent<Vars>();
        textboxRef = GameObject.Find("TextboxHandler").GetComponent<Textbox>();
        hitSound = GameObject.Find("HitAudio").GetComponent<HitSound>();
        upgradeIconSlot = this.transform.GetChild(0).GetComponent<Image>();
        nothing = Resources.Load<Sprite>("Nothing");
    }

    void Update()
    {
        if(upgSlotNum < upgradeHandler.available.Count) {
            displayedUpgrade = upgradeHandler.GetNextUpgrade(upgSlotNum);
            upgradeIconSlot.sprite = displayedUpgrade.upgradeIcon;
        }
        else
        {
            displayedUpgrade = null;
            upgradeIconSlot.sprite = nothing;
        }
    }

    public void ChangeVars()
    {
        if(displayedUpgrade == null)
        {
            textboxRef.HideBox();
        }
        else
        {
            textboxRef.SetTitleColor(Color.cyan);
            textboxRef.SetTitleText(displayedUpgrade.upgName);
            textboxRef.SetDescriptionText(displayedUpgrade.upgDescription);
            textboxRef.SetCostText(vars.GetComponent<Vars>().Abbr(displayedUpgrade.price));
            textboxRef.SetFlavorText(displayedUpgrade.upgLore);
            textboxRef.ShowBox();
        }
    }

    public void BuyUpgrade()
    {
        if(displayedUpgrade != null && vars.hits >= displayedUpgrade.price)
        {
            vars.hits -= displayedUpgrade.price;
            upgradeHandler.Buy(displayedUpgrade);
            hitSound.source.PlayOneShot(hitSound.upgradeBuy, 1f);
        }
        else
            hitSound.source.PlayOneShot(hitSound.cantUse, 1f);
    }

    public void DestroyObject()
    {
        Destroy(this);
    }
}
