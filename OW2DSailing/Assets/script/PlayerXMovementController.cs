using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerXMovementController : MonoBehaviour
{
    //[SerializeField]
    private GameObject pControler;
    private Rigidbody2D pControllerRb2d;

    //[SerializeField]
    private float horizontalWaterSpeed;
    private float turnRate;
    private float zDir;

    //[SerializeField]
    private int playerTurnDirection;

    private bool playerMoveing;

    private Vector3 PlayerRotation;

    public Quaternion rot;

    public void Awake()
    {
        //assign proper variables
        pControler = GameObject.Find("PlayerControler");
        pControllerRb2d = pControler.GetComponent<Rigidbody2D>();
        //Debug.Log("xmovement");
    }
    private void Update()
    {
        //Quaternion rot = pControler.transform.rotation;
        //zDir = rot.eulerAngles.z;
        //zDir += -Input.GetAxis("Horizontal") * 360f * Time.deltaTime;
        //rot = Quaternion.Euler(0, 0, zDir);
        //pControler.transform.rotation = rot;
    }
    public void PlayerTurnRateAssignment(int playerSpeedStatusPara,int playerTurnDirectionPara) 
    {
        playerTurnDirection = playerTurnDirectionPara;
        if(playerTurnDirection == 0) 
        {
            horizontalWaterSpeed = 0f;
        }
        switch (playerSpeedStatusPara) 
        {
            case 0:
                //ship is stationary
                horizontalWaterSpeed = 0f;
                turnRate = 0f;
                PlayerTurnRateExecution(turnRate, horizontalWaterSpeed);
                playerMoveing = false;
                FindObjectOfType<PlayerMovementCalculator>().GetPlayerMovementDetection(playerMoveing);
                break;
            case 1:
                // WaterSpeed = very slow & turndirection = left(-1) or right(1)
                if (playerTurnDirection == 1) 
                {
                    turnRate = 1f;
                }else if(playerTurnDirection == -1) 
                {
                    turnRate = -1f;
                }
                else if (playerTurnDirection == 0)
                {
                    turnRate = 0f;
                }
                horizontalWaterSpeed = 10f;
                PlayerTurnRateExecution(turnRate, horizontalWaterSpeed);
                playerMoveing = true;
                FindObjectOfType<PlayerMovementCalculator>().GetPlayerMovementDetection(playerMoveing);
                break;
            case 2:
                // WaterSpeed = slow
                if (playerTurnDirection == 1)
                {
                    turnRate = 1f;
                }
                else if (playerTurnDirection == -1)
                {
                    turnRate = -1f;
                }
                else if (playerTurnDirection == 0)
                {
                    turnRate = 0f;
                }
                horizontalWaterSpeed = 12f;
                PlayerTurnRateExecution(turnRate, horizontalWaterSpeed);
                playerMoveing = true;
                FindObjectOfType<PlayerMovementCalculator>().GetPlayerMovementDetection(playerMoveing);
                break;
            case 3:
                // WaterSpeed = battle
                if (playerTurnDirection == 1)
                {
                    turnRate = 1f;
                }
                else if (playerTurnDirection == -1)
                {
                    turnRate = -1f;
                }
                else if (playerTurnDirection == 0)
                {
                    turnRate = 0f;
                }
                horizontalWaterSpeed = 14f;
                PlayerTurnRateExecution(turnRate, horizontalWaterSpeed);
                playerMoveing = true;
                FindObjectOfType<PlayerMovementCalculator>().GetPlayerMovementDetection(playerMoveing);
                break;
            case 4:
                // WaterSpeed = fast
                if (playerTurnDirection == 1)
                {
                    turnRate = 1f;
                }
                else if (playerTurnDirection == -1)
                {
                    turnRate = -1f;
                }
                else if (playerTurnDirection == 0)
                {
                    turnRate = 0f;
                }
                horizontalWaterSpeed = 16f;
                PlayerTurnRateExecution(turnRate, horizontalWaterSpeed);
                playerMoveing = true;
                FindObjectOfType<PlayerMovementCalculator>().GetPlayerMovementDetection(playerMoveing);
                break;
            case 5:
                // WaterSpeed = very fast
                if (playerTurnDirection == 1)
                {
                    turnRate = 1f;
                }
                else if (playerTurnDirection == -1)
                {
                    turnRate = -1f;
                }else if(playerTurnDirection == 0) 
                {
                    turnRate = 0f;
                }
                horizontalWaterSpeed = 19f;
                PlayerTurnRateExecution(turnRate, horizontalWaterSpeed);
                playerMoveing = true;
                FindObjectOfType<PlayerMovementCalculator>().GetPlayerMovementDetection(playerMoveing);
                break;
        }
    }
    public void GetTurnRate(float TurnRatePara) 
    {
        turnRate = TurnRatePara;
    }
    private void PlayerTurnRateExecution(float turnRatePara, float speedPara)
    {
        //Rotate the player ship

        rot = pControler.transform.rotation;
        zDir = rot.eulerAngles.z;
        zDir += -turnRatePara * speedPara * Time.deltaTime;
        rot = Quaternion.Euler(0, 0, zDir);
        pControler.transform.rotation = rot;

    }
}
