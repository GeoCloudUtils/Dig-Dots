using ScriptUtils.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelController : MonoBehaviour
{
    public Rigidbody2D redBall;
    public Rigidbody2D blueBall;
    public event UnityAction<bool> gameDoneEvent;
    void Start()
    {
        redBall.gameObject.AddComponent<ColliderEventSystem>().ColliderEntered += GameController_ColliderEntered;
    }
    private void GameController_ColliderEntered(ColliderEventSystem eventTarget, Collider2D other)
    {
        if (other.tag == "blueBall")
        {
            redBall.GetComponent<CircleCollider2D>().enabled = false;
            blueBall.GetComponent<CircleCollider2D>().enabled = false;
            redBall.bodyType = RigidbodyType2D.Kinematic;
            blueBall.bodyType = RigidbodyType2D.Kinematic;
            blueBall.angularVelocity = 0f;
            blueBall.velocity = Vector2.zero;
            redBall.angularVelocity = 0f;
            redBall.velocity = Vector2.zero;
            GameDone();
        }
    }
    private void GameDone()
    {
        if (gameDoneEvent != null)
            gameDoneEvent.Invoke(true);
    }
}
