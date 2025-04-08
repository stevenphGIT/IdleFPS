using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffman : Boss
{
    public static Buffman Instance;

    public Sprite crossed;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    public void StartFight()
    {
        rend.sprite = crossed;
        animator.Play("SnowmanFightSlide");
    }
}
