using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowman : MonoBehaviour
{
    public static Snowman Instance;

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

        normalSprite = this.gameObject.GetComponent<SpriteRenderer>().sprite;
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
                if (Vars.Instance.snowmenSlain > 2)
                {
                    HitSound.Instance.source.PlayOneShot(HitSound.Instance.snowCrunch1);
                    Music.Instance.StopSounds();
                    knocked = true;
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = angrySprite;
                    StartCoroutine(AngerCutscene());
                }
                else
                {
                    HitSound.Instance.source.PlayOneShot(HitSound.Instance.snowmanTopple);
                    knocked = true;
                    this.gameObject.GetComponent<SpriteRenderer>().sprite = knockedSprite;
                    Vars.Instance.snowmenSlain++;
                }
            }
        }
    }
    private void Update()
    {
        if (respawnTimer > 0)
        {
            respawnTimer -= Time.deltaTime;
        }
        else if (respawnTimer != -50)
        {
            knocked = false;
            clickCount = 10;
            this.gameObject.GetComponent<SpriteRenderer>().sprite = normalSprite;
            if (this.gameObject.activeSelf)
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
        yield return new WaitForSeconds(1f);
        shakeTimer = 1.2f;
        yield return new WaitForSeconds(1f);
        SpeechBox.Instance.Say(openingSpeech, new Color(0.9f, 0.9f, 1f));
    }
}
