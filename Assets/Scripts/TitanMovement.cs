using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitanMovement : movement
{
    private float dashCount = 0;
    private float dashCooldown = 2;
    private float timePast = 0;
    //all the same as regular movement except wallrun
    public new void Awake()
    {
        base.Awake();

    }

    new void Start()
    {
        playerScale = transform.localScale;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (health <= 0)
        {
            health = 500;
        }
        if (defaulthealth <= 0)
        {
            defaulthealth = 500;
        }
    }


    private new void FixedUpdate()
    {
        base.FixedUpdate();
        Movement();
    }

    private new void Update()
    {
        MyInput();
        Look();
        if (!grounded)
        {
            smootherJump();
        }
        else
        {
            timeOffGround = 0;
        }
        if (dashCount > 0)
        {
            timePast += Time.deltaTime;
        } else
        {
            timePast = 0;
        }
        Debug.Log(timePast);
        Debug.Log(dashCount);
    }
    //wallrun inputs removed, disabling wallrun
    public override void WallRunInput()
    {
    }
    public void dashReset()
    {
        dashCount--;
        jumpCount--;
    }
    //dash instead of jump
    public override void Jump()
    {
        if (dashCount < 1)
        {
            if (grounded && readyToJump)
            {
                readyToJump = false;
                rb.AddForce(orientation.forward * jumpForce * 5f);
                dashCount++;
                jumpCount++;
                Invoke(nameof(ResetJump), jumpCooldown);
                Invoke(nameof(dashReset), dashCooldown);
                
                
            }
        }
        
    }
}