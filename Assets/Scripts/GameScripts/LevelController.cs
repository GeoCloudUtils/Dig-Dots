using ScriptUtils.Events;
using ScriptUtils.Visual;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LevelController : MonoBehaviour
{
    public ParticleCleanerEvent[] FX;
    public Rigidbody2D redBall;
    public Rigidbody2D blueBall;
    public event UnityAction<bool> gameDoneEvent;

    private int currentLevel = 0;
    void Start()
    {
        redBall.gameObject.AddComponent<ColliderEventSystem>().ColliderEntered += GameController_ColliderEntered;
        blueBall.gameObject.AddComponent<ColliderEventSystem>().ColliderEntered += GameController_ColliderEntered;
        currentLevel = PlayerPrefs.GetInt("levelIndex");
    }
    private void GameController_ColliderEntered(ColliderEventSystem eventTarget, Collider2D other)
    {
        if (other.tag == "blueBall" || other.tag == "obstacle")
        {
            FreezeBalls();
            GameDone(other.tag == "blueBall", eventTarget);
        }
    }
    private void FreezeBalls()
    {
        redBall.GetComponent<CircleCollider2D>().enabled = false;
        blueBall.GetComponent<CircleCollider2D>().enabled = false;
        redBall.bodyType = RigidbodyType2D.Kinematic;
        blueBall.bodyType = RigidbodyType2D.Kinematic;
        blueBall.angularVelocity = 0f;
        blueBall.velocity = Vector2.zero;
        redBall.angularVelocity = 0f;
        redBall.velocity = Vector2.zero;
    }
    private void GameDone(bool complete, ColliderEventSystem target)
    {
        if (!complete)
        {
            target.gameObject.SetActive(false);
            Instantiate(FX[target.name == "Red" ? 0 : 1], new Vector3(target.transform.position.x, target.transform.position.y, -1.5f), Quaternion.identity);
        }
        PlayerPrefs.SetString("Level" + (currentLevel + 1).ToString(), "Level" + (currentLevel + 1).ToString());
        if (gameDoneEvent != null)
            gameDoneEvent.Invoke(complete);
    }
}
