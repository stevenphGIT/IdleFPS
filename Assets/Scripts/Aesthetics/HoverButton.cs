using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverButton : MonoBehaviour
{
    public SpriteRenderer insideImage;
    public SpriteRenderer buttonImage;

    private void Start()
    {
        Color locColor = LocationManager.Instance.locations[LocationManager.Instance.activeLocation].GetSecondColor();
        buttonImage.color = locColor *= 0.8f;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (Prestige.Instance.inPrestigeAnim) return;
        Color locColor = LocationManager.Instance.locations[LocationManager.Instance.activeLocation].GetSecondColor();
        buttonImage.color = locColor *= 1f;
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        Color locColor = LocationManager.Instance.locations[LocationManager.Instance.activeLocation].GetSecondColor();
        buttonImage.color = locColor *= 0.8f;
    }
}
