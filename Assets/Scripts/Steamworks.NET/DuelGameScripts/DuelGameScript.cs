using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuelGameScript : MonoBehaviour
{
    public void StartGame()
    {
        DataPersistenceManager.instance.SaveGame();
    }
}
