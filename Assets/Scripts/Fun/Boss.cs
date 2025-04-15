using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer rend;
    public GameObject boss, shakeObj;
    public string bossName;
    public Color bossColor;

    protected float shakeTimer = -50, shakeIntensity = 0.2f;
    protected float startX;
    protected float startY;

    protected bool posSet = false;

    private void Update()
    {
        DamageTick();
    }
    protected void DamageTick()
    {
        if (shakeTimer > 0)
        {
            if (posSet)
            {
                shakeTimer -= Time.deltaTime;
                float shakeX = startX + (Random.value - 0.5f) * shakeIntensity;
                float shakeY = startY + (Random.value - 0.5f) * shakeIntensity;
                shakeObj.transform.position = new Vector3(shakeX, shakeY, 0);
            }
        }
        else if (shakeTimer != -50)
        {
            shakeObj.transform.position = new Vector3(startX, startY, 0);
            boss.GetComponent<SpriteRenderer>().color = Color.white;
            shakeTimer = -50;
        }
    }
    public void SetOrigin()
    {
        startX = shakeObj.transform.position.x;
        startY = shakeObj.transform.position.y;
        posSet = true;
    }
    public void Hurt(double damage, string source)
    {
        if (shakeTimer == -50)
        {
            SetOrigin();
        }
        shakeTimer = 0.6f;
        boss.GetComponent<SpriteRenderer>().color = new Color(1, 0.4f, 0.4f);
        FloatingText.Instance.PopText("-" + damage + "\n" + source, Color.red, boss.transform.position);
    }
}
