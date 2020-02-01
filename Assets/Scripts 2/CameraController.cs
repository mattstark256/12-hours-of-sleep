using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Vector3 camPosition;

    
    ScreenShake screenShake;

    // Start is called before the first frame update
    void Start()
    {
       // camPosition = Vector3.zero;
        screenShake = GetComponent<ScreenShake>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = camPosition + screenShake.GetShakePosition();
        transform.rotation = Quaternion.identity * screenShake.GetShakeRotation();
    }
}
