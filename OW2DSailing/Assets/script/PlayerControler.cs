using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    [SerializeField]
    private GameObject pControler;
    private Rigidbody2D pControllerRb2d;

    [SerializeField]
    private int groundSpeedStatus;
    [SerializeField]
    private float forwardWaterSpeed;
    [SerializeField]
    private float horizontalWaterSpeed;

    private bool yMovePlus;
    private bool yMoveMinus;
    [SerializeField]
    private float xMove;
    //private bool xMoveMinus;

    private Vector2 PlayerRotation;

    public void Update()
    {
        //check if a key responsible for horizontal(A & D) or vertical(W & S) movement is pressed
        yMovePlus = Input.GetButtonDown("W");
        yMoveMinus = Input.GetButtonDown("S");
        xMove = Input.GetAxisRaw("Horizontal");

        //call forward player movement script
        PlayerMovementManager();

        //call sideways player movement & turnrate script
        if (xMove == 1 && groundSpeedStatus > 0) 
        {
            //move the ship right
            PlayerTurnRateExecution(xMove);
        }
        else if (xMove == -1 && groundSpeedStatus > 0) 
        {
            //move the ship left
            PlayerTurnRateExecution(xMove);
        }
    }
    public void Awake()
    {
        //assign proper variables
        pControllerRb2d = pControler.GetComponent<Rigidbody2D>();
    }
    public void Start()
    {
        //set player movement varibles to 0 at start
        forwardWaterSpeed = 0f;
        groundSpeedStatus = 0;
    }
    public void PlayerMovementManager() 
    {
        if (yMovePlus == true) 
        {
            //Debug.Log("E001");
            groundSpeedStatus++;
            PlayerSpeedAssignment(groundSpeedStatus);
            if (groundSpeedStatus > 5) 
            {
                groundSpeedStatus = 5;
            }
        }
        if (yMoveMinus == true) 
        {
            //Debug.Log("E002");
            groundSpeedStatus--;
            PlayerSpeedAssignment(groundSpeedStatus);
            if (groundSpeedStatus < 0) 
            {
                groundSpeedStatus = 0;
            }
        }
    }
    private void PlayerSpeedAssignment(int speedStatusPara) 
    {
        switch (speedStatusPara) 
        {
            case 0:
                // ship is stationary
                forwardWaterSpeed = 0f;
                PlayerForwardSpeedExecution(forwardWaterSpeed);
                break;
            case 1:
                // groundSpeed = very slow
                forwardWaterSpeed = 0.25f;
                PlayerForwardSpeedExecution(forwardWaterSpeed);
                break;
            case 2:
                // groundSpeed = slow
                forwardWaterSpeed = 0.5f;
                PlayerForwardSpeedExecution(forwardWaterSpeed);
                break;
            case 3:
                // groundSpeed = battle
                forwardWaterSpeed = 1f;
                PlayerForwardSpeedExecution(forwardWaterSpeed);
                break;
            case 4:
                // groundSpeed = fast
                forwardWaterSpeed = 1.25f;
                PlayerForwardSpeedExecution(forwardWaterSpeed);
                break;
            case 5:
                // groundSpeed = very fast
                forwardWaterSpeed = 2f;
                PlayerForwardSpeedExecution(forwardWaterSpeed);
                break;
        }
    }
    private void PlayerForwardSpeedExecution(float speed) 
    {
        pControllerRb2d.velocity = new Vector2(0, speed);
    }
    private void PlayerTurnRateExecution(float turnRatePara) 
    {
        pControler.transform.Rotate(0, 0, -turnRatePara * 0.005f);
        PlayerRotation = new Vector2(turnRatePara, 0f);
        pControllerRb2d.AddRelativeForce(PlayerRotation);
    }
}
