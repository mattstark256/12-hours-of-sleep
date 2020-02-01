using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [SerializeField]
    private Vector3 shakePosition;
    [SerializeField]
    private Quaternion shakeRotation;
    // normalised screenshake factor
    [SerializeField]
    private float trauma;

    // Screenshake variables
    [SerializeField]
    [Range(0f, 5f)]
    float translationFactor = 1f; // distance camera position is transformed

    [SerializeField]
    [Range(0f, 90f)]
    float rotationFactor = 15f; // amount camera rotates (in degrees)

    [SerializeField]
    [Range(0.1f, 100f)]
    float translationSpeed = 15f;

    [SerializeField]
    [Range(0.1f, 100f)]
    float rotationSpeed = 15f;

    // Trauma variables
    [SerializeField]
    bool traumaDrop = true;

    [SerializeField]
    [Range(0.01f, 3f)]
    float traumaDropoffRate = 0.7f;

    // Perlin noise variables
    float xInitialSeed;
    float yInitialSeed;
    float rotationInitialSeed;

    float xseed;
    float yseed;
    float rotationSeed;

    public Vector3 GetShakePosition()
    {
        return shakePosition;
    }

    public Quaternion GetShakeRotation()
    {
        return shakeRotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Perlin noise seeds
        xInitialSeed = Random.Range(1f, 999999f);
        yInitialSeed = Random.Range(1f, 999999f);
        rotationInitialSeed = Random.Range(1f, 999999f);

        xseed = xInitialSeed;
        yseed = yInitialSeed;
        rotationSeed = rotationInitialSeed;
        shakePosition = Vector3.zero;
        shakeRotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
       
        

        TraumaDropOff();
        float shakeFactor = GetTrauma();
        if (shakeFactor > 0.001f)
        {
            CalculateScreenshake(shakeFactor);
        }
        else
        {
            shakePosition = Vector3.zero;
            shakeRotation = Quaternion.identity;
        }

    }
    public float GetTrauma()
    {
        return trauma * trauma;
    }
    public void AddTrauma(float _trauma = 0.333f)
    {
        trauma += _trauma;
    }
    void TraumaDropOff()
    {

        if (trauma > 1) trauma = 1f;
        if (trauma > 0 && traumaDrop == true) trauma -= traumaDropoffRate * Time.deltaTime;
        if (trauma < 0) trauma = 0;
    }

    void CalculateScreenshake(float shakeFactor)
    {

        Vector3 traumaOffset;
        Quaternion traumaRotation;

        xseed += Time.deltaTime * translationSpeed;
        yseed += Time.deltaTime * translationSpeed;
        rotationSeed += Time.deltaTime * rotationSpeed;

        float xOffset = (0.5f - Mathf.PerlinNoise(xInitialSeed + xseed, xInitialSeed + 1 + xseed)) * 2;
        float yOffset = (0.5f - Mathf.PerlinNoise(yInitialSeed + yseed, yInitialSeed + 1 + yseed)) * 2;
        float rotationOffsetNormalised = (0.5f - Mathf.PerlinNoise(rotationInitialSeed + rotationSeed, rotationInitialSeed + 1 + rotationSeed)) * 2;

        traumaOffset = new Vector3(xOffset * translationFactor * trauma, yOffset * translationFactor * trauma, 0);
        float rotationAmount = rotationOffsetNormalised * rotationFactor * trauma;

        traumaRotation = Quaternion.AngleAxis(rotationAmount, Vector3.back);

        shakeRotation = traumaRotation;
        shakePosition = traumaOffset;

    }

   
}
