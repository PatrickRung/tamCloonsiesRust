using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public Transform player;

    public FlagsAttribute maxLookLength;
    //this is actually the camera controller
    //updates camera roation and position according to movement
    void Update()
    {
        transform.position = player.transform.position;
    }

    public GameObject LookingAt;
    public GameObject getLookingAt()
    {
        return LookingAt;
    }
    private void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            if(!object.ReferenceEquals(hit.transform, null))
            {
                LookingAt = hit.transform.gameObject;
            }

        }
        else
        {
            LookingAt = null;
        }

    }
}