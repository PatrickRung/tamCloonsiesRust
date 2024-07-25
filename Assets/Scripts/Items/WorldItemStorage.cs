//used for storing references to objects that could be inactive 

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItemStorage : MonoBehaviour
{
    //universially required gameobjects
    public bool multiplayerEnabled;
    public GameObject itemDesc, 
        itemName, 
        ammoCount, 
        sensitivitySlider, 
        menu, 
        player, 
        PlayerCamera;
    //gameobjects required for online
    public GameObject entitySpawnHandling,
                      RocketLauncherGameController;
}
