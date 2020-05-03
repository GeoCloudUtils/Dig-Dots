using ScriptUtils.Extensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollLevel : MonoBehaviour
{
    public TextMeshProUGUI bottomIndicatorText;
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI chapterText;
    public string chapter = "";
    public int min, max;
    public Image hoverImage;
    public Image checkImage;
    public bool isFirstLevel = false;
    public bool complete = false;
    public bool isBlocked = true;

    private Image[] allChapterImages;
    private void Awake()
    {
        allChapterImages = GetComponentsInChildren<Image>();
        DoBlock();
    }
    private void Start()
    {
        if (isFirstLevel)
            Unblock();
        bottomIndicatorText.SetText(min + "/" + max);
        int ch_index = transform.GetSiblingIndex();
        chapterText.SetText("Chapter " + (ch_index + 1));
        SetSectionUI();
    }
    public void DoBlock()
    {
        GetComponent<Button>().interactable = false;
        hoverImage.gameObject.SetActive(true);
        foreach (Image img in allChapterImages)
            img.gameObject.GetOrAddComponent<_2dxFX_GrayScale>();
    }

    private void Update()
    {
        complete = chapterComplete();
    }
    public void Unblock()
    {
        if (isBlocked)
            isBlocked = false;
        GetComponent<Button>().interactable = true;
        hoverImage.gameObject.SetActive(false);
        if (checkImage.GetComponent<_2dxFX_GrayScale>())
            Destroy(checkImage.GetComponent<_2dxFX_GrayScale>());
        foreach (Image img in allChapterImages)
        {
            if (img.GetComponent<_2dxFX_GrayScale>())
                Destroy(img.GetComponent<_2dxFX_GrayScale>());
        }
        PlayerPrefs.SetString("CHAPTER", "CHAPTER" + chapter);
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
    public bool chapterComplete()
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
