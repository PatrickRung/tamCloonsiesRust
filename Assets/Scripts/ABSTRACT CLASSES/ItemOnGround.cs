using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnGround : Button
{
    public GameObject gameDesc;

    public void FixedUpdate()
    {
        if(base.returnLookingAtStatus())
        {
            gameDesc.SetActive(true);
        }
        else
        {
            gameDesc.SetActive(false);
        }
    }
    public override void buttonInteraction()
    {

    }
}
