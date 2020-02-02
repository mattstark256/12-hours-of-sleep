using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputFixer : MonoBehaviour
{
    [SerializeField]
    private InputPanelController inputPanelController;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FragmentPickup fragmentPickup = collision.GetComponent<FragmentPickup>();
        if (fragmentPickup != null)
        {
            AudioManager.Instance.Play("collect_fragment");
            inputPanelController.AddFragment(fragmentPickup);
        }

        KeyPickup keyPickup = collision.GetComponent<KeyPickup>();
        if (keyPickup != null)
        {
            AudioManager.Instance.Play("collect_key");
            Debug.Log("adding key");
            inputPanelController.AddKey(keyPickup);
        }
    }
}
