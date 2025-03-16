using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.UI;

public class SpeechBox : MonoBehaviour
{
    public static SpeechBox Instance; 

    public TMP_Text nameText, speechText, answer1text, answer2text, narrateText;

    public GameObject npcTalk, narratorTalk;

    public GameObject box,answerParent;
    public SpriteRenderer iconRenderer;
    public Sprite system;

    public bool isSpeaking = false;

    public bool boxShowing = false;

    public bool showIcon = false;

    private bool skipped = false;

    public Animator vignetteAnim;

    private string textToSay;
    private string option1text = "",option2text = "";
    private string speaker;
    private Color speakerColor;

    private bool unskippable;
    private bool silent;

    public float textDelay;

    private int lineCount = 0;
    private TextHolder activeText = null;
    private TextHolder branch1 = null,branch2 = null;
    private AudioClip voice = null;
    private Sprite icon = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    public void Say(TextHolder obj, Color sColor)
    {
        if (isSpeaking || activeText != null)
            return;

        activeText = obj;
        if (!activeText.standard)
            Vars.Instance.addedDialogueIDs.Add(activeText.identifier);
        DialogueObject d = null;
        if (activeText.standard)
        {
            int randIndex = Random.Range(0, activeText.lines.Length);
            d = obj.lines[randIndex];
        }
        else
        {
            d = obj.lines[0];
        }
        textToSay = d.speech;
        float sp = d.textSpeed;
        if (sp != 0)
            textDelay = sp;
        else
            textDelay = 0.03f;
        this.unskippable = d.unskippable;
        this.silent = d.stopSound;
        showIcon = !(d.icon == null);
        if (showIcon)
        {
            speaker = obj.character;
            speakerColor = sColor;
            icon = d.icon;
            voice = d.voice;
        }
        else
        {
            voice = HitSound.Instance.defaultSpeak;
            speaker = "";
            icon = null;
        }
        
        StartSpeaking();
    }
    public void Say(string name, string singleLine)
    {
        if (isSpeaking)
            return;
        
        voice = HitSound.Instance.defaultSpeak;
        speaker = name;
        speakerColor = Color.white;
        textToSay = singleLine;
        textDelay = 0.03f;
        this.unskippable = false;
        this.silent = false;

        showIcon = false;

        if (textToSay.Length > 0)
            StartSpeaking();
    }
    public void NextLine()
    {
        if (activeText == null)
        {
            if (!isSpeaking)
                StopSpeaking();
            else
                Skip();
            return;
        }
        else
        {
            if (isSpeaking)
            {
                Skip();
            }
            else
            {
                if (lineCount > activeText.lines.Length - 1)
                {
                    return;
                }
                else if (lineCount == activeText.lines.Length - 1)
                {
                    if (activeText.branch.question.Length == 0)
                        StopSpeaking();
                    else
                    {
                        textToSay = activeText.branch.question;
                        textDelay = 0.03f;
                        icon = activeText.branch.icon;
                        branch1 = activeText.branch.choices[0].ensuingDialogue;
                        branch2 = activeText.branch.choices[1].ensuingDialogue;
                        option1text = activeText.branch.choices[0].answer;
                        option2text = activeText.branch.choices[1].answer;
                        StartSpeaking();
                        lineCount++;
                    }
                }
                else if (!activeText.standard)
                {

                    lineCount++;
                    DialogueObject d = activeText.lines[lineCount];
                    textToSay = d.speech;
                    float sp = d.textSpeed;
                    if (sp != 0)
                        textDelay = sp;
                    else
                        textDelay = 0.03f;
                    this.unskippable = d.unskippable;
                    this.silent = d.stopSound;
                    showIcon = !(d.icon == null);

                    if (showIcon)
                    {
                        speaker = activeText.character;
                        icon = d.icon;
                    }
                    else
                    {
                        speaker = "";
                        icon = null;
                    }
                    StartSpeaking();
                }
                else
                {
                    StopSpeaking();
                }
            }
        }
    }
    public void ChooseBranch(int i)
    {
        if (isSpeaking)
            return;

        if (i == 0) 
            activeText = branch1;
        else 
            activeText = branch2;
        if(!activeText.standard)
            Vars.Instance.addedDialogueIDs.Add(activeText.identifier);
        lineCount = -1;
        answerParent.SetActive(false);
        branch1 = null;
        branch2 = null;
        NextLine();
    }
    public void Skip()
    {
        if (this.unskippable)
            return;
        skipped = true;
    }
    public void StartSpeaking()
    {
        if (isSpeaking)
            return;

        vignetteAnim.Play("VignetteFadeIn");
        boxShowing = true;
        nameText.text = "";
        speechText.text = "";
        answer1text.text = "";
        answer2text.text = "";
        narrateText.text = "";
        box.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - 2.5f, box.transform.position.z);
        box.SetActive(true);
        isSpeaking = true;
        StartCoroutine("DisplayText");
    }

    public void StopSpeaking()
    {
        vignetteAnim.Play("VignetteFadeOut");
        box.SetActive(false);
        textToSay = null;
        speaker = null;
        isSpeaking=false;
        boxShowing = false;
        lineCount = 0;
        activeText = null;
    }

    private IEnumerator DisplayText()
    {
        narratorTalk.SetActive(!showIcon);
        npcTalk.SetActive(showIcon);
        TMP_Text mainText;
        AudioClip speakSound = voice;
        if (showIcon)
            mainText = speechText;
        else
        {
            mainText = narrateText;
            speakSound = HitSound.Instance.defaultSpeak;
        }
        nameText.text = speaker;
        nameText.color = speakerColor;
        iconRenderer.sprite = icon;
        yield return new WaitForSeconds(0.2f);
        while (textToSay.Length > 0)
        {
            while (textToSay[0] == '<')
            {
                int endIndex = textToSay.IndexOf('>');
                string extractedText = textToSay[..(endIndex + 1)];
                mainText.text += extractedText;
                textToSay = textToSay.Remove(0, endIndex + 1);
            }
            mainText.text += textToSay[0];
            if (!skipped)
            {
                if (!this.silent && textToSay[0] != ' ')
                    HitSound.Instance.source.PlayOneShot(voice);
                if(textToSay[0] == '.')
                    yield return new WaitForSeconds(textDelay * 2f);
                else
                    yield return new WaitForSeconds(textDelay);
            }
            textToSay = textToSay[1..];
        }
        if (option1text.Length > 0)
        {
            if (!skipped)
                yield return new WaitForSeconds(1f);
            answerParent.SetActive(true);
            while (option1text.Length > 0)
            {
                while (option1text[0] == '<')
                {
                    int endIndex = option1text.IndexOf('>');
                    string extractedText = option1text[..(endIndex + 1)];
                    answer1text.text += extractedText;
                    option1text = option1text.Remove(0, endIndex + 1);
                }
                answer1text.text += option1text[0];
                if (!skipped)
                {
                    if (!this.silent && option1text[0] != ' ')
                        HitSound.Instance.source.PlayOneShot(HitSound.Instance.click);
                    yield return new WaitForSeconds(textDelay);
                }
                option1text = option1text[1..];
            }
            if(!skipped)
                yield return new WaitForSeconds(0.5f);
            while (option2text.Length > 0)
            {
                while (option2text[0] == '<')
                {
                    int endIndex = option2text.IndexOf('>');
                    string extractedText = option2text[..(endIndex + 1)];
                    answer2text.text += extractedText;
                    option2text = option2text.Remove(0, endIndex + 1);
                }
                answer2text.text += option2text[0];
                if (!skipped)
                {
                    if (!this.silent && option2text[0] != ' ')
                        HitSound.Instance.source.PlayOneShot(HitSound.Instance.click);
                    yield return new WaitForSeconds(textDelay);
                }
                option2text = option2text[1..];
            }
        }
        
        isSpeaking = false;
        skipped = false;
    }
    public string ProcessDialogue(string dialogue)
    {
        Dictionary<string, string> variables = new Dictionary<string, string>
        {
            { "rads", Vars.Instance.TotalAbbr(Vars.Instance.rads) }
        };

        foreach (var variable in variables)
        {
            dialogue = dialogue.Replace("{" + variable.Key + "}", variable.Value);
        }

        return dialogue;
    }
}
