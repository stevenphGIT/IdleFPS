using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderRect : MonoBehaviour
{
    public GameObject content;
    public Transform top, bottom;

    public InputSlider slider;

    public float minHeight, maxHeight;

    private void Update()
    {
        if (slider.valUpdated)
        {

            float val = (-1 * Mathf.Lerp(minHeight, maxHeight, slider.sliderValue));

            content.transform.position = new Vector3(content.transform.position.x, val, content.transform.position.z);
            slider.valUpdated = false;
        }
    }
}
