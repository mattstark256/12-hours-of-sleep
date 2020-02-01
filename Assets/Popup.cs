using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField]
    private Text text;

    private PopupManager popupManager;

    public void InitializePopup(PopupManager _popupManager)
    {
        popupManager = _popupManager;
    }

    public void SetText(string newText)
    {
        text.text = newText;
    }

    public void ClosePopup()
    {
        popupManager.CloseCurrentPopup();
    }
}
