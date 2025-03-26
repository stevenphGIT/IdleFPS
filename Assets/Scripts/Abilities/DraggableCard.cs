using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DraggableCard : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text cooldownText;

    private float cooldownTime;
    private float upTime;

    public Image icon;
    public SpriteRenderer front;
    public Ability heldAbility;

    public Animator animator;

    public SpriteRenderer cardFront;

    private void Update()
    {
        this.transform.position = Crosshair.Instance.crosshairPos;
        if (!Input.GetMouseButton(0))
        {
            Destroy(this.gameObject);
        }
    }
    public void SetDisplay()
    {
        nameText.text = heldAbility.abilityName;
        descriptionText.text = heldAbility.abilityDescription;
        cooldownText.text = ValToString.Instance.ShortenTime(upTime);
        icon.sprite = heldAbility.abilityIcon;
        cardFront.sprite = PackHandler.Instance.cardFronts[heldAbility.rarity];
    }

    public void SetHeldAbility(Ability a)
    {
        heldAbility = a;
        SetVars();
        SetDisplay();
    }

    public void SetVars()
    {
        double durationMult = 1;
        if (LocationManager.Instance.activeLocation == 4 && AvailableUpgrades.Instance.locationBonuses)
        {
            durationMult *= (1 + (LocationManager.Instance.locations[4].GetMult() * UpsAndVars.Instance.locationBonus));
            durationMult *= UpsAndVars.Instance.duration;
        }
        cooldownTime = (float)((heldAbility.abilityCooldown / UpsAndVars.Instance.cooldown) + (heldAbility.abilityDuration * durationMult));
        upTime = (float)(heldAbility.abilityDuration * durationMult);
    }
}
