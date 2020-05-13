using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameAdCounter : MonoBehaviour
{
    public static GameAdCounter instance = null;
    public int counter = 0;
    public int nextStetp;
    public int step = 3;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        nextStetp += step;
    }
}
