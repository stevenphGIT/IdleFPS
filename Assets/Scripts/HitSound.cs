using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSound : MonoBehaviour
{
    public static HitSound Instance;

    public AudioClip standardHit;
    public AudioClip silverHit;
    public AudioClip goldHit;
    public AudioClip platHit;
    public AudioClip omegaHit;
    public AudioClip upgradeBuy;
    public AudioClip closeMenu;
    public AudioClip openMenu;
    public AudioClip click;
    public AudioClip cantUse;
    public AudioClip sliderClick;
    public AudioClip slideOpen;
    public AudioClip slideClose;
    public AudioClip smallClick;
    public AudioClip snowCrunch1, snowCrunch2, snowCrunch3;
    public AudioClip snowmanTopple;

    public AudioClip deckOpen;
    public AudioClip deckClose;


    public AudioClip fwoom;
    public AudioClip comboEnd;

    public AudioClip defaultSpeak;

    public List<AudioClip> abilitySounds;
    public AudioSource source;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        source = GetComponent<AudioSource>();
    }
}
