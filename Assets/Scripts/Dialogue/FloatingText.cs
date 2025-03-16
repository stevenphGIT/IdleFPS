using BreakInfinity;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class FloatingText : MonoBehaviour
{
    public static FloatingText Instance;

    public GameObject hitFloatText, canvas, frontCanvas;

    public GameObject textSpawn;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void PopText(BigDouble hits, Color32 textColor, float speed)
    {
        /*Vector3 mousePos = Crosshair.Instance.crosshairPos;
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);*/
        GameObject popText = Instantiate(hitFloatText, textSpawn.transform.position, Quaternion.identity, canvas.transform);
        popText.GetComponentInChildren<TMP_Text>().text = "+" + Vars.Instance.PopupAbbr(hits);
        popText.GetComponentInChildren<TMP_Text>().color = textColor;
        popText.GetComponentInChildren<Animator>().speed = speed;
    }
        
    public void PopText(string words, Color32 textColor, float offset)
    {
        Vector3 mousePos = Crosshair.Instance.crosshairPos;
        mousePos.z = Camera.main.nearClipPlane;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos += new Vector3(0, offset, 0);
        GameObject popText = Instantiate(hitFloatText, worldPos, Quaternion.identity, frontCanvas.transform);
        popText.GetComponentInChildren<TMP_Text>().text = words;
        popText.GetComponentInChildren<TMP_Text>().color = textColor;
    }

    public void PopText(string words, Color32 textColor, Vector3 position)
    {
        GameObject popText = Instantiate(hitFloatText, position, Quaternion.identity, frontCanvas.transform);
        popText.GetComponentInChildren<TMP_Text>().text = words;
        popText.GetComponentInChildren<TMP_Text>().color = textColor;
    }
}
