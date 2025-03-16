using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraViewHandler : MonoBehaviour
{
    public static CameraViewHandler Instance;

    public GameObject homeObj;
    public GameObject shopObj;
    public GameObject prestigeObj;
    public GameObject cardObj;
    public GameObject alleyObj;

    private Vector3 homePos;
    private Vector3 shopPos;
    private Vector3 prestigePos;
    private Vector3 cardPos;
    private Vector3 alleyPos;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    void Start()
    {
        homePos = homeObj.transform.position;
        shopPos = shopObj.transform.position;
        prestigePos = prestigeObj.transform.position;
        cardPos = cardObj.transform.position;
        alleyPos = alleyObj.transform.position;
    }

    public void MoveCamToPos(string name) 
    {
        switch (name)
        {
            case "home":
                this.transform.position = homePos;
                break;
            case "shop":
                this.transform.position = shopPos;
                break;
            case "prestige":
                this.transform.position = prestigePos;
                break;
            case "card":
                this.transform.position = cardPos;
                break;
            case "alley":
                this.transform.position = alleyPos;
                break;
            default:
                Debug.LogError("Invalid camera location called!");
                break;
        }
    }
}
