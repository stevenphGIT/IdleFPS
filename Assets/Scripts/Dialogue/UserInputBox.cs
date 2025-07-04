using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInputBox : MonoBehaviour
{
    public static UserInputBox Instance;
    public GameObject[] textHolders;
    public Animator boxAnim;
    public Animator vignetteAnim;
    public HitSound hitSound;
    private NoticeBox notice;

    //0 = Settings
    //1 = Stats
    //2 = Crosshair
    //3 = Duel
    //4 = World Map
    private void Start()
    {
        notice = GameObject.Find("NoticeHandler").GetComponent<NoticeBox>();
        if (Instance == null) { Instance = this; }
        
    }
    private void ShowText(int index)
    {
        for (int i = 0; i < textHolders.Length; i++)
        {
            textHolders[i].gameObject.SetActive(false);
        }
        textHolders[index].gameObject.SetActive(true);
    }
    public void HideBox()
    {
        hitSound.source.PlayOneShot(hitSound.closeMenu);
        boxAnim.SetBool("active", false);
        vignetteAnim.Play("VignetteFadeOut");
        Music.Instance.Unmuffle();
    }
    public void ShowBox(int index)
    {
        if (Active())
        {
            boxAnim.SetBool("active", false);
            hitSound.source.PlayOneShot(hitSound.closeMenu);
            vignetteAnim.Play("VignetteFadeOut");
            Music.Instance.Unmuffle();
        }
        else
        {
            hitSound.source.PlayOneShot(hitSound.openMenu);
            ShowText(index);
            boxAnim.SetBool("active", true);
            vignetteAnim.Play("VignetteFadeIn");
            Music.Instance.Muffle();
        }

    }

    public void CrosshairSettings()
    {
        ShowText(2);
        hitSound.source.PlayOneShot(hitSound.openMenu);
    }
    public bool Active()
    {
        return boxAnim.GetBool("active");
    }
}
