using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameDoneController : MonoBehaviour
{
    public GameObject[] CanvasContents;
    public Image screenShootImage;

    string path = "";
    private void Start()
    {
        Debug.Log(Application.persistentDataPath);
        path = Application.persistentDataPath + "/ScreenShot.png";
        Debug.Log(path);
    }
    public void SetContent(bool good)
    {
        CanvasContents[good ? 0 : 1].SetActive(true);
        if (good)
            StartCoroutine(WaitBeforeScreenShotUploaded());
    }
    private IEnumerator WaitBeforeScreenShotUploaded()
    {
        yield return new WaitForSeconds(1.5f);
#if UNITY_ANDROID
        path = "file:///" + Application.persistentDataPath;
        if (System.IO.File.Exists(path + "ScreenShot.png"))
            Debug.Log("Exist");
        else
            Debug.Log("Not Exist");
#else
        path = Application.persistentDataPath + "/ScreenShot.png";
        if (System.IO.File.Exists(path))
        {
            byte[] data = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(64, 64, TextureFormat.ARGB32, false);
            texture.LoadImage(data);
            texture.name = Path.GetFileNameWithoutExtension(path);
        }
        else
        {
            Debug.Log("Not exist");
        }

#endif
    }
}
