using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementCalculator : MonoBehaviour
{
    //[SerializeField]
    private float X;
    //[SerializeField]
    private float Y;
    [SerializeField]
    private float Z;

    //[SerializeField]
    private float xStored;
    //[SerializeField]
    private float yStored;
    //[SerializeField]
    private float zStored;

    private float diffrenceX;
    private float diffrenceY;
    private float diffrenceZ;
    [SerializeField]
    private float Distance;
    [SerializeField]
    private float t;
    [SerializeField]
    private float pVelocity;
    private int moveCounter;
    private int playerIsMoveing = 0;

    private List<Vector4> playerXYZmovementsAndMoveCountList;
    private Vector4 playerXYZmovementsAndMoveCount;

    private bool playerMovement;

    private void Start()
    {
        Invoke(nameof(StorePositon), 1f);
    }
    private void Update()
    {
        if (playerMovement == true) 
        {
            t += Time.deltaTime;
            CalCulatePosition();
            CalculateRotation();
        }
        else 
        {
            t = 0;
        }
    }
    public void GetPlayerPositionRotation(float PosX, float PosY, float RotZ)
    {
        X = PosX;
        Y = PosY;
        Z = RotZ;
   
        //formulas :
        // Velocity(units/seconds) = distance(units) / time(seconds)
        //
        //      / \
        //     /   \
        //    /  S  \
        //   /-------\
        //  /  V * T  \
        //  -----------
    }
    public void GetPlayerMovementDetection(bool PlayerMovePara) 
    {
        playerMovement = PlayerMovePara;
    }
    private void StorePositon()
    {
        xStored = X;
        yStored = Y;
        zStored = Z;
    }
    private void CalCulatePosition() 
    {
        diffrenceX = X - xStored;
        diffrenceY = Y - yStored;
        diffrenceZ = Z - zStored;
        if (diffrenceX < -1) 
        {
            Distance = -diffrenceX + diffrenceY;
            CalculateVelocity();
        }
        else if (diffrenceX > -1) 
        {
            Distance = diffrenceX + diffrenceY;
            CalculateVelocity();
        }
    }
    private void CalculateRotation() 
    {
        diffrenceZ = Z - zStored;
    }
    private void CalculateVelocity() 
    {
        pVelocity = Distance / t;
    }
}
