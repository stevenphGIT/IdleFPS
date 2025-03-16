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

    private float distance;

    bool clickedOn = false;

    public bool vertical = false;

    public bool valUpdated = true;
    private void Start()
    {
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
                float prevSliderVal = sliderValue;

                float tempVal;

                float xMax;
                float xMin;
                float xVal;

                if (!vertical)
                {
                    xMax = rightBorder.transform.position.x;
                    xMin = leftBorder.transform.position.x;
                    xVal = Mathf.Clamp(Crosshair.Instance.transform.position.x, xMin, xMax);

                    this.transform.position = new(xVal, this.transform.position.y);
                }
                else
                {
                    xMax = rightBorder.transform.position.y;
                    xMin = leftBorder.transform.position.y;
                    xVal = Mathf.Clamp(Crosshair.Instance.transform.position.y, xMin, xMax);

                    this.transform.position = new(this.transform.position.x, xVal);
                }

                tempVal = sliderMinValue + (((xVal - xMin) / distance) * (sliderMaxValue - sliderMinValue));

                if(sliderSnap)
                    tempVal = Mathf.Round(tempVal);

                sliderValue = tempVal;

                UpdateBar();

                if (sliderValue != prevSliderVal)
                {
                    valUpdated = true;
                }
            }
            else
            {
                clickedOn = false;
            }
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
