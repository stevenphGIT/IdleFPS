using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleableButton : MonoBehaviour
{
    public Sprite availableSprite, lockedSprite;

    private Collider2D col;
    private SpriteRenderer rend;

    public void SetLocked(bool locked)
    {
        col = GetComponent<Collider2D>();
        rend = GetComponent<SpriteRenderer>();
        if (locked)
        {
            col.enabled = false;
            rend.sprite = lockedSprite;
        }
        else
        {
            col.enabled = true;
            rend.sprite = availableSprite;
        }
    }
}
