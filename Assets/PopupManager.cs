using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoBehaviour
{
    // Singleton
    private static PopupManager instance;
    public static PopupManager Instance { get { return instance; } }


    [SerializeField]
    private Popup popupPrefab;
    [SerializeField]
    private Transform popupParent;

    private bool popupOpen;
    private Popup currentPopup;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }


    public void ShowPopup(string text)    { StartCoroutine(ShowPopupCoroutine(text));   }
    private IEnumerator ShowPopupCoroutine(string text)
    {
        while (popupOpen)
        {
            yield return null;
        }

        currentPopup = Instantiate(popupPrefab, popupParent);
        currentPopup.InitializePopup(this);
        currentPopup.SetText(text);
        popupOpen = true;
    }


    public void CloseCurrentPopup()
    {
        Destroy(currentPopup.gameObject);
        currentPopup = null;
        popupOpen = false;
    }
}
