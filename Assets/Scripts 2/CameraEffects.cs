using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{

    // Singleton
    private static CameraEffects instance;
    public static CameraEffects Instance { get { return instance; } }


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }



    [SerializeField]
    Camera camera;

    ScreenShake screenShake;
    ChromaticAberrationScript aberrations;

    bool aberrationUsesScreenshakeTrauma = false;

    // Start is called before the first frame update
    void Start()
    {
        screenShake = camera.GetComponent<ScreenShake>();
        aberrations = camera.GetComponent<ChromaticAberrationScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (aberrationUsesScreenshakeTrauma)
        {
            SetAbberationTrauma(screenShake.GetTrauma());
        }
    }

    public void AberrationUsesScreenshakeTrauma(bool value)
    {
        aberrationUsesScreenshakeTrauma = value;
    }

    public void AddScreenShakeAndChromaticAberration(float amount = 0.333f)
    {
        screenShake.AddTrauma(amount);
        aberrations.SetAbberationTrauma(amount);
    }

    public void AddScreenShake(float amount = 0.333f)
    {
        screenShake.AddTrauma(amount);

    }

    public void SetDefaultAberration(float value)
    {
        aberrations.SetDefaultAberration( value);
    }

    public void SetAberrationFactor(float value) // factor to be used with trauma
    {
        aberrations.SetAberrationFactor(value);
    }
    public void SetAbberationTrauma(float value) // to be used alongside screenshake trauma
    {
        aberrations.SetAbberationTrauma(value);
    }



}
