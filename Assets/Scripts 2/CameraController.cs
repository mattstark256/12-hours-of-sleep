using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private Vector3 camPosition;

    [SerializeField]
    GameObject player;

    ScreenShake screenShake;

    [SerializeField]
    [Range(0,10)]
    float horizontalCameraDamping;
    
    [SerializeField]
    [Range(0,10)]
    float verticalCameraDamping;

    [SerializeField]
    [Range(0,10)]
    float maxDistanceFromCamera = 3;

    Vector3 targetPosition;
    // private Vector3 currentVelocity;

    Camera camera;


    // Start is called before the first frame update
    void Start()
    {
        screenShake = GetComponent<ScreenShake>();
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        float verticalDistanceFromCam = camPosition.y - player.transform.position.y;



        camPosition.x = Mathf.Lerp(camPosition.x, player.transform.position.x, horizontalCameraDamping * Time.deltaTime);

        if(player.GetComponent<PlayerController>().IsOnFloor()) // dont move camera for little jumps
        {
            camPosition.y = Mathf.Lerp(camPosition.y, player.transform.position.y, verticalCameraDamping * Time.deltaTime);
        }

        if(verticalDistanceFromCam > maxDistanceFromCamera)
        {
            // vector from player to camera
            Vector3 playerTOcam = new Vector3(camPosition.x,camPosition.y,0) - player.transform.position;
          //  camPosition.y = new player.transform.position + playerTOcam * maxDistanceFromCamera;
        }

        transform.position = camPosition + screenShake.GetShakePosition();
        transform.rotation = Quaternion.identity * screenShake.GetShakeRotation();
    }
}
