using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    public float destroyTime;
    void Awake()
    {
        Destroy(gameObject, destroyTime);
    }
}
