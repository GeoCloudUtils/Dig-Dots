using ScriptUtils.GameUtils;
using ScriptUtils.Interface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogoManager : MonoBehaviour
{
    public GameObject loadingScreen;
    public float delay;
    public void LoadGameLogo()
    {
        StartCoroutine(LoadLogo());
    }
    private IEnumerator LoadLogo()
    {
        yield return new WaitForSeconds(delay);
        Navigator.getInstance().setLoadingScreenPrefab<LoadingScreen>(loadingScreen);
        Navigator.getInstance().LoadLevel("GameLogo");
    }
}
