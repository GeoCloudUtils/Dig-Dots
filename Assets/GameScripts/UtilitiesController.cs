using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UtilitiesController : MonoBehaviour
{
    public Button settingCloseButton;
    public Image contentBg;
    public Transform settingContent;
    private Tween contentTween;
    public event UnityAction contentAction;
    void Start()
    {
        settingCloseButton.onClick.AddListener(HideContent);
    }
    private void HideContent()
    {
        if (contentTween != null)
            contentTween.Kill();
        contentBg.DOFade(0f, 0.25f);
        contentTween = settingContent.DORotateQuaternion(Quaternion.Euler(0f, 0f, 25f), 0.2f).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (contentAction != null)
                contentAction.Invoke();
        });
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (IsPointerOverUIElement())
                return;
            else
                HideContent();
        }
    }
    private void OnEnable()
    {
        ShowContent();
    }
    private void ShowContent()
    {
        if (contentTween != null)
            contentTween.Kill();
        contentBg.DOFade(0.5f, 0.25f);
        contentTween = settingContent.DORotateQuaternion(Quaternion.Euler(Vector3.zero), 0.2f).SetEase(Ease.Linear);
    }

    ///Returns 'true' if we touched or hovering on Unity UI element.
    public static bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }
    ///Returns 'true' if we touched or hovering on Unity UI element.
    public static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.tag == "utilitiesPanel")
                return true;
        }
        return false;
    }
    ///Gets all event systen raycast results of current mouse or touch position.
    static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }
}
