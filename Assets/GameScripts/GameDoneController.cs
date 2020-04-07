using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDoneController : MonoBehaviour
{
    public GameObject[] CanvasContents;
    public void SetContent(bool good)
    {
        CanvasContents[good ? 0 : 1].SetActive(true);
    }
}
