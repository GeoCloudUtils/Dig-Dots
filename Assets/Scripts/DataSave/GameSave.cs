using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameSave
{
    public static void SaveLevel(int levelIndex)
    {
        PlayerPrefs.SetString("Level" + levelIndex.ToString(), "Level" + levelIndex.ToString());
    }
}
