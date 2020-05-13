using ScriptUtils.GameUtils;
using ScriptUtils.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SountInit : MonoBehaviour
{
    public SoundGameManager sManagerPrefab;
    public AudioClip clickFX;
    public GameObject PressToPlayText;
    public loadingbar lBar;
    public GameObject loadingScreen;
    public AudioClip bgMusic;
    public bool started = false;
    private SoundGameManager sManager;
    void Start()
    {
        if (FindObjectOfType<SoundGameManager>() != null)
        {
            GameObject target = FindObjectOfType<SoundGameManager>().gameObject;
            Destroy(target);
        }
        sManager = Instantiate(sManagerPrefab, Vector3.zero, transform.rotation) as SoundGameManager;
        lBar.fillDone += Init;
    }

    public void Init()
    {
        lBar.transform.parent.gameObject.SetActive(false);
        sManager.SetMusic(bgMusic);
        float musicVolume = PlayerPrefs.HasKey("Volume") ? PlayerPrefs.GetFloat("Volume") : 0.5f;
        sManager.SetMusicVolume(musicVolume);
        sManager.PlayMusic(bgMusic);
        PressToPlayText.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (PressToPlayText.gameObject.activeSelf)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (!started)
                    LoadInterface();
            }
        }
    }

    public void LoadInterface()
    {
        sManager.PlaySound(clickFX);
        started = true;
        Navigator.getInstance().setLoadingScreenPrefab<LoadingScreen>(loadingScreen);
        Navigator.getInstance().LoadLevel("Menu");
    }
}
