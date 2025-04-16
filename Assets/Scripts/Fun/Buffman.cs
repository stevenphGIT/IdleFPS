using System.Collections;
using UnityEngine;

public class Buffman : Boss
{
    public static Buffman Instance;

    public Sprite crossed;

    public GameObject deadman;

    public Sprite sad, hopeful, dead;

    public TextHolder speech;

    protected void Awake()
    {
        deadman.SetActive(false);
        if (Instance == null)
            Instance = this;
    }
    public void StartFight()
    {
        health = maxHealth;
        rend.sprite = crossed;
        animator.Play("SnowmanFightSlide");
        BossHandler.Instance.StartFight();
    }

    public override void KillBoss()
    {
        StartCoroutine(DeathCutscene());
        ShakeForSeconds(1f);
    }

    public IEnumerator DeathCutscene()
    {
        CutsceneHandler.Instance.inCutscene = true;
        BossHandler.Instance.fighting = false;
        animator.enabled = false;
        HitSound.Instance.source.PlayOneShot(HitSound.Instance.snowmanFurious);
        rend.sprite = sad;
        yield return new WaitForSeconds(3f);
        SpeechBox.Instance.Say(speech, new Color(0.9f, 0.9f, 1f));
        yield return new WaitUntil(() =>
            !SpeechBox.Instance.isSpeaking && !SpeechBox.Instance.boxShowing);
        yield return new WaitForSeconds(1f);
        rend.sprite = hopeful;
        HitSound.Instance.source.PlayOneShot(HitSound.Instance.snowCrunch1);
        yield return new WaitForSeconds(2f);
        ShakeForSeconds(2f);
        yield return new WaitForSeconds(1.9f);
        HitSound.Instance.source.PlayOneShot(HitSound.Instance.snowmanTopple);
        rend.sprite = dead;
        yield return new WaitForSeconds(1f);
        deadman.transform.position = boss.transform.position;
        boss.SetActive(false);
        deadman.SetActive(true);
        yield return new WaitForSeconds(1f);
        BossHandler.Instance.Reset();
        CutsceneHandler.Instance.ShowShop();
        CutsceneHandler.Instance.inCutscene = false;
    }
}
