using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using Unity.Properties;
using UnityEngine;

public class EntitySpawnHandler : NetworkBehaviour
{
    public NetworkPrefabsList NetworkPrefabs;
    public PlayerController PlayerCamera;
    public NetworkVariable<bool> SpawnCode = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    void Start() {
        SpawnCode.Value = false;
    }

    //declaring a variable within the parameters means that it is optional
    public void SpawnEntity(GameObject Entity, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion(), Transform parent = null) {
        int indexOfPrefab = -1;
        Debug.Log(Entity.name);
        for(int i = 0; i < NetworkPrefabs.PrefabList.Count; i++) {
            if(NetworkPrefabs.PrefabList[i].Prefab.name == Entity.name) {
                indexOfPrefab = i;
            }
        }
        SpawnEntityRpc(indexOfPrefab);
        //identifies gameobject so that we have a reference to that specifc prefab created on the server end
    }
    [Rpc(SendTo.Server)]
    void SpawnEntityRpc(int indexOfPrefab, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion()) {
        Debug.Log("server recieved messege " + indexOfPrefab);
        if(indexOfPrefab == -1) {
            Debug.Log("gameObject does not exist");
            return;
        } 
        GameObject EntitySpawned = null;

        EntitySpawned = NetworkManager.Instantiate(NetworkPrefabs.PrefabList[indexOfPrefab].Prefab, position, rotation);
        EntitySpawned.GetComponent<NetworkObject>().Spawn();
        SpawnCode.Value = true;
        // PlayerCamera.addItemToClientRPC(EntitySpawned.name);

        //send back the network id in order to get a refernce to the network object
    }
}
