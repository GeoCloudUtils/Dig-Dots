using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollLevel : MonoBehaviour
{
    public ScrollLevel lastTabLevel;
    public TextMeshProUGUI bottomIndicatorText;
    public TextMeshProUGUI levelCountText;
    public TextMeshProUGUI infoText;
    public int min, max;
    public Image hoverImage;
    public Image checkImage;
    public Image levelImage;
    public bool isFirstLevel = false;
    public bool isBlocked = true;
    public bool complete = false;
    private void Start()
    {
        if (isFirstLevel)
            Unblock();
        levelCountText.text = max.ToString();
        bottomIndicatorText.SetText(min + "/" + max);
        SetSectionUI();
        if (!isFirstLevel)
        {
            if (isLastSectionComplete())
                Unblock();
            else
                DoBlock();
        }
    }
    private void Awake()
    {
        isBlocked = true;
    }
    public void DoBlock()
    {
        GetComponent<Button>().interactable = false;
        hoverImage.gameObject.SetActive(true);
        if (checkImage.GetComponent<_2dxFX_GrayScale>() == null)
            checkImage.gameObject.AddComponent<_2dxFX_GrayScale>();
        if (levelImage.GetComponent<_2dxFX_GrayScale>() == null)
            levelImage.gameObject.AddComponent<_2dxFX_GrayScale>();
    }

    private void Update()
    {
        complete = isFirstLevel ? firstComplete() : isLastSectionComplete();
        if (!isFirstLevel)
        {
            if (lastTabLevel.complete)
            {
                if (isBlocked)
                    Unblock();
            }
        }
    }
    public void Unblock()
    {
        isBlocked = false;
        GetComponent<Button>().interactable = true;
        hoverImage.gameObject.SetActive(false);
        if (checkImage.GetComponent<_2dxFX_GrayScale>())
            Destroy(checkImage.GetComponent<_2dxFX_GrayScale>());
        if (levelImage.GetComponent<_2dxFX_GrayScale>())
            Destroy(levelImage.GetComponent<_2dxFX_GrayScale>());
    }
    private void SetSectionUI()
    {
        int index = 0;
        for (int i = min; i <= max; i++)
        {
            string level = PlayerPrefs.GetString("Level" + i.ToString());
            if (level != "")
                index++;
        }
        infoText.SetText(index + " from " + 12 + " levels completed!");
    }
    public bool isLastSectionComplete()
    {
        for (int i = min - 12; i <= max - 12; i++)
        {
            string key = PlayerPrefs.GetString("Level" + i.ToString());
            if (!PlayerPrefs.HasKey(key))
                return false;
        }
        return true;
    }
    private bool firstComplete()
    {
        for (int i = min; i <= max; i++)
        {
            string key = PlayerPrefs.GetString("Level" + i.ToString());
            if (!PlayerPrefs.HasKey(key))
                return false;
        }
        return true;
    }
}
