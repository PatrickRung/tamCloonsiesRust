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

    // Start is called before the first frame update
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

    // Update is called once per frame
    void Update()
    {
        if (GameObject.ReferenceEquals(playerController.getLookingAt(), titan) && GameObject.ReferenceEquals(playerController.getLookingAt().name, titan.name) && Input.GetKeyDown("e"))
        {
            player.SetActive(false);
            titan.GetComponent<TitanMovement>().enabled = true;
            player.transform.position = pilotSpot.transform.position;
            titan.GetComponent<TitanMovement>().playerCam = cam.transform;
            titan.GetComponent<TitanMovement>().orientation= titan.transform;

            isintitan = true;
        }
    }
}
