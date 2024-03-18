using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitanSwitchHandler : MonoBehaviour
{ 
    
    public GameObject titan;
    public GameObject player;
    public GameObject pilotSpot;
    public PlayerController playerController;
    public GameObject cam;
    public bool isintitan;

    //updating variables to real game objects and scripts
    void Start()
    {
        titan = GameObject.Find("titan pill");
        player = GameObject.Find("pill");
        pilotSpot = GameObject.Find("pilotPos");
        playerController = GameObject.Find("playerCam").GetComponent<PlayerController>();
        cam = GameObject.Find("playerCam");
        if(object.ReferenceEquals(playerController, null))
        {
            Debug.Log("no player controller");
        }
        if (object.ReferenceEquals(titan, null))
        {
            Debug.Log("no titan");
        }
    }

    //checks if player interacts with titan and puts player in titan
    void Update()
    {
        if (GameObject.ReferenceEquals(playerController.getLookingAt(), titan) && Input.GetKeyDown("e"))
        {
            player.SetActive(false);
            titan.GetComponent<TitanMovement>().enabled = true;
            player.transform.position = pilotSpot.transform.position;
            titan.GetComponent<TitanMovement>().playerCam = cam.transform;

            isintitan = true;
        }
    }
}
