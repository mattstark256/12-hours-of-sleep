﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickStart : MonoBehaviour
{
    public void Begin(int Index)
    {
        SceneManager.LoadScene(Index);
    }
}
