using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameDoneController : MonoBehaviour
{
    public GameObject[] CanvasContents;
    public Image screenShootImage;
    public Button nextLevelButton;
    public event UnityAction NextLevelEvent;
    public TextMeshProUGUI textLevel;
    string path = "";
    private void Start()
    {
        path = Application.persistentDataPath + "/ScreenShot.png";
        nextLevelButton.onClick.AddListener(LoadNextLevel);
    }
    private void LoadNextLevel()
    {
        nextLevelButton.interactable = false;
        if (NextLevelEvent != null)
            NextLevelEvent.Invoke();
    }

    public void SetContent(bool good)
    {
        CanvasContents[good ? 0 : 1].SetActive(true);
        int levelIndex = PlayerPrefs.GetInt("levelIndex");
        textLevel.text = "Level" + " " + (levelIndex + 1) + "\n" + "COMPLETE";
        if (good)
            StartCoroutine(WaitBeforeScreenShotUploaded());
    }
    private IEnumerator WaitBeforeScreenShotUploaded()
    {
        yield return new WaitForSeconds(1.5f);
        //#if UNITY_ANDROID
        //        path = "file:///" + Application.persistentDataPath;
        //        if (System.IO.File.Exists(path + "ScreenShot.png"))
        //            Debug.Log("Exist");
        //        else
        //            Debug.Log("Not Exist");
        //#else
        //        path = Application.persistentDataPath + "/ScreenShot.png";
        //        if (System.IO.File.Exists(path))
        //        {
        //            byte[] data = File.ReadAllBytes(path);
        //            Texture2D texture = new Texture2D(64, 64, TextureFormat.ARGB32, false);
        //            texture.LoadImage(data);
        //            texture.name = Path.GetFileNameWithoutExtension(path);
        //        }
        //        else
        //        {
        //            Debug.Log("Not exist");
        //        }

        //#endif
    }
}
