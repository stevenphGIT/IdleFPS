using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneHandler : MonoBehaviour
{
    public static CutsceneHandler Instance;
    public bool inCutscene = false;

    public Animator shop;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void HideShop() {
        shop.Play("ShopHide");
    }

    public void ShowShop()
    {
        shop.Play("ShopShow");
    }
}
