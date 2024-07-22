using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Netcode;
using Unity.Properties;
using UnityEngine;

public class EntitySpawnHandler : NetworkBehaviour
{
    public NetworkPrefabsList NetworkPrefabs;
    public PlayerController PlayerCamera;
    public NetworkVariable<bool> SpawnStatus = new NetworkVariable<bool>(false, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public NetworkVariable<ulong> SpawnID = new NetworkVariable<ulong>(0, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
    public Queue<GameObject> spawnRequest = new Queue<GameObject>();
    void Start() {
        SpawnStatus.Value = false;
    }

    //declaring a variable within the parameters means that it is optional
    public async Task<ulong> SpawnEntity(GameObject Entity, 
                                                Vector3 position = new Vector3(), 
                                                Quaternion rotation = new Quaternion(), 
                                                Transform parent = null,
                                                Vector3 force = new Vector3()) {
        int indexOfPrefab = -1;
        for(int i = 0; i < NetworkPrefabs.PrefabList.Count; i++) {
            if(NetworkPrefabs.PrefabList[i].Prefab.name == Entity.name) {
                indexOfPrefab = i;
            }
        }
        SpawnEntityRpc(indexOfPrefab, position, rotation);
        while(!SpawnStatus.Value) {
            await Task.Delay(25);
        }
        ResetSpawningSequenceRPC();
        return SpawnID.Value;
    }

    [Rpc(SendTo.Server)]
    void ResetSpawningSequenceRPC() {
        SpawnStatus.Value = false;
    }

    public void NetworkEntityAddForce(ulong NetworkObjectID, Vector3 force) {

        NetworkEntityAddForceRPC(NetworkObjectID, force);
    }
    [Rpc(SendTo.Server)]
    void NetworkEntityAddForceRPC(ulong NetworkObjectID, Vector3 force) {
        if (NetworkManager.Singleton.SpawnManager.SpawnedObjects.TryGetValue(NetworkObjectID, out NetworkObject netObj)) {
            netObj.gameObject.GetComponent<Rigidbody>().useGravity = false;
            netObj.gameObject.GetComponent<Rigidbody>().AddForce(force);
        }
    }


    [Rpc(SendTo.Server)]
    void SpawnEntityRpc(int indexOfPrefab, Vector3 position = new Vector3(), Quaternion rotation = new Quaternion(), Vector3 force = new Vector3()) {
        Debug.Log("server recieved messege " + indexOfPrefab);
        if(indexOfPrefab == -1) {
            Debug.Log("gameObject does not exist");
            return;
        } 
        GameObject EntitySpawned = null;

        EntitySpawned = NetworkManager.Instantiate(NetworkPrefabs.PrefabList[indexOfPrefab].Prefab, position, rotation);
        EntitySpawned.GetComponent<NetworkObject>().Spawn();
        SpawnStatus.Value = true;
        SpawnID.Value = EntitySpawned.GetComponent<NetworkObject>().NetworkObjectId;
        EntitySpawned.transform.position = position;
        EntitySpawned.transform.rotation = rotation;
        if (EntitySpawned.TryGetComponent<Rigidbody>(out Rigidbody rb)) {
            rb.useGravity = false;
            rb.AddForce(force);
        }

    }
}
