using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private Rigidbody bulletRigidBody;
    private Collider bulletColllider;
    public GameObject playerGun, player;
    public float damage;

    // Start is called before the first frame update
    void Awake()
    {
        bulletColllider = gameObject.GetComponent<Collider>();
        playerGun = GameObject.Find("Tommy_gun_2");
        player = GameObject.Find("playerCam");
        bulletRigidBody = GetComponent<Rigidbody>();

        Invoke("bulletDespawn", 10f); 
    }

    // Update is called once per frame

    void bulletDespawn()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 6)
        {
            bulletDespawn();
        }
    }
}
