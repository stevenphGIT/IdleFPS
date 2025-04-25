using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NoticeBox : MonoBehaviour
{
    public static NoticeBox Instance;

    public Camera main;
    public GameObject box;
    public GameObject dismissBox;
    public GameObject yesBox;
    public GameObject noBox;
    public TMP_Text title;
    public TMP_Text description;
    public Animator animator;
    public Animator vignetteAnim;
    public int idNum;
    public bool activeBox = false;

    private void Awake()
    {
        if (Instance == null) { Instance = this; }
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


    public void SetDescriptionColor(Color dColor)
    {
        description.color = dColor;
    }

    public void SetChoosable(bool x)
    {
        if (x)
        {
            dismissBox.SetActive(false);
            yesBox.SetActive(true);
            noBox.SetActive(true);
        }
        else 
        {
            dismissBox.SetActive(true);
            yesBox.SetActive(false);
            noBox.SetActive(false);
        }
    }

    public void HideBox()
    {
        vignetteAnim.Play("VignetteFadeOut");
        Music.Instance.Unmuffle();
        animator.SetBool("Show", false);
        activeBox = false;
    }

    public void ShowBox()
    {
        box.transform.position = new Vector3(main.transform.position.x, box.transform.position.y, box.transform.position.z);
        activeBox = true;
        animator.SetBool("Show", true);
        vignetteAnim.Play("VignetteFadeIn");
        Music.Instance.Muffle();
    }
}
