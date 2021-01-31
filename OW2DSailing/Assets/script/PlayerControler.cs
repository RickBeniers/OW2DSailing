using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    [SerializeField]
    private GameObject pControler;

    private int groundSpeedStatus;
    private float groundSpeed;

    private float xMove;
    private float zMove;

    public void Update()
    {
        
    }
    public void Start()
    {
        groundSpeed = 0;
        groundSpeedStatus = 0;
    }
    public void playerMovementManager() 
    {
        if (groundSpeed == 0 && groundSpeedStatus == 0) 
        { 
            //if W is pressed & speed = 0, change speed to very slow.
            //if W is pressed & speed = very slow, change speed to slow.
            //if W is pressed & speed = slow, change speed to half.
            //if W is pressed & speed = half, change speed to fast.
            //if W is pressed & speed = fast, change speed to very fast.

            //if S is pressed & speed = very fast, change speed to fast.
            //if S is pressed & speed = fast, change speed to half.
            //if S is pressed & speed = half, change speed to slow.
            //if S is pressed & speed = slow, change speed to very slow.
            //if S is pressed & speed = very slow, change speed to 0.
        }
    }
}
