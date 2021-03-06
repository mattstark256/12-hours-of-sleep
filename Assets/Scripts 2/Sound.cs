﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound 
{
 

    [SerializeField]
    public string name;
    [SerializeField]
    public AudioClip clip;
    [SerializeField]
    [Range(0,1)]
    public float volume = 0.8f;
    [SerializeField]
    public bool loop;
    [HideInInspector]
    public AudioSource source;



}

//[System.Serializable]
//public class Music : Sound
//{
//    [SerializeField]
//    public bool symbolCrash;
//}
