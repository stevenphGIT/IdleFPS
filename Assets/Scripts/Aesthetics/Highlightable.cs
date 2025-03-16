using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlightable : MonoBehaviour
{
    protected SpriteRenderer rend;
    public bool showDuringDialogue = false;

    private void Awake()
    {
        rend = this.gameObject.GetComponent<SpriteRenderer>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (Prestige.Instance.inPrestigeAnim) return;
        if (!showDuringDialogue && SpeechBox.Instance.boxShowing) return;
        rend.material.SetFloat("_OutlineEnabled", 1);
        rend.material.SetColor("_SolidOutline", Color.white);
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        rend.material.SetFloat("_OutlineEnabled", 0);
    }
}
