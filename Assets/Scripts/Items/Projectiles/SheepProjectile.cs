using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using Unity.Mathematics;

public class SheepProjectile : concussionMine
{
    public NetworkVariable<Quaternion> rotation = new NetworkVariable<Quaternion>();
    void FixedUpdate()
    {
        if(Input.GetMouseButton(1)) {
            rotation.Value = worldItems.PlayerCamera.transform.rotation;
            
            bulletRigidBody.AddForce(transform.forward * 100);
        }
        gameObject.transform.rotation = rotation.Value;
    }
}
