using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GameData : MonoBehaviour
{
    public int levelIndex;
    public string levelName;

    public GameData(int levelInt, string nameStr)
    {
        levelIndex = levelInt;
        levelName = nameStr;
    }
}
