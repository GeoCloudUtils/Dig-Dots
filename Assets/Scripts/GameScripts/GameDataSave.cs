using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataSave : MonoBehaviour
{
    public static GameDataSave instance = null;
    public int reloadIndex = 0;
    public int levelIndex = 0;
    void Awake()
    {
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log(scene.name);
        if (scene.name == "Menu")
        {
            Destroy(this.gameObject);
            return;
        }
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}
