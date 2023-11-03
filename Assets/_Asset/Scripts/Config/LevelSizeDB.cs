using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class LevelSizeDB : ScriptableObject
{
    public LevelSize[] levelSizes;

    public int levelSizesCount
    {
        get
        {
            return levelSizes.Length;
        }
    }

    public LevelSize GetLevelSize(int index)
    {
        return levelSizes[index];
    }
}
