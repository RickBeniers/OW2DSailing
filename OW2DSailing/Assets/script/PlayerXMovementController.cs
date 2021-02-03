using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerXMovementController : MonoBehaviour
{
    [SerializeField]
    private GameObject pControler;
    private Rigidbody2D pControllerRb2d;

    [SerializeField]
    private float horizontalWaterSpeed;
    [SerializeField]
    private int playerTurnDirection;

    private Vector2 PlayerRotation;

    public void Awake()
    {
        //assign proper variables
        pControler = GameObject.Find("PlayerControler");
        pControllerRb2d = pControler.GetComponent<Rigidbody2D>();
        //Debug.Log("xmovement");
    }
    public void PlayerTurnRateAssignment(int playerSpeedPara,int playerTurnDirectionPara) 
    {
        playerTurnDirection = playerTurnDirectionPara;
        if(playerTurnDirection == 0) 
        {
            horizontalWaterSpeed = 0f;
        }
        switch (playerSpeedPara) 
        {
            case 0:
                horizontalWaterSpeed = 0f;
                PlayerTurnRateExecution(horizontalWaterSpeed);
                break;
            case 1:
                if(playerTurnDirection == 1) 
                {
                    horizontalWaterSpeed = 0.005f;
                }else if(playerTurnDirection == -1) 
                {
                    horizontalWaterSpeed = -0.005f;
                }
                PlayerTurnRateExecution(horizontalWaterSpeed);
                break;
            case 2:
                if (playerTurnDirection == 1)
                {
                    horizontalWaterSpeed = 0.01f;
                }
                else if (playerTurnDirection == -1)
                {
                    horizontalWaterSpeed = -0.01f;
                }
                PlayerTurnRateExecution(horizontalWaterSpeed);
                break;
            case 3:
                if (playerTurnDirection == 1)
                {
                    horizontalWaterSpeed = 0.06f;
                }
                else if (playerTurnDirection == -1)
                {
                    horizontalWaterSpeed = -0.06f;
                }
                PlayerTurnRateExecution(horizontalWaterSpeed);
                break;
            case 4:
                if (playerTurnDirection == 1)
                {
                    horizontalWaterSpeed = 0.08f;
                }
                else if (playerTurnDirection == -1)
                {
                    horizontalWaterSpeed = -0.08f;
                }
                PlayerTurnRateExecution(horizontalWaterSpeed);
                break;
            case 5:
                if (playerTurnDirection == 1)
                {
                    horizontalWaterSpeed = 0.1f;
                }
                else if (playerTurnDirection == -1)
                {
                    horizontalWaterSpeed = -0.1f;
                }
                PlayerTurnRateExecution(horizontalWaterSpeed);
                break;
        }
    }
    private void PlayerTurnRateExecution(float turnRatePara)
    {
        //Rotate the player ship
        pControler.transform.Rotate(0, 0, -turnRatePara);

        //give player ship a sideways velocity(speed)
        PlayerRotation = new Vector2(turnRatePara, 0f);
        pControllerRb2d.AddForce(PlayerRotation);
    }
}
