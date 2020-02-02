using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupTrigger : MonoBehaviour
{
    [SerializeField]
    private string popupText = "default text";

    private bool hasShownPopup = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasShownPopup)
        {
            PopupManager.Instance.ShowPopup(popupText);
            hasShownPopup = true;
        }
    }
}
