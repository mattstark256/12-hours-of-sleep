using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBreaker : MonoBehaviour
{
    [SerializeField]
    private InputPanelController inputPanelController;

    private bool broken = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!broken)
        {
            AudioManager.Instance.Play("game_break");
            inputPanelController.Break();
            broken = true;
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F2)&& Input.GetKeyDown(KeyCode.F5))
        if (!broken)
        {
            inputPanelController.Break();
            broken = true;
        }
    }
}
