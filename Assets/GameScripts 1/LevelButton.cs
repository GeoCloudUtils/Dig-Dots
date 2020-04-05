using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[ExecuteInEditMode]
public class LevelButton : MonoBehaviour
{
    public TextMeshProUGUI leveCounter;
    public event UnityAction OnButtonClick;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OpenGame);
    }

    private void OpenGame()
    {
        if (OnButtonClick != null)
            OnButtonClick.Invoke();
    }

    private void Update()
    {
        if (leveCounter == null)
            leveCounter = GetComponentInChildren<TextMeshProUGUI>();
    }

}
