using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputPanelTester : MonoBehaviour
{
    InputPanelController inputPanelController;

    [SerializeField]
    private InputFragment inputFragmentPrefab;

    // Start is called before the first frame update
    void Start()
    {
        inputPanelController = GetComponent<InputPanelController>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inputPanelController.Break();
        }
    }
}
