using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EntitySpawnHandler : NetworkBehaviour
{
    public NetworkPrefabsList NetworkPrefabs;

    public void SpawnEntity(GameObject Entity) {
        int indexOfPrefab = -1;
        for(int i = 0; i < NetworkPrefabs.PrefabList.Count; i++) {
            if(NetworkPrefabs.PrefabList[i].Prefab == Entity) {
                indexOfPrefab = i;
            }
        }
        SpawnEntityRpc(indexOfPrefab);
    }
    [Rpc(SendTo.Server)]
    void SpawnEntityRpc(int indexOfPrefab) {
        if(indexOfPrefab == -1) return;
        NetworkPrefabs.PrefabList[indexOfPrefab].Prefab.GetComponent<NetworkObject>().Spawn();
    }
}
