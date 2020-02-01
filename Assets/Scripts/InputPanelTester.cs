using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InputPanelTester : MonoBehaviour
{
    [SerializeField]
    private InputPanelController inputPanelController;
    [SerializeField]
    private KeyPickup keyPickup;
    [SerializeField]
    private FragmentPickup fragmentPickup;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inputPanelController.Break();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            inputPanelController.AddKey(keyPickup);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            inputPanelController.AddFragment(fragmentPickup);
        }
    }
}
