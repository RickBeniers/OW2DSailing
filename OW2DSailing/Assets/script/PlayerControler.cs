using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    [SerializeField]
    private int groundSpeedStatus;
    [SerializeField]
    private int groundSpeedTurnStatus;
    [SerializeField]
    private float xMove;

    //[SerializeField]
    private bool yMovePlus;
    //[SerializeField]
    private bool yMoveMinus;
    //[SerializeField]
    private bool xMovePlus;
    //[SerializeField]
    private bool xMoveMinus;

    public void Start()
    {
        //set player movement varibles to 0 at start
        groundSpeedStatus = 0;
    }
    public void Update()
    {
        //check if a key responsible for horizontal(A & D) or vertical(W & S) movement is pressed
        yMovePlus = Input.GetButtonDown("W");
        yMoveMinus = Input.GetButtonDown("S");
        xMove = Input.GetAxisRaw("Horizontal");

        //call forward player movement script
        PlayerMovementManager();
    }

    public void PlayerMovementManager() 
    {
        if (yMovePlus == true) 
        {
            //increase speed

            //Debug.Log("E001");
            groundSpeedStatus++;
            FindObjectOfType<PlayerYMovementController>().PlayerForwardSpeedAssignment(groundSpeedStatus);
            if (groundSpeedStatus > 5) 
            {
                groundSpeedStatus = 5;
            }
        }
        if (yMoveMinus == true) 
        {
            //decrease speed

            //Debug.Log("E002");
            groundSpeedStatus--;
            FindObjectOfType<PlayerYMovementController>().PlayerForwardSpeedAssignment(groundSpeedStatus);
            if (groundSpeedStatus < 0) 
            {
                groundSpeedStatus = 0;
            }
        }

        //call sideways player movement & turnrate script
        if (xMove == 1 && groundSpeedStatus > 0)
        {
            //move the ship right
            xMovePlus = true;
            xMoveMinus = false;
            groundSpeedTurnStatus = 1;
            FindObjectOfType<PlayerXMovementController>().PlayerTurnRateAssignment(groundSpeedStatus, groundSpeedTurnStatus);
        }
        else if (xMove == -1 && groundSpeedStatus > 0)
        {
            //move the ship left
            xMoveMinus = true;
            xMovePlus = false;
            groundSpeedTurnStatus = -1;
            FindObjectOfType<PlayerXMovementController>().PlayerTurnRateAssignment(groundSpeedStatus, groundSpeedTurnStatus);
        }
        else if(groundSpeedStatus < 1 || xMove == 0)
        {
            //stop turning
            groundSpeedTurnStatus = 0;
            xMoveMinus = false;
            xMovePlus = false;
            FindObjectOfType<PlayerXMovementController>().PlayerTurnRateAssignment(groundSpeedStatus, groundSpeedTurnStatus);
        }
    }
}