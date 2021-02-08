using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerYMovementController : MonoBehaviour
{
    public GameObject pControler;
    private Rigidbody2D pControllerRb2d;

    [SerializeField]
    private float forwardWaterSpeed;

    private bool playerMoveing;

    private float addSpeed;

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
                PlayerForwardSpeedExecution(forwardWaterSpeed);
                playerMoveing = false;
                FindObjectOfType<PlayerMovementCalculator>().GetPlayerMovementDetection(playerMoveing);
                break;
            case 1:
                // WaterSpeed = very slow
                forwardWaterSpeed = 0.25f;
                PlayerForwardSpeedExecution(forwardWaterSpeed);
                playerMoveing = true;
                FindObjectOfType<PlayerMovementCalculator>().GetPlayerMovementDetection(playerMoveing);
                break;
            case 2:
                // WaterSpeed = slow
                forwardWaterSpeed = 0.5f;
                PlayerForwardSpeedExecution(forwardWaterSpeed);
                playerMoveing = true;
                FindObjectOfType<PlayerMovementCalculator>().GetPlayerMovementDetection(playerMoveing);
                break;
            case 3:
                // WaterSpeed = battle
                forwardWaterSpeed = 1f;
                PlayerForwardSpeedExecution(forwardWaterSpeed);
                playerMoveing = true;
                FindObjectOfType<PlayerMovementCalculator>().GetPlayerMovementDetection(playerMoveing);
                break;
            case 4:
                // WaterSpeed = fast
                forwardWaterSpeed = 1.25f;
                PlayerForwardSpeedExecution(forwardWaterSpeed);
                playerMoveing = true;
                FindObjectOfType<PlayerMovementCalculator>().GetPlayerMovementDetection(playerMoveing);
                break;
            case 5:
                // WaterSpeed = very fast
                forwardWaterSpeed = 2f;
                PlayerForwardSpeedExecution(forwardWaterSpeed);
                playerMoveing = true;
                FindObjectOfType<PlayerMovementCalculator>().GetPlayerMovementDetection(playerMoveing);
                break;
        }
    }
    private void PlayerForwardSpeedExecution(float speed)
    {
        //give player ship a forward velocity(speed)
        pControllerRb2d.velocity = new Vector2(0, speed);
    }
}
