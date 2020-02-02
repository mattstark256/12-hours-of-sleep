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
    float maxDistanceFromCameraUp = 2.4f;
    
    [SerializeField]
    [Range(0,10)]
    float maxDistanceFromCameraDown = 4f;

    Vector3 targetPosition;
     private Vector3 currentVelocity;

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
        Vector3 targetPosition = camPosition;
        targetPosition.x = player.transform.position.x;
        float targetY = player.transform.position.y + 2.5f;
        targetPosition.y = (player.GetComponent<PlayerController>().IsOnFloor()) ?
            targetY :
            Mathf.Clamp(targetPosition.y, targetY - maxDistanceFromCameraUp, targetY + maxDistanceFromCameraDown);

        //if (player.GetComponent<PlayerController>().IsOnFloor()) { Debug.Log("on floor"); }

        camPosition = new Vector3(
            Mathf.Lerp(camPosition.x, targetPosition.x, horizontalCameraDamping * Time.deltaTime),
            Mathf.Lerp(camPosition.y, targetPosition.y, verticalCameraDamping * Time.deltaTime),
            -10);





        //targetPosition = player.transform.position + new Vector3(0,2.5f,0);
        //float verticalDistanceFromCam = camPosition.y - (player.transform.position.y);

        //camPosition.x = Mathf.Lerp(camPosition.x,targetPosition.x, horizontalCameraDamping * Time.deltaTime);




        //if (Mathf.Abs(verticalDistanceFromCam) > maxDistanceFromCameraUp || player.GetComponent<PlayerController>().IsOnFloor()) // dont move camera for little jumps
        //{
        //    camPosition.y = Mathf.Lerp(camPosition.y, targetPosition.y, verticalCameraDamping * Time.deltaTime);
        //}

        //if (Mathf.Abs(verticalDistanceFromCam) > maxDistanceFromCameraDown && verticalDistanceFromCam > 0)
        //{
        //    // camPosition.y = Mathf.Lerp(camPosition.y, targetPosition.y, player.verticalCameraDamping * Time.deltaTime);
        //    // vector from player to camera
        //    //Vector3 playerTOcam = (new Vector3(camPosition.x,camPosition.y,0) -targetPosition).normalized;
        //    //camPosition.y = (player.transform.position + playerTOcam * maxDistanceFromCamera).y;
        //    camPosition = Vector3.SmoothDamp(camPosition, targetPosition + new Vector3(0,0,-10), ref currentVelocity, 2.5f/ Mathf.Abs(player.GetComponent<PlayerController>().GetVelocity().y));
        //}

        transform.position = camPosition + screenShake.GetShakePosition();
        transform.rotation = Quaternion.identity * screenShake.GetShakeRotation();
    }
}
