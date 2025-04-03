using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorFunctions : MonoBehaviour
{
    public static Color DarkenColor(Color color)
    {
        Color bgColor = color / 3;
        bgColor.a = 1;
        return bgColor;
    }

    public static Color DarkenColor(Color color, int darkenAmount)
    {
        Color bgColor = color / darkenAmount;
        bgColor.a = 1;
        return bgColor;
    }
}
