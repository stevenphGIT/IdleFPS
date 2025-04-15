using UnityEngine;

public class Buffman : Boss
{
    public static Buffman Instance;

    public Sprite crossed;

    protected void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    public void StartFight()
    {
        rend.sprite = crossed;
        animator.Play("SnowmanFightSlide");
        BossHandler.Instance.StartFight();
    }
}
