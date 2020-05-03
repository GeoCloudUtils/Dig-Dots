using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BetterToggleGroup : ToggleGroup
{
    public Action<Toggle> OnChange;
    public Toggle[] allToggles;
    protected override void Start()
    {
        base.Start();
        allToggles = GetComponentsInChildren<Toggle>();
        ManageListening(true);
    }

    private void ManageListening(bool listen)
    {
        Debug.Log("Listening");
        int count = 0;
        foreach (Transform transformToggle in gameObject.transform)
        {
            count += 1;
            Toggle toggle = transformToggle.gameObject.GetComponent<Toggle>();
            if (listen)
            {
                toggle.onValueChanged.AddListener(OnTog);
            }
            else
            {
                toggle.onValueChanged.RemoveListener(OnTog);
            }
        }
        if (count == 0)
        {
            Debug.LogWarning("No Toggles found in Children. Is your scene set up correctly ?");
        }
        Debug.Log(ActiveToggles());
    }

    private void OnTog(bool isSelected)
    {
        if (isSelected)
        {
            if (OnChange != null)
            {
                OnChange(FirstActiveToggle());
            }
        }
    }

    public Toggle FirstActiveToggle()
    {
        foreach (Toggle t in allToggles)
        {
            return t;
        }
        return null;
    }
}
