using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class SlideShow : MonoBehaviour
{
    public ScrollLevel levelParent;
    public float x_left;
    public float center;
    public float delay;
    public bool makeSlideShow = false;
    public Image slideShowImage;
    private Vector3 startPosition;
    public Sprite[] Icons; // icons array
    private int currentImageIndex = 0;

    private void Awake()
    {
        slideShowImage = GetComponent<Image>();
        delay = UnityEngine.Random.Range(4.0f, 6.0f);
    }
    void Start()
    {
        startPosition = transform.localPosition;
        LoadIcons();
        slideShowImage.sprite = Icons[0];
        DoSlideShow();
    }
    private void DoSlideShow()
    {
        if (transform.localPosition.x == center)
        {
            if (levelParent.isBlocked)
                return;
            transform.DOLocalMoveX(x_left, 0.5f).SetEase(Ease.InOutBack).SetDelay(delay).OnComplete(() =>
            {
                currentImageIndex++;
                transform.localPosition = startPosition;
                if (currentImageIndex >= Icons.Length)
                    currentImageIndex = 0;
                slideShowImage.sprite = Icons[currentImageIndex];
                DoSlideShow();
            });
        }
        else
            transform.DOLocalMoveX(center, 0.5f).SetEase(Ease.OutBack).OnComplete(DoSlideShow);
    }
    void LoadIcons()
    {
        object[] loadedIcons = Resources.LoadAll("LevelsScreenshots/section1", typeof(Sprite));
        Icons = new Sprite[loadedIcons.Length];
        for (int x = 0; x < loadedIcons.Length; x++)
            Icons[x] = (Sprite)loadedIcons[x];
    }
}
