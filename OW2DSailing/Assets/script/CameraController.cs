using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //[SerializeField]
    private Transform cameraTarget;
    //[SerializeField]
    private GameObject mainPlayerCamera;
    //public float zCameraPosition;
    //public Vector3 offSet;
    //private Vector3 targetPosition;

    //needs to be higher than 10f
    public float smoothSpeed;
    public Vector3 offSet;

    private void Start()
    {
        //find the player and players camera
        cameraTarget = GameObject.Find("PlayerControler").transform;
        mainPlayerCamera = GameObject.Find("MainPlayerCamera");
    }
    // Update is called once per frame
    public void FixedUpdate()
    {
        //if camera is found
        if(cameraTarget != null) 
        {
            //position of the target is stored in an vector3
            //targetPosition = cameraTarget.position;
            //Set z position of the camera
            //targetPosition.z = zCameraPosition;
            //position of the camera equals that of the target
            //mainPlayerCamera.transform.position = Vector3.Lerp(mainPlayerCamera.transform.position, cameraTarget.transform.position, offSet);

            CameraFollowManager();
        }
    }
    private void CameraFollowManager() 
    {
        Vector3 desiredPosition = cameraTarget.position + offSet;
        Vector3 smoothedPosition = Vector3.Lerp(mainPlayerCamera.transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        mainPlayerCamera.transform.position = smoothedPosition;
        mainPlayerCamera.transform.LookAt(cameraTarget);
    }
}
