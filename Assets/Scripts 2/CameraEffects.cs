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

    // Start is called before the first frame update
    void Start()
    {
        screenShake = camera.GetComponent<ScreenShake>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddScreenShake(float amount = 0.333f)
    {
        
        screenShake.AddTrauma(amount);
    }



}
