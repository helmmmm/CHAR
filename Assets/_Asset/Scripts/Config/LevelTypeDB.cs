using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelTypeDB : ScriptableObject
{
    public LevelType[] levelTypes;

    public int levelTypeCount
    {
        get
        {
            return levelTypes.Length;
        }
    }

    public LevelType GetLevelType(int index)
    {
        return levelTypes[index];
    }
}
