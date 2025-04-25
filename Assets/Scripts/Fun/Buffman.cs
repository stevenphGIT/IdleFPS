using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buffman : Boss
{
    public static Buffman Instance;

    public Sprite crossed;

    public GameObject deadman;

    public Sprite sad, hopeful, dead;

    public TextHolder speech;

    private float targetFreezeTimer;
    private float freezeFrequency = 50f;

    [SerializeField]
    private GameObject shield;
    public List<GameObject> activeShields;
    new void Update()
    {
        if (!attacking && BossHandler.Instance.fighting)
        {
            if (targetFreezeTimer > 0)
                targetFreezeTimer -= Time.deltaTime;
            else
            {
                targetFreezeTimer = freezeFrequency;
                FreezeTargets();
            }
        }
        base.Update();
    }
    private void FreezeTargets()
    {
        foreach (GameObject obj in activeShields)
        {
            Destroy(obj);
        }
        activeShields.Clear();
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Target"))
        {
            activeShields.Add(Instantiate(shield, obj.transform.position, Quaternion.identity, obj.transform));
        }
    }
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
        deadman.transform.position = new Vector3(boss.transform.position.x, boss.transform.position.y, 300);
        boss.SetActive(false);
        deadman.SetActive(true);
        yield return new WaitForSeconds(1f);
        BossHandler.Instance.Reset();
        CutsceneHandler.Instance.ShowShop();
        CutsceneHandler.Instance.inCutscene = false;
    }
}
