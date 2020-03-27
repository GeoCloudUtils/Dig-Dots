using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsTriggerManager : MonoBehaviour
{
    public BallController redBall;
    public BallController blueBall;

    public InterfaceManager iManager;
    void Start()
    {
        iManager = FindObjectOfType<InterfaceManager>();
        redBall.OnHit += RedBall_OnHit;
    }

    private void RedBall_OnHit()
    {
        iManager.currentLevelIndex++;
        iManager.Reload();
    }
}
