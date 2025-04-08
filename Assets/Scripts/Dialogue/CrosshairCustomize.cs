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

    public SpriteRenderer template, crosshairRend;

    public GameObject crosshair;

    public Animator crosshairAnim, templateCrosshairAnim;

    public Sprite okBox, nothingBox;

    public bool outline;
    public GameObject outlineParent, templateOutlineParent;
    private SpriteRenderer[] outlines;
    private SpriteRenderer[] templateOutlines;
    public GameObject outlineCheckbox, rainbowCheckbox;
    public InputSlider outlineSlider;

    public bool rainbow;
    public InputSlider red, green, blue;
    public float redV, greenV, blueV;

    public InputSlider size;

    public Sprite[] crosshairs;
    public int crosshairNum;
    void Awake()
    {
        if (Instance == null)
            Instance = this;

        outlines = outlineParent.GetComponentsInChildren<SpriteRenderer>();
        templateOutlines = templateOutlineParent.GetComponentsInChildren<SpriteRenderer>();
    }

    void Start()
    {
        UpdateValueOnChange();
        SetCheckboxSprites();
        SetOutlineWidth();
    }

    void Update()
    {
        if (red.valUpdated || green.valUpdated || blue.valUpdated || outlineSlider.valUpdated || size.valUpdated)
        {
            UpdateValueOnChange();
            red.valUpdated = false;
            green.valUpdated = false;
            blue.valUpdated = false;
            outlineSlider.valUpdated = false;
            size.valUpdated = false;
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
        if (template != null && crosshairRend != null)
        {
            crosshair.transform.localScale = new Vector3(size.sliderValue, size.sliderValue, 1);
            template.color = CrosshairColor();
            crosshairRend.color = CrosshairColor();
            template.sprite = crosshairs[crosshairNum];
            crosshairRend.sprite = crosshairs[crosshairNum];
            foreach (SpriteRenderer outlineRenderer in outlines)
            {
                outlineRenderer.sprite = crosshairs[crosshairNum];
            }
            foreach (SpriteRenderer outlineRenderer in templateOutlines)
            {
                outlineRenderer.sprite = crosshairs[crosshairNum];
            }
        }
    }

    public void UpdateValueOnChange()
    {
        ChangeCrosshairImage();
        SetOutlineWidth();
        red.sliderValueText.text = Mathf.Round(red.sliderValue * 255f) + "";
        green.sliderValueText.text = Mathf.Round(green.sliderValue * 255f) + "";
        blue.sliderValueText.text = Mathf.Round(blue.sliderValue * 255f) + "";
        outlineSlider.sliderValueText.text = Mathf.Round(outlineSlider.sliderValue * 10000f) / 10.0 + "";
        size.sliderValueText.text = Mathf.Round(size.sliderValue) + "";
    }
    public void SetCheckboxSprites()
    {
        if (outlineCheckbox.activeSelf)
        {
            if (outline)
            {
                outlineCheckbox.GetComponent<SpriteRenderer>().sprite = okBox;
                outlineParent.SetActive(true);
                templateOutlineParent.SetActive(true);
            }
            else
            {
                outlineCheckbox.GetComponent<SpriteRenderer>().sprite = nothingBox;
                outlineParent.SetActive(false);
                templateOutlineParent.SetActive(false);
            }
        }
        if (rainbowCheckbox.activeSelf)
        {
            if (rainbow)
            {
                rainbowCheckbox.GetComponent<SpriteRenderer>().sprite = okBox;
                crosshairAnim.enabled = true;
                templateCrosshairAnim.enabled = true;
                ChangeCrosshairImage();
            }
            else
            {
                rainbowCheckbox.GetComponent<SpriteRenderer>().sprite = nothingBox;
                crosshairAnim.enabled = false;
                templateCrosshairAnim.enabled = false;
                ChangeCrosshairImage();
            }
        }
    }
    private void SetOutlineWidth()
    {
        float outlineWidth = outlineSlider.sliderValue;
        foreach (Transform child in outlineParent.transform)
        {
            child.localPosition = outlineParent.transform.localPosition;
            if (child.gameObject.name.Contains("Up"))
            {
                child.localPosition += new Vector3(0, outlineWidth, 0);
            }
            if (child.gameObject.name.Contains("Down"))
            {
                child.localPosition += new Vector3(0, -outlineWidth, 0);
            }
            if (child.gameObject.name.Contains("Right"))
            {
                child.localPosition += new Vector3(outlineWidth, 0, 0);
            }
            if (child.gameObject.name.Contains("Left"))
            {
                child.localPosition += new Vector3(-outlineWidth, 0, 0);
            }
        }
        foreach (Transform child in templateOutlineParent.transform)
        {
            child.localPosition = templateOutlineParent.transform.localPosition;
            if (child.gameObject.name.Contains("Up"))
            {
                child.localPosition += new Vector3(0, outlineWidth, 0);
            }
            if (child.gameObject.name.Contains("Down"))
            {
                child.localPosition += new Vector3(0, -outlineWidth, 0);
            }
            if (child.gameObject.name.Contains("Right"))
            {
                child.localPosition += new Vector3(outlineWidth, 0, 0);
            }
            if (child.gameObject.name.Contains("Left"))
            {
                child.localPosition += new Vector3(-outlineWidth, 0, 0);
            }
        }
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
        this.outline = data.outline;
        outlineSlider.sliderValue = data.outlineWidth;
        size.sliderValue = data.crosshairSize;
        this.rainbow = data.rainbow;
    }

    public void SaveData(ref GameData data)
    {
        data.redV = red.sliderValue;
        data.blueV = blue.sliderValue;
        data.greenV = green.sliderValue;
        data.crosshairNum = this.crosshairNum;
        data.outline = this.outline;
        data.outlineWidth = outlineSlider.sliderValue;
        data.crosshairSize = size.sliderValue;
        data.rainbow = this.rainbow;
    }
}
