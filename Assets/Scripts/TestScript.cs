using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestScript : MonoBehaviour
{
    public int countU, countP, countG, countS, countR = 0;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 100000; i++)
        {
            float randomFloat = (float)Math.Round(UnityEngine.Random.Range(0f, 1000f));
            if (randomFloat < 1)
            {
                countU++;
            }
            else if (randomFloat < 6)
            {
                countP++;
            }
            else if (randomFloat < 16)
            {
                countG++;
            }
            else if (randomFloat < 66)
            {
                countS++;
            }
            else
            {
                countR++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
