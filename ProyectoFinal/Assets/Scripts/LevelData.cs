using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelProgressData
{
    public List<bool> levelCompleted = new List<bool>();

    public LevelProgressData(int levelCount)
    {
        for (int i = 0; i < levelCount; i++)
        {
            levelCompleted.Add(false);
        }
    }
}