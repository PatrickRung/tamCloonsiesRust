using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitanMovement : movement
{
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
    }
    //wallrun inputs removed, disabling wallrun
    public new void WallRunInput()
    {

    }
}