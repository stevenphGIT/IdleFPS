using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowman : MonoBehaviour
{
    public static Snowman Instance;

    public GameObject snowman, snowmanTransition, buffman;

    public ParticleSystem snowPuff;

    bool knocked = false;
    int clickCount = 10;

    float shakeTimer = 0f;
    float shakeIntensity = 0.2f;

    float respawnTimer = -50f;

    float startX;
    float startY;

    [SerializeField]
    Sprite knockedSprite, normalSprite, angrySprite;
    [SerializeField]
    TextHolder openingSpeech;
    private void Awake()
    {
        if(Instance == null)
            Instance = this;
        startX = transform.position.x;
        startY = transform.position.y;
        snowman.SetActive(true);
        snowmanTransition.SetActive(false);
        buffman.SetActive(false);
    }
    public void Click()
    {
        if (!knocked)
        {
            if (clickCount > 0)
            {
                shakeTimer = 0.1f;
                float random = Random.value;
                if (random > 0.66f)
                {
                    HitSound.Instance.source.PlayOneShot(HitSound.Instance.snowCrunch1, 2f);
                }
                else if (random > 0.33f)
                {
                    HitSound.Instance.source.PlayOneShot(HitSound.Instance.snowCrunch2, 2f);
                }
                else
                {
                    HitSound.Instance.source.PlayOneShot(HitSound.Instance.snowCrunch3, 2f);
                }
                clickCount--;
            }
            else
            {
                if (Vars.Instance.snowmenSlain >= 2)
                {
                    HitSound.Instance.source.PlayOneShot(HitSound.Instance.snowCrunch1);
                    Music.Instance.StopSounds();
                    Music.Instance.Pause();
                    knocked = true;
                    snowman.GetComponent<SpriteRenderer>().sprite = angrySprite;
                    HitSound.Instance.source.PlayOneShot(HitSound.Instance.snowmanFurious);
                    StartCoroutine(AngerCutscene());
                }
                else
                {
                    HitSound.Instance.source.PlayOneShot(HitSound.Instance.snowmanTopple);
                    knocked = true;
                    snowman.GetComponent<SpriteRenderer>().sprite = knockedSprite;
                    Vars.Instance.snowmenSlain++;
                    respawnTimer = 10;
                }
            }
        }
    }
    private void Update()
    {
        if (respawnTimer > 0)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer < 0.5f)
            {
                shakeTimer = 0.1f;
            }
        }
        else if (respawnTimer != -50)
        {
            knocked = false;
            clickCount = 10;
            snowman.GetComponent<SpriteRenderer>().sprite = normalSprite;
            if (snowman.activeSelf)
            {
                HitSound.Instance.source.PlayOneShot(HitSound.Instance.snowCrunch3);
            }
            respawnTimer = -50;
        }
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            float shakeX = startX + (Random.value - 0.5f) * shakeIntensity;
            float shakeY = startY + (Random.value - 0.5f) * shakeIntensity;
            this.transform.position = new Vector3(shakeX, shakeY, 0);
        }
        else if (shakeTimer != -50)
        {
            this.transform.position = new Vector3(startX, startY, 0);
            shakeTimer = -50;
        }
    }

    private IEnumerator AngerCutscene()
    {
        CutsceneHandler.Instance.inCutscene = true;
        yield return new WaitForSeconds(2f);
        shakeTimer = 1.2f;
        yield return new WaitForSeconds(1f);
        SpeechBox.Instance.Say(openingSpeech, new Color(0.9f, 0.9f, 1f));
        yield return new WaitUntil(() =>
            !SpeechBox.Instance.isSpeaking && !SpeechBox.Instance.boxShowing);
        CutsceneHandler.Instance.HideShop();
        yield return new WaitForSeconds(0.6f);
        shakeIntensity = 0.1f;
        shakeTimer = 2.1f;
        yield return new WaitForSeconds(2f);
        snowman.SetActive(false);
        buffman.SetActive(false);
        snowmanTransition.SetActive(true);
        snowmanTransition.GetComponent<Animator>().Play("SnowmanEvolve");
        yield return new WaitForSeconds(2.2f);
        shakeTimer = 1f;
        yield return new WaitForSeconds(1.5f);
        shakeTimer = 0.5f;
        yield return new WaitForSeconds(1.7f);
        shakeTimer = 1f;
        yield return new WaitForSeconds(2f);
        snowman.SetActive(false);
        snowmanTransition.SetActive(false);
        buffman.SetActive(true);
        buffman.GetComponent<Animator>().Play("SnowmanFightStart");
        //TextQueue.Instance.AddNotificationToList(new TextQueue.Notification("Boss Battle!", "The Snowman", Color.white, new Color(0.78f, 0.96f, 1f)));
        yield return new WaitForSeconds(2f);
        CutsceneHandler.Instance.inCutscene = false;
        Buffman.Instance.StartFight();
    }
}
