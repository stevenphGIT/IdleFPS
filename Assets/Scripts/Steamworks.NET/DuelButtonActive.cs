using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuelButtonActive : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!SteamManager.Initialized || !SteamUser.BLoggedOn())
        {
            this.gameObject.SetActive(false);
        }
    }
}
