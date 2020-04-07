using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotHandler : MonoBehaviour
{
    public static ScreenshotHandler instance;
    private Camera myCamera;
    private bool takeScreenShotOnNextFrame;

    private void Awake()
    {
        instance = this;
        myCamera = gameObject.GetComponent<Camera>();
    }

    private void OnPostRender()
    {
        if (takeScreenShotOnNextFrame)
        {
            takeScreenShotOnNextFrame = false;
            RenderTexture renderTexture = myCamera.targetTexture;
            Texture2D myCamRenderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
            myCamRenderResult.ReadPixels(rect, 0, 0);

            byte[] byteArray = myCamRenderResult.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.persistentDataPath + "/ScreenShot.png", byteArray);
            Debug.Log("Saved screenshot at " + Application.persistentDataPath);

            RenderTexture.ReleaseTemporary(renderTexture);
            myCamera.targetTexture = null;
        }
    }

    public void TakeScreenShot(int width, int height)
    {
        myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        takeScreenShotOnNextFrame = true;
    }
}
