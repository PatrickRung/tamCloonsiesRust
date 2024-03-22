using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class concussionMine : BulletScript
{
    
    public new void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.layer == 6||collision.gameObject.layer == 9)
        {
            bulletRigidBody.velocity=Vector3.zero;
            CancelInvoke();
        }
        if (collision.gameObject.layer == 7)
        {

            Vector3 direction = new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y - .5f, collision.gameObject.transform.position.z) - new Vector3(transform.position.x, transform.position.y, transform.position.z);
            collision.gameObject.GetComponent<Rigidbody>().AddForce(1000*(direction));
            bulletDespawn();

        }
    }
}
