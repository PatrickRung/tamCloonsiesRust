using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private GameObject[] playerInventory;

    public Transform player;
    public FlagsAttribute maxLookLength;
    public GameObject tomyGun, fistOfFury;
    public Text item1, item2, item3;

    public void Start()
    {
        playerInventory = new GameObject[3];
        playerInventory[1] = tomyGun;
        for (int i = 0; i < playerInventory.Length; i++) {
            if(object.ReferenceEquals(playerInventory[i], null))
            {
                playerInventory[i] = fistOfFury;
                item1.text = playerInventory[i].name;
            }
        }
    }

    //this is actually the camera controller
    //updates camera roation and position according to movement
    void Update()
    {
        transform.position = player.transform.position;
        swapWeapon();
    }

    public GameObject LookingAt;
    public GameObject getLookingAt()
    {
        return LookingAt;
    }
    private void FixedUpdate()
    {
        //checks what the user is looking at
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        {
            if(!object.ReferenceEquals(hit.transform, null))
            {
                LookingAt = hit.transform.gameObject;
            }

        }
        else
        {
            LookingAt = null;
        }
    }

    //makes a weapon disapear when other weapon selected
    public void swapWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            foreach (GameObject weapon in playerInventory)
            {
                weapon.SetActive(false);
            }
            playerInventory[0].SetActive(true);
            item1.text = playerInventory[0].name;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            foreach (GameObject weapon in playerInventory)
            {
                weapon.SetActive(false);
            }
            playerInventory[1].SetActive(true);
            item2.text = playerInventory[1].name;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            foreach (GameObject weapon in playerInventory)
            {
                weapon.SetActive(false);
            }
            playerInventory[2].SetActive(true);
            item3.text = playerInventory[2].name;
        }
    }
}