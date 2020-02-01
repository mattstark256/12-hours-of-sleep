using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChromaticAberrationScript : MonoBehaviour
{

    [SerializeField]
    [Range(0,1)]
    float defaultChromaticAberration = 0; 

    [SerializeField]
    float aberrationFactor= 0;

    [SerializeField]
    float trauma = 0;
    [SerializeField]
    float traumaDropoffRate;
    [SerializeField]
    bool traumaDrop = true;


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
        TraumaDropOff();
        aberrationMat.SetFloat("_AberrationAmount", defaultChromaticAberration + trauma * aberrationFactor);
    }

    void TraumaDropOff()
    {

        if (trauma > 1) trauma = 1f;
        if (trauma > 0 && traumaDrop == true) trauma -= traumaDropoffRate * Time.deltaTime;
        if (trauma < 0) trauma = 0;
    }

    public void SetDefaultAberration(float value)
    {
        defaultChromaticAberration = value;
    }
    public void SetAberrationFactor(float value)
    {
        aberrationFactor = value;
    }
    public void SetAbberationTrauma(float value)
    {
        trauma = value;
    }

}
