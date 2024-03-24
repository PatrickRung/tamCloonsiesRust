using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Rigidbody bulletRigidBody;
    private Collider bulletColllider;
    public GameObject playerGun, player;
    public float damage;
    public GameObject explosion = null;

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

    public void bulletDespawn()
    {
        if (!explosion.Equals(null))
        {
            GameObject newPackage = Instantiate(explosion, null);
            newPackage.transform.position = gameObject.transform.position;
        }
        Destroy(gameObject);

    }

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 6)
        {
            bulletDespawn();
        }
    }
}
