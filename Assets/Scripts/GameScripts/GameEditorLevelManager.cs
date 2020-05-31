using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class GameEditorLevelManager : MonoBehaviour
{
    public LevelController[] AllLevels;
    public bool doRename = false;
    private void Start()
    {

    }
    void Update()
    {
        if (doRename)
        {
            doRename = false;
            Rename();
        }
    }
    private void Rename()
    {
        AllLevels = GetComponentsInChildren<LevelController>(true);
        for (int i = 0; i < AllLevels.Length; i++)
            AllLevels[i].name = "Level" + (i + 1);
    }
}
