using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHolder : MonoBehaviour
{
    public Ability equippedAbility;
    private Collider2D slotCollider;
    void Start()
    {
        slotCollider = GetComponent<Collider2D>();
    }

    public void EquipCard(LiftableCard card)
    {
        equippedAbility = card.heldAbility;
        int num = GetComponent<indexNum>().index;
        Abilities.Instance.slottedAbilities[num] = equippedAbility;
    }

    public void EquipCard(Ability ab)
    {
        equippedAbility = ab;
        int num = GetComponent<indexNum>().index;
        Abilities.Instance.slottedAbilities[num] = equippedAbility;
    }

    public void Unequip()
    {
        equippedAbility = null;
        int num = GetComponent<indexNum>().index;
        Abilities.Instance.slottedAbilities[num] = equippedAbility;
    }
}
