using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //[SerializeField]
    private Transform cameraTarget;
    //[SerializeField]
    private GameObject mainPlayerCamera;

    public float zCameraPosition;

    private Vector3 targetPosition;

    private void Start()
    {
        //find the player and players camera
        cameraTarget = GameObject.Find("PlayerControler").transform;
        mainPlayerCamera = GameObject.Find("MainPlayerCamera");
    }
    // Update is called once per frame
    public void Update()
    {
        //if camera is found
        if(cameraTarget != null) 
        {
            //position of the target is stored in an vector3
            targetPosition = cameraTarget.position;
            //Set z position of the camera
            targetPosition.z = zCameraPosition;
            //position of the camera equals that of the target
            mainPlayerCamera.transform.position = targetPosition;
        }
    }
}
