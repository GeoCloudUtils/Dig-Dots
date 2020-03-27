using ScriptUtils.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallController : MonoBehaviour
{
    public Rigidbody2D ballRb;
    public string hitTagName;
    public event UnityAction OnHit;
    void Start()
    {
        gameObject.AddComponent<ColliderEventSystem>().ColliderEntered += BallController_ColliderEntered;
    }
    private void BallController_ColliderEntered(ColliderEventSystem eventTarget, Collider2D other)
    {
        if (other.tag == hitTagName)
        {
            ballRb.bodyType = RigidbodyType2D.Kinematic;
            ballRb.velocity = Vector2.zero;
            ballRb.angularVelocity = 0f;
            other.enabled = false;
            if (OnHit != null)
                OnHit.Invoke();
        }
    }
}
