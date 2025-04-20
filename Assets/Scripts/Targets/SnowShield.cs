using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowShield : MonoBehaviour
{
    private int stage = 4;

    [SerializeField]
    private Sprite[] stages;
    [SerializeField]
    private ParticleSystem effect;

    private float shakeTimer = 0f;
    private float startX, startY;
    public void Click()
    {
        if (stage > 0)
        {
            stage--;
            effect.Play();
            shakeTimer = 0.1f;
            UpdateSprite();
        }
        else
            Destroy(this.gameObject);
    }
    private void UpdateSprite()
    {
        this.gameObject.GetComponent<SpriteRenderer>().sprite = stages[stage];
    }
    private void Awake()
    {
        SetOrigin();
    }
    private void Update() {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            float shakeX = startX + (Random.value - 0.5f) * 0.1f;
            float shakeY = startY + (Random.value - 0.5f) * 0.1f;
            this.gameObject.transform.position = new Vector3(shakeX, shakeY, 0);
        }
        else if (shakeTimer != -50)
        {
            this.gameObject.transform.position = new Vector3(startX, startY, -5);
            shakeTimer = -50f;
            SetOrigin();
        }
    }
    public void SetOrigin()
    {
        startX = this.gameObject.transform.position.x;
        startY = this.gameObject.transform.position.y;
    }
}
