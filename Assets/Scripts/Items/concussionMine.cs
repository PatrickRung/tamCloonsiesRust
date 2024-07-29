using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class concussionMine : BulletScript
{
    public bool isLocatedOnServer;
    public new void OnCollisionEnter(Collision collision) {
        //if it colides with wall or floor
        if (collision.gameObject.layer == 6||collision.gameObject.layer == 9)
        {
            bulletRigidBody.velocity=Vector3.zero;
            CancelInvoke();
        }
        //if colides with player or entity
        if (collision.gameObject.layer == 7 || collision.gameObject.layer == 8)
        {
            explosion(collision.gameObject);
        }
    }
    public void explosion(GameObject player)
    {
        Vector3 direction = new Vector3(player.transform.position.x, player.transform.position.y - .5f, player.transform.position.z) 
                                        - new Vector3(transform.position.x, transform.position.y, transform.position.z);
        player.GetComponent<Rigidbody>().AddForce(1000 * direction / (direction.magnitude));
            if(!isLocatedOnServer) return;
            if(worldItems.multiplayerEnabled) {
                player.GetComponent<CharacterTemplate>().changeHealthRPC(-damage);
            }
            else {
                player.GetComponent<CharacterTemplate>().changeHealth(-damage);
            }



        bulletDespawn();
    }
}
