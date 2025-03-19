using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using UnityEngine;

public class InputSlider : MonoBehaviour
{
    public TMP_Text sliderValueText;
    public float sliderDefaultValue;
    public float sliderMinValue;
    public float sliderMaxValue;
    public float sliderValue;

    public bool sliderSnap;

    public GameObject leftBorder, rightBorder;

    public Collider2D scrollRect;
    private bool scrollable = false;

    private float distance;

    bool clickedOn = false;

    public bool vertical = false;

    public bool valUpdated = true;
    private void Start()
    {
        if (scrollRect != null)
        { 
            scrollable = true;
        }
        float leftPos;
        float rightPos;
        if (!vertical)
        {
            leftPos = leftBorder.transform.position.x;
            rightPos = rightBorder.transform.position.x;
        }
        else
        {
            leftPos = leftBorder.transform.position.y;
            rightPos = rightBorder.transform.position.y;
        }
        distance = Mathf.Abs(leftPos - rightPos);
        if (sliderValue == -1)
        {
            sliderValue = sliderDefaultValue;
        }
        UpdateBar();
    }
    void Update()
    {
        if (clickedOn)
        {
            if (Input.GetMouseButton(0))
            {
                UpdateSliderPosition(Crosshair.Instance.transform.position);
            }
            else
            {
                clickedOn = false;
            }
        }
        if (scrollable && scrollRect.OverlapPoint(Crosshair.Instance.transform.position))
        {
            float scrollDelta = Input.mouseScrollDelta.y;
            if (scrollDelta != 0)
            {
                float prevSliderVal = sliderValue;

                sliderValue += scrollDelta * (sliderMaxValue - sliderMinValue) * 0.05f;
                sliderValue = Mathf.Clamp(sliderValue, sliderMinValue, sliderMaxValue);

                if (sliderSnap)
                    sliderValue = Mathf.Round(sliderValue);

                UpdateBar();

                if (sliderValue != prevSliderVal)
                {
                    valUpdated = true;
                }
            }
        }
    }
    void UpdateSliderPosition(Vector3 crosshairPos)
    {
        float prevSliderVal = sliderValue;
        float tempVal;
        float xMax, xMin, xVal;

        if (!vertical)
        {
            xMax = rightBorder.transform.position.x;
            xMin = leftBorder.transform.position.x;
            xVal = Mathf.Clamp(crosshairPos.x, xMin, xMax);
            this.transform.position = new Vector3(xVal, this.transform.position.y);
        }
        else
        {
            xMax = rightBorder.transform.position.y;
            xMin = leftBorder.transform.position.y;
            xVal = Mathf.Clamp(crosshairPos.y, xMin, xMax);
            this.transform.position = new Vector3(this.transform.position.x, xVal);
        }

        tempVal = sliderMinValue + (((xVal - xMin) / distance) * (sliderMaxValue - sliderMinValue));

        if (sliderSnap)
            tempVal = Mathf.Round(tempVal);

        sliderValue = tempVal;
        UpdateBar();

        if (sliderValue != prevSliderVal)
        {
            valUpdated = true;
        }
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "crosshair")
            if (Input.GetMouseButton(0))
            {
                clickedOn = true;
            }
    }
    public void UpdateBar()
    {
        if (!vertical)
        {
            float leftPos = leftBorder.transform.position.x;
            float rightPos = rightBorder.transform.position.x;
            this.transform.position = new Vector3(leftPos + (distance * (sliderValue - sliderMinValue)) / (sliderMaxValue - sliderMinValue), this.transform.position.y);
        }
        else
        {
            float leftPos = leftBorder.transform.position.y;
            float rightPos = rightBorder.transform.position.y;
            this.transform.position = new Vector3(this.transform.position.x, leftPos + (distance * (sliderValue - sliderMinValue)) / (sliderMaxValue - sliderMinValue));
        }
    }
}
