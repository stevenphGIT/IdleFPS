using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenDealer : NPC
{
    //Custom Texts
    public TextHolder snowmanKilledText1, snowmanKilledText2, introText;

    
    private readonly float timerMax = 5f;
    private float timer = 5f;
    private void Start()
    {
        AddObjectToQueue(introText);
        npcColor = new Color(0.5f, 0.063f, 0.063f);
        CheckForNewDialogue();
    }
    public new void Click()
    {
        CheckForNewDialogue();
        base.Click();
    }
    private void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            CheckForNewDialogue();
            timer = timerMax;
        }
    }
    private void CheckForNewDialogue()
    {
        if (Vars.Instance.snowmenSlain > 2)
        {
            AddObjectToQueue(snowmanKilledText2);
        }
        else if (Vars.Instance.snowmenSlain > 0)
        {
            AddObjectToQueue(snowmanKilledText1);
        }
        if (textHolderQueue.Count != 0)
        {
            notificationObj.SetActive(true);
        }
    }
}
