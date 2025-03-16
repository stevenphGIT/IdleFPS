using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class CrosshairCustomize : MonoBehaviour, IDataPersistence
{
    public static CrosshairCustomize Instance;

    public TMP_Text numText;

    public SpriteRenderer template, crosshair;
    public InputSlider red, green, blue;
    public float redV, greenV, blueV;

    public Sprite[] crosshairs;
    public int crosshairNum;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        UpdateValueOnChange();
    }

    void Update()
    {
        if (red.valUpdated || green.valUpdated || blue.valUpdated)
        {
            UpdateValueOnChange();
            red.valUpdated = false;
            green.valUpdated = false;
            blue.valUpdated = false;
        }
    }

    public void IncrementCrosshairNum()
    {
        if (crosshairNum >= 8)
            crosshairNum = 0;
        else
            crosshairNum++;

        ChangeCrosshairImage();
    }

    public void ChangeCrosshairImage()
    {
        numText.text = (crosshairNum + 1).ToString();
        if (template != null && crosshair != null)
        {
            template.color = CrosshairColor();
            crosshair.color = CrosshairColor();
            template.sprite = crosshairs[crosshairNum];
            crosshair.sprite = crosshairs[crosshairNum];
        }
    }

    public void UpdateValueOnChange()
    {
        ChangeCrosshairImage();
        red.sliderValueText.text = Mathf.Round(red.sliderValue * 255f) + "";
        green.sliderValueText.text = Mathf.Round(green.sliderValue * 255f) + "";
        blue.sliderValueText.text = Mathf.Round(blue.sliderValue * 255f) + "";
    }
    public Color CrosshairColor()
    {
        return new Color(red.sliderValue, green.sliderValue, blue.sliderValue);
    }

    public void LoadData(GameData data)
    {
        red.sliderValue = data.redV;
        blue.sliderValue = data.blueV;
        green.sliderValue = data.greenV;
        this.crosshairNum = data.crosshairNum;
    }

    public void SaveData(ref GameData data)
    {
        data.redV = red.sliderValue;
        data.blueV = blue.sliderValue;
        data.greenV = green.sliderValue;
        data.crosshairNum = this.crosshairNum;
    }
}
