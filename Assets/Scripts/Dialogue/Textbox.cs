using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Textbox : MonoBehaviour
{
    public static Textbox Instance;

    public GameObject box;
    public GameObject crosshair;
    public GameObject mainCam;
    public TMP_Text title;
    public TMP_Text description;
    public TMP_Text altDescription;
    public TMP_Text flavor;
    public TMP_Text cost;
    public bool shown = false;

    public bool shouldUpdate;
    private float updateCounter = -1;

    private void Awake()
    {
        if(Instance == null)
            { Instance = this; }
    }
    private void Update()
    {
        if (!IsActive())
            updateCounter = -1;
        if(updateCounter >= 0)
            updateCounter -= Time.deltaTime;
        else
            shouldUpdate = true;
    }

    public void SetUpdated()
    {
        shouldUpdate = false;
        updateCounter = 0.01f;
    }
    public void RequestUpdate()
    {
        shouldUpdate = true;
        updateCounter = -1;
    }
    public bool IsActive()
    {
        return box.activeSelf;
    }

    public void SetTitleText(string tText) 
    {
        title.text = tText;
    }

    public void SetTitleColor(Color tColor)
    {
        title.color = tColor;
    }

    public void SetDescriptionText(string dText)
    {
        description.text = dText;
    }

    public void SetFullDescriptionText(string aText)
    {
        description.text = string.Empty;
        flavor.text = string.Empty;
        altDescription.text = aText;
    }

    public void SetFullDescriptionColor(Color dColor)
    {
        altDescription.color = dColor;
    }

    public void SetFlavorText(string fText)
    {
        flavor.text = fText;
    }

    public void SetCostText(string cText)
    {
        cost.text = cText;
    }

    public void SetCostColor(Color cColor)
    {
        cost.color = cColor;
    }

    public void HideBox()
    {
        box.SetActive(false);
        title.color = Color.white;
        cost.color = Color.white;
        altDescription.text = string.Empty;
        shown = false;
    }

    public void ShowBox()
    {
        Vector3 offsetPos;
        float xOffset;
        float yOffset;
        if (crosshair.transform.position.y > mainCam.transform.position.y)
        {
            yOffset = -2f;
        }
        else 
        {
            yOffset = 2f;
        }

        if (crosshair.transform.position.x > mainCam.transform.position.x)
        {
            xOffset = -2f;
        }
        else 
        {
            xOffset = 2f;
        }
       

        offsetPos = new Vector3(crosshair.transform.position.x + xOffset, crosshair.transform.position.y + yOffset, 0);

        if (!shown)
        {
            box.transform.position = offsetPos;
        }
        box.SetActive(true);
        shown = true;
    }
    public void ShowBox(float xOffset, float yOffset)
    {
        Vector3 offsetPos;

        offsetPos = new Vector3(crosshair.transform.position.x + xOffset, crosshair.transform.position.y + yOffset, 0);

        if (!shown)
        {
            box.transform.position = offsetPos;
        }
        box.SetActive(true);
        shown = true;
    }
}
