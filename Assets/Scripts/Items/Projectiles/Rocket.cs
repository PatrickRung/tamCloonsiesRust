using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : concussionMine
{
    public float radius;
    public new void OnCollisionEnter(Collision collision) {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        for(int i = 0; i < colliders.Length; i++) {
            if(colliders[i].gameObject.TryGetComponent<movement>(out movement players)) {
                Debug.Log("spawning for players");
                explosion(colliders[i].gameObject);
            }
        }
    }
}
