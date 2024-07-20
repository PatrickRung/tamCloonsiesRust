using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Properties;
using UnityEngine;

public class EntitySpawnHandler : NetworkBehaviour
{
    public NetworkPrefabsList NetworkPrefabs;
    public NetworkVariable<int> EntityID = new NetworkVariable<int>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    void Start() {
        EntityID.Value = 0;
    }

    //declaring a variable within the parameters means that it is optional
    public int SpawnEntity(GameObject Entity, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion(), Transform parent = null) {
        int indexOfPrefab = -1;
        for(int i = 0; i < NetworkPrefabs.PrefabList.Count; i++) {
            if(NetworkPrefabs.PrefabList[i].Prefab.name == Entity.name) {
                indexOfPrefab = i;
            }
        }
        SpawnEntityRpc(indexOfPrefab);
        //identifies gameobject so that we have a reference to that specifc prefab created on the server end

        return EntityID.Value;
    }
    [Rpc(SendTo.Server)]
    void SpawnEntityRpc(int indexOfPrefab, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion()) {
        EntityID.Value++;
        Debug.Log("server recieved messege " + indexOfPrefab);
        if(indexOfPrefab == -1) {
            Debug.Log("gameObject does not exist");
            return;
        } 
        GameObject EntitySpawned = null;

        EntitySpawned = NetworkManager.Instantiate(NetworkPrefabs.PrefabList[indexOfPrefab].Prefab, position, rotation);
        Debug.Log("spawned " + EntitySpawned.name + " " + EntityID.Value);
        EntitySpawned.name = EntitySpawned.name + " " + EntityID.Value;
        EntitySpawned.GetComponent<NetworkObject>().Spawn();

    }
}
