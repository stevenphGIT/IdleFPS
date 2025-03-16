using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public List<TextHolder> textHolderQueue;
    public TextHolder baseObject;
    private TextHolder activeText = null;
    protected Color npcColor = Color.red;
    public GameObject notificationObj;

    public void Click()
    {
        if(textHolderQueue.Count > 0)
            activeText = textHolderQueue[0];
        else
            activeText = baseObject;
        SpeechBox.Instance.Say(activeText, npcColor);
        textHolderQueue.Remove(activeText);
        if (textHolderQueue.Count == 0)
        {
            if (notificationObj != null)
                notificationObj.SetActive(false);
        }
    }
    protected void AddObjectToQueue(TextHolder obj)
    {
        int objID = obj.identifier;
        bool flag = false;
        foreach (int id in Vars.Instance.addedDialogueIDs)
        {
            if (id == objID)
            {
                flag = true; 
                break;
            }
        }
        foreach (TextHolder t in textHolderQueue)
        {
            if (t.identifier == objID)
            {
                flag = true;
                break;
            }
        }
        if (!flag)
        {
            textHolderQueue.Add(obj);
        }
    }
}
