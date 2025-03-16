using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//[CreateAssetMenu]
[CreateAssetMenu]
public class AbilityDatabase : ScriptableObject
{
    [SerializeField] private List<Ability> adb;

    public int Size()
    {
        return adb.Count;
    }

    public void Buy(int i)
    {
        adb.RemoveAt(i);
    }
}
