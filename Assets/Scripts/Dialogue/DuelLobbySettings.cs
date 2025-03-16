using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class DuelLobbySettings : MonoBehaviour
{
    public static DuelLobbySettings Instance;
    //Data
    public int bigDoubleExponent;
    public bool useTotalHits = false;
    //Objects
    public Sprite okBox, nothingBox;
    public GameObject totalHitCheckbox;
    public InputSlider hitSlider;
    public TMP_Text hitText;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        UpdateValueOnChange();
    }

    private void Update()
    {
        if (hitSlider.valUpdated)
            UpdateValueOnChange();
    }

    public void UpdateValueOnChange()
    {
        int exp = (int)hitSlider.sliderValue;
        if (hitText != null)
        {
            hitText.text = Vars.Instance.PriceAbbr(new BreakInfinity.BigDouble(10, (exp - 1)));
        }
    }

    public void SwitchTHBox()
    {
        useTotalHits = !useTotalHits;

        if (useTotalHits)
            totalHitCheckbox.GetComponent<SpriteRenderer>().sprite = okBox;
        else
            totalHitCheckbox.GetComponent<SpriteRenderer>().sprite = nothingBox;
    }
}
