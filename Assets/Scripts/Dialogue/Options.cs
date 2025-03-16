using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.PlayerLoop;

public class Options : MonoBehaviour, IDataPersistence
{
    public static Options Instance;

    public bool fs;
    public bool vs;
    public bool targetExplode;
    public Sprite okBox, nothingBox;
    public GameObject fsCheckbox, vsCheckbox, teCheckbox;
    public InputSlider master, music, effects;
    public AudioMixer mixer;
    public float masterV, musicV, effectsV;

    public InputSlider sens;
    public TMP_Text sensT;
    public float sensV;
    void Awake()
    {
        if(Instance == null)
            Instance = this;
    }
    void Start()
    {
        fs = Screen.fullScreen;

        if (QualitySettings.vSyncCount == 0)
        {
            vs = false;
        }
        else
        {
            vs = true;
        }

        UpdateValueOnChange();
        SetCheckboxSprites();
    }
    void Update()
    {
        if (sens.valUpdated)
        {
            UpdateValueOnChange();
        }
        if (master.valUpdated)
        {
            UpdateValueOnChange();
        }
        if (effects.valUpdated)
        {
            UpdateValueOnChange();
        }
        if (music.valUpdated)
        {
            UpdateValueOnChange();
        }
    }
    public void SetCheckboxSprites()
    {
        if (!fsCheckbox.activeSelf)
        {
            return;
        }
        if (fs)
        {
            fsCheckbox.GetComponent<SpriteRenderer>().sprite = okBox;
        }
        else
        {
            fsCheckbox.GetComponent<SpriteRenderer>().sprite = nothingBox;
        }
        if (vs)
        {
            vsCheckbox.GetComponent<SpriteRenderer>().sprite = okBox;
        }
        else
        {
            vsCheckbox.GetComponent<SpriteRenderer>().sprite = nothingBox;
        }
        if (targetExplode)
        {
            teCheckbox.GetComponent<SpriteRenderer>().sprite = okBox;
        }
        else
        {
            teCheckbox.GetComponent<SpriteRenderer>().sprite = nothingBox;
        }
    }
    public void ApplyGraphics()
    {
        Screen.fullScreen = fs;

        if (vs)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
    }

    

    public void UpdateValueOnChange()
    {
        sens.sliderValueText.text = sens.sliderValue.ToString("n2");

        master.sliderValueText.text = Mathf.Round(master.sliderValue * 100) + "%";
        music.sliderValueText.text = Mathf.Round(music.sliderValue * 100) + "%";
        effects.sliderValueText.text = Mathf.Round(effects.sliderValue * 100) + "%";

        if (mixer != null)
        {
            mixer.SetFloat("Master", Mathf.Log(master.sliderValue) * 20f);
            mixer.SetFloat("Music", Mathf.Log(music.sliderValue) * 20f);
            mixer.SetFloat("Effects", Mathf.Log(effects.sliderValue) * 20f);
        }
    }

    public void LoadData(GameData data)
    {
        this.fs = data.fs;
        this.vs = data.vs;
        this.targetExplode = data.targetExplode;
        master.sliderValue = data.masterV;
        music.sliderValue = data.musicV;
        effects.sliderValue = data.effectsV;

        sens.sliderValue = data.sensV;
    }

    public void SaveData(ref GameData data)
    {
        data.fs = this.fs;
        data.vs = this.vs;
        data.targetExplode = this.targetExplode;
        data.masterV = master.sliderValue;
        data.musicV = music.sliderValue;
        data.effectsV = effects.sliderValue;

        data.sensV = sens.sliderValue;
    }
}
