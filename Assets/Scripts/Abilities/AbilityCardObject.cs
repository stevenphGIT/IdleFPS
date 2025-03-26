using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityCardObject : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text descriptionText;
    public TMP_Text cooldownText;

    private float cooldownTime;
    private float upTime;

    public Image icon;
    public SpriteRenderer front, back;
    public Ability heldAbility;

    public Animator animator;

    public SpriteRenderer cardFront;
    public SpriteRenderer cardBack;

    private bool shouldShake = false;
    private bool isShowing = false;
    private bool counted = false;

    private float startX;
    private float startY;

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "crosshair")
        {
            if (!isShowing)
            {
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("FlipCardToFront") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1.0f)
                    isShowing = true;
                else if(shouldShake)
                {
                    float shakeX = startX + (Random.value - 0.5f) * 0.1f;
                    float shakeY = startY + (Random.value - 0.5f) * 0.1f;
                    this.transform.position = new Vector3(shakeX, shakeY, 0);
                }
            }
            if (Input.GetMouseButton(0))
            {
                if (!isShowing)
                {
                    if (shouldShake)
                    {
                        shouldShake = false;
                        this.transform.position = new Vector3(startX, startY, 0);
                        animator.Play("FlipCardToFront");
                        if (!counted)
                        {
                            PackHandler.Instance.FlipCard();
                            counted = true;
                        }
                    }
                }
            }
        }
    }

    public void AllowShake() 
    {
        startX = transform.position.x;
        startY = transform.position.y;
        shouldShake = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(shouldShake)
            this.transform.position = new Vector3(startX, startY, 0);
    }
    public void SetDisplay()
    {
        nameText.text = heldAbility.abilityName;
        descriptionText.text = heldAbility.abilityDescription;
        cooldownText.text = ValToString.Instance.ShortenTime(upTime);
        icon.sprite = heldAbility.abilityIcon;
        cardFront.sprite = PackHandler.Instance.cardFronts[heldAbility.rarity];
        cardBack.sprite = PackHandler.Instance.cardBacks[heldAbility.rarity];
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
