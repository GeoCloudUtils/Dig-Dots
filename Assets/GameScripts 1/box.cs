using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using EduUtils.Events;

public class box : MonoBehaviour
{
    public float shakeDuration;
    public float strenght = 1;
    public int vibrato = 10;
    public float randomnes = 90;

    private Tween boxTween = null;
    private Vector3 startPosition;
    private bool inAnimation = false;
    void Start()
    {
        gameObject.AddComponent<MouseEventSystem>().MouseEvent += boxMouseEvent;
        startPosition = transform.position;
    }

    private void boxMouseEvent(GameObject target, MouseEventType type)
    {
        if (type == MouseEventType.CLICK)
            DoAnim();
    }
    private void DoAnim()
    {
        if (inAnimation)
            return;
        if (boxTween != null)
            boxTween.Kill();
        inAnimation = true;
        boxTween = transform.DOShakePosition(shakeDuration, strenght, vibrato, randomnes, false, true).OnComplete(() =>
        {
            inAnimation = false;
            transform.position = startPosition;
            boxTween.Kill();
        });
    }
}
