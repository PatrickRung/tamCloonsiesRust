using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

public class BulletScript : NetworkBehaviour
{
    public Rigidbody bulletRigidBody;
    private Collider bulletColllider;
    public WorldItemStorage worldItems;
    public GameObject playerCam;
    public int damage;
    public GameObject explosion = null;
    private GameObject DebugSphere;

    // Start is called before the first frame update
    void Awake()
    {
        bulletColllider = gameObject.GetComponent<Collider>();
        worldItems = GameObject.Find("World Items").GetComponent<WorldItemStorage>();
        playerCam = worldItems.PlayerCamera;   
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
        if(worldItems.GetComponent<WorldItemStorage>().multiplayerEnabled && gameObject.GetComponent<NetworkRigidbody>() != null) {
            despawnRPC();
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
    [Rpc(SendTo.Server)]
    void despawnRPC() {
        if(!IsServer) return;
        gameObject.GetComponent<NetworkObject>().Despawn();
        Destroy(gameObject);
    }
}
