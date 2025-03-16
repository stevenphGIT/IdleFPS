using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    SpriteRenderer rend;
    float time = 0.15f;
    private void Awake()
    {
        rend = this.GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, (time / 0.15f));
        time -= Time.deltaTime;
    }
}
