using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    public static Music Instance;

    public List<AudioClip> tracks;

    public AudioSource source;

    public AudioLowPassFilter filter;
    public AudioHighPassFilter filter2;

    private float timer;

    private bool paused;

    const float FADE_TIME_SECONDS = 5;
    // Start is called before the first frame update\
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        source = GetComponent<AudioSource>();
        ChooseSong();
        Unmuffle();
    }

    void Update()
    {
        if (!source.isPlaying && !paused)
            timer += Time.deltaTime;
        if (timer > 90)
        {
            timer -= 90;
            ChooseSong();
        }
    }
    public void Muffle()
    {
        filter.enabled = true;
        filter2.enabled = true;
    }
    public void Unmuffle()
    {
        filter.enabled = false;
        filter2.enabled= false;
    }
    public void ChooseSong()
    {
        if (Prestige.Instance.inPrestigeAnim || Prestige.Instance.inPrestigeShop)
        {
            return;
        }
        int musicNum = LocationManager.Instance.activeLocation;
        source.clip = tracks[musicNum];
        source.Play();
        paused = false;
    }

    public void Pause()
    {
        source.Pause();
        paused = true;
    }
    public void Resume()
    {
        if (paused)
        {
            source.Play();
            paused = false;
        }
    }
    public void SkipSong()
    {
        source.Stop();
        ChooseSong();
    }
    public void StopSounds()
    {
        source.Stop();
    }
}
