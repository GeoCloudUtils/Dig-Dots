using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class Cloud : MonoBehaviour
{
    public float maxX, minX;
    void Start()
    {
        bool left = UnityEngine.Random.value > 0.5f;
        transform.DOMoveX(left ? minX : maxX, UnityEngine.Random.Range(120f, 200f)).OnComplete(Move);
    }
    private void Move()
    {
        transform.DOMoveX(transform.position.x > 0f ? minX : maxX, UnityEngine.Random.Range(120f, 200f)).OnComplete(Move);
    }
}
