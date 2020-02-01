using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChromaticAberrationScript : MonoBehaviour
{

    [SerializeField]
    [Range(0,1)]
    float defaultChromaticAberration = 0; 

    [SerializeField]
    float traumaFactor = 0; 
    [SerializeField]
    float aberrationFactor= 0;


    [SerializeField]
    Material aberrationMat;
    // Start is called before the first frame update
    void Start()
    {
        aberrationMat.SetFloat("_AberrationAmount", defaultChromaticAberration);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, aberrationMat);
    }

    // Update is called once per frame
    void Update()
    {
        aberrationMat.SetFloat("_AberrationAmount", defaultChromaticAberration + traumaFactor * aberrationFactor);
    }



}
