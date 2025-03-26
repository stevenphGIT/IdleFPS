using UnityEngine;

public class HeldCard : MonoBehaviour
{
    public static HeldCard Instance;

    public bool isHolding = false;
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
}
