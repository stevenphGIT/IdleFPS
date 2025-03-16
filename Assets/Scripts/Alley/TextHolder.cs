using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BreakInfinity;

[CreateAssetMenu]
public class TextHolder : ScriptableObject
{
    public string character = "";
    public bool standard;
    public int identifier;
    public DialogueObject[] lines;
    public DialogueBranchObject branch;
}

[System.Serializable]
public class DialogueObject
{
    [TextArea(2, 8)] public string speech;
    public Sprite icon;
    public float textSpeed;
    public bool stopMusic;
    public bool stopSound;
    public bool unskippable;
    public AudioClip voice;
}

[System.Serializable]
public class DialogueBranchObject
{
    [TextArea(2, 8)] public string question;
    public Sprite icon;
    public DialogueChoiceObject[] choices;
}

[System.Serializable]
public class DialogueChoiceObject
{
    [TextArea(2, 8)] public string answer;
    public TextHolder ensuingDialogue;
}