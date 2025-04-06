using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneHandler : MonoBehaviour
{
    public static CutsceneHandler Instance;
    public bool inCutscene = false;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
}
