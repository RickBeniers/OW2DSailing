using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerYMovementController : MonoBehaviour
{
    public GameObject pControler;
    private Rigidbody2D pControllerRb2d;

    [SerializeField]
    private float forwardWaterSpeed;
    private float speedDir;

    private bool playerMoveing;

    private float addSpeed;
    [SerializeField]
    private Vector3 pos;
    private Vector3 Velocity;
    
    public void Awake()
    {
        //assign proper variables
        //pControler = GameObject.Find("PlayerControler");
        pControllerRb2d = pControler.GetComponent<Rigidbody2D>();
        //Debug.Log("Ymovement");
    }
    public void Start()
    {
        //set player movement varibles to 0 at start
        forwardWaterSpeed = 0f;
    }
    public void PlayerForwardSpeedAssignment(int speedStatusPara)
    {
        switch (speedStatusPara)
        {
            case 0:
                // ship is stationary
                forwardWaterSpeed = 0f;
                speedDir = 1f;
                playerMoveing = false;
                PlayerForwardSpeedExecution(forwardWaterSpeed, speedDir, playerMoveing);
                FindObjectOfType<PlayerMovementCalculator>().GetPlayerMovementDetection(playerMoveing);
                //Debug.Log("gkseyesryk");
                break;
            case 1:
                // WaterSpeed = very slow
                forwardWaterSpeed = 1f;
                speedDir = 1f;
                playerMoveing = true;
                PlayerForwardSpeedExecution(forwardWaterSpeed, speedDir, playerMoveing);
                FindObjectOfType<PlayerMovementCalculator>().GetPlayerMovementDetection(playerMoveing);
                break;
            case 2:
                // WaterSpeed = slow
                forwardWaterSpeed = 2f;
                speedDir = 1f;
                playerMoveing = true;
                PlayerForwardSpeedExecution(forwardWaterSpeed, speedDir, playerMoveing);
                FindObjectOfType<PlayerMovementCalculator>().GetPlayerMovementDetection(playerMoveing);
                break;
            case 3:
                // WaterSpeed = battle
                forwardWaterSpeed = 2.5f;
                speedDir = 1f;
                playerMoveing = true;
                PlayerForwardSpeedExecution(forwardWaterSpeed, speedDir, playerMoveing);
                FindObjectOfType<PlayerMovementCalculator>().GetPlayerMovementDetection(playerMoveing);
                break;
            case 4:
                // WaterSpeed = fast
                forwardWaterSpeed = 3f;
                speedDir = 1f;
                playerMoveing = true;
                PlayerForwardSpeedExecution(forwardWaterSpeed, speedDir, playerMoveing);
                FindObjectOfType<PlayerMovementCalculator>().GetPlayerMovementDetection(playerMoveing);
                break;
            case 5:
                // WaterSpeed = very fast
                forwardWaterSpeed = 3.25f;
                speedDir = 1f;
                playerMoveing = true;
                PlayerForwardSpeedExecution(forwardWaterSpeed, speedDir, playerMoveing);
                FindObjectOfType<PlayerMovementCalculator>().GetPlayerMovementDetection(playerMoveing);
                break;
        }
    }
    private void PlayerForwardSpeedExecution(float speed, float speedDirection, bool playerMovingPara)
    {
        //give player ship a forward velocity(speed)

        //Debug.Log("contact");
        pos = pControler.transform.position;
        if (playerMovingPara == true)
        {
            pos = pControler.transform.position;
            Velocity = new Vector3(0, speedDirection * speed * Time.deltaTime, 0);
            pos += FindObjectOfType<PlayerXMovementController>().rot * Velocity;
            pControler.transform.position = pos;
        }
    }
}
