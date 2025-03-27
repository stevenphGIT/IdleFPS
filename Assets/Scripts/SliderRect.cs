using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderRect : MonoBehaviour
{
    public GameObject content;
    public GameObject sliderParent;
    public Transform top, bottom;
    private float dTop, dBot;
    public Transform objectTop, objectBottom;
    public InputSlider slider;

    private float minHeight, maxHeight;
    private bool forceUpdate = false;
    private void Start()
    {
        UpdateBounds();
    }

    private void Update()
    {
        if (slider.valUpdated || forceUpdate)
        {
            forceUpdate = false;
            float val = Mathf.Lerp(minHeight, maxHeight, slider.sliderValue);

            content.transform.position = new Vector3(
                content.transform.position.x,
                val,
                content.transform.position.z
            );

            slider.valUpdated = false;
        }
    }

    public void ResetSliderPos()
    {
        forceUpdate = true;
        UpdateBounds();
    }

    private void UpdateBounds()
    {
        dTop = objectTop.transform.position.y - content.transform.position.y;
        dBot = content.transform.position.y - objectBottom.transform.position.y;

        maxHeight = top.position.y - dTop;
        minHeight = bottom.position.y + dBot;

        if (objectBottom.position.y > bottom.position.y && objectTop.position.y <= top.position.y)
        {
            sliderParent.SetActive(false);
            slider.sliderValue = 1;
        }
        else
        {
            sliderParent.SetActive(true);
        }
    }
}
