using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class DifficultyDB : ScriptableObject
{
    public Difficulty[] difficultyTypes;

    public int difficultyTypesCount
    {
        get
        {
            return difficultyTypes.Length;
        }
    }

    public Difficulty GetDifficulty(int index)
    {
        return difficultyTypes[index];
    }
}
