﻿using System.Collections;
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
            inputPanelController.Break();
            broken = true;
        }
    }
}