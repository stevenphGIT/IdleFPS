using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetSlideTexts : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text subtitleText;

    public void SetStrings(string title, string subtitle)
    {
        titleText.text = title;
        subtitleText.text = subtitle;
    }

    public void SetColors(Color titleColor, Color subColor)
    {
        titleText.color = titleColor;
        subtitleText.color = subColor;
    }

    public void SetAll(string title, string subtitle, Color titleColor, Color subColor)
    {
        titleText.text = title;
        subtitleText.text = subtitle;
        titleText.color = titleColor;
        subtitleText.color = subColor;
    }
}
