using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField]
    private Text text;
    [SerializeField]
    private Button button;

    private PopupManager popupManager;

    public void InitializePopup(PopupManager _popupManager)
    {
        popupManager = _popupManager;
        button.gameObject.SetActive(false);
        StartCoroutine(MakeButtonFadeInCoroutine());
    }

    public void SetText(string newText)
    {
        text.text = newText;
    }

    public void ClosePopup()
    {
        popupManager.CloseCurrentPopup();
    }

    private IEnumerator MakeButtonFadeInCoroutine()
    {
        yield return new WaitForSeconds(1);
        button.gameObject.SetActive(true);
    }
}
