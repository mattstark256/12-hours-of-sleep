using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBreaker : MonoBehaviour
{
    [SerializeField]
    private InputPanelController inputPanelController;

    [SerializeField]
    private BreakPopup breakPopupPrefab;
    [SerializeField]
    private Transform breakPopupParent;
    [SerializeField]
    private Collider2D invisibleBarrier;


    private bool broken = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!broken)
        {
            Break();
        }
    }


    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F2)&& Input.GetKeyDown(KeyCode.F5))
        if (!broken)
            {
                Break();
            }
    }


    private void Break()
    {
        broken = true;
        StartCoroutine(BreakCoroutine());

    }

    private IEnumerator BreakCoroutine()
    {
        BreakPopup breakPopup = Instantiate(breakPopupPrefab, breakPopupParent);

        while (!breakPopup.buttonPressed)
        {
            yield return null;
        }

        Destroy(breakPopup.gameObject);
        AudioManager.Instance.Play("game_break");
        inputPanelController.Break();
        broken = true;

        yield return new WaitForSeconds(2.5f);

        Destroy(invisibleBarrier.gameObject);

        PopupManager.Instance.ShowPopup("What have you done?!! You've broken the game! You better try to fix it.");
        PopupManager.Instance.ShowPopup("The only key that's left is the A key. If you want to move right, you'll have to click and drag it into the move right slot.");
    }
}
