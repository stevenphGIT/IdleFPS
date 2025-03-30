using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LiftableCard : MonoBehaviour
{
    public TMP_Text nameText;
    public TMP_Text descriptionText;

    public Image icon;

    public Ability heldAbility;

    public Image cardFront;

    public CardHolder slot;

    public bool isHeld = false;
    private Vector3 returnPos;

    void Update()
    {
        if (isHeld)
        {
            transform.position = Crosshair.Instance.transform.position;
            if (Prestige.Instance.inPrestigeAnim) isHeld = false;
            if (SpeechBox.Instance.boxShowing) isHeld = false;
            if (NoticeBox.Instance.activeBox) isHeld = false;
        }

        if (isHeld && Input.GetMouseButtonUp(0))
        {
            isHeld = false;
            HeldCard.Instance.isHolding = false;
            
            CardHolder nearestSlot = FindValidCardSlot();

            if (nearestSlot != null)
            {
                if (nearestSlot == slot)
                {
                    transform.position = returnPos;
                    return;
                }
                if (slot != null)
                {
                    slot.EquipCard(nearestSlot.equippedAbility);
                }
                nearestSlot.EquipCard(this);
                Abilities.Instance.DisplayBinder();
            }
            else
            {
                if (slot != null)
                {
                    slot.Unequip();
                }
                transform.position = returnPos;
                transform.SetParent(Abilities.Instance.binder.transform);
                Abilities.Instance.DisplayBinder();
            }
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "crosshair")
        {
            if (Prestige.Instance.inPrestigeAnim) return;
            if (SpeechBox.Instance.boxShowing) return;
            if (NoticeBox.Instance.activeBox) return;
            if (Input.GetMouseButton(0) && !isHeld)
            {
                if (!HeldCard.Instance.isHolding)
                {
                    isHeld = true;
                    HeldCard.Instance.isHolding = true;
                    transform.SetParent(Abilities.Instance.topObj.transform);
                    transform.SetAsLastSibling();
                }
            }
            if (Input.GetMouseButton(1))
            {
                if (!NoticeBox.Instance.activeBox)
                {
                    string hex = ColorUtility.ToHtmlStringRGB(Abilities.Instance.abilityColor[heldAbility.rarity]);
                    NoticeBox.Instance.SetTitleText("<color=#" + hex + ">Ability: </color>" + heldAbility.abilityName);
                    NoticeBox.Instance.SetTitleColor(UnityEngine.Color.white);
                    NoticeBox.Instance.SetDescriptionText("<color=#" + hex + ">Effect: </color>" + heldAbility.abilityDescription
                        + "\n<color=#" + hex + ">Duration: </color> " + ValToString.Instance.ShortenTime(Abilities.Instance.TrueDuration(heldAbility.abilityDuration))
                        + "\n<color=#" + hex + ">Cooldown: </color> " + ValToString.Instance.ShortenTime(Abilities.Instance.TrueCooldown(heldAbility.abilityCooldown)));
                    NoticeBox.Instance.SetDescriptionColor(UnityEngine.Color.white);
                    NoticeBox.Instance.SetChoosable(false);
                    NoticeBox.Instance.ShowBox();
                    HitSound.Instance.source.PlayOneShot(HitSound.Instance.openMenu, 1f);
                }
            }
        }
    }

    private CardHolder FindValidCardSlot()
    {
        Collider2D[] colliders = Physics2D.OverlapPointAll(GetComponent<Collider2D>().bounds.center);
        foreach (Collider2D col in colliders)
        {
            CardHolder holder = col.GetComponent<CardHolder>();
            if (holder != null)
            {
                return holder;
            }
        }
        return null;
    }
    public void SetReturnPos(Vector3 r)
    {
        returnPos = r;
    }
    public void SetDisplay()
    {
        nameText.text = heldAbility.abilityName;
        descriptionText.text = heldAbility.abilityDescription;
        icon.sprite = heldAbility.abilityIcon;
        cardFront.sprite = PackHandler.Instance.cardFronts[heldAbility.rarity];
    }

    public void SetHeldAbility(Ability a)
    {
        heldAbility = a;
        SetDisplay();
    }
}
