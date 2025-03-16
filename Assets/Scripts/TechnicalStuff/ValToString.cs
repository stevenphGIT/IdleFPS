using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValToString : MonoBehaviour
{
    public static ValToString Instance;
    void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    public string ShortenTime(float t)
    {
        int days;
        int hours;
        int minutes;

        string output = "";

        if (t > 86400)
        {
            days = Mathf.RoundToInt(t /= 86400);
            output += days + " day";
            if (days > 1)
                output += "s";
        }
        else if (t > 3600)
        {
            hours = Mathf.RoundToInt(t /= 3600);
            output += hours + " hour";
            if (hours > 1)
                output += "s";
        }
        else if (t > 60)
        {
            minutes = Mathf.RoundToInt(t /= 60);
            output += minutes + " minute";
            if (minutes > 1)
                output += "s";
        }
        else
        {
            output += Mathf.RoundToInt(t) + " second";
            if (Mathf.RoundToInt(t) > 1)
                output += "s";
        }

        return output;
    }
}
