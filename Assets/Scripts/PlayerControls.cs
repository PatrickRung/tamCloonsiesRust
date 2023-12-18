//handles what the player is looking at
//handles inventory and starting weapons

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
            }
            updateHotBar();
        }
        setWeaponActive(0);
        item1.text = playerInventory[0].name;
    }

    private void updateHotBar()
    {
        item1.text = playerInventory[0].name;
        item2.text = playerInventory[1].name;
        item3.text = playerInventory[2].name;
    }

    //this is actually the camera controller
    //updates camera roation and position according to movement
    void Update()
    {
        transform.position = player.transform.position;
        swapWeapon();
    }

    //returns what the player is looking at to other classes
    private GameObject LookingAt;
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
    private void swapWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            setWeaponActive(0);
            item1.text = playerInventory[0].name;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            setWeaponActive(1);
            item2.text = playerInventory[1].name;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            setWeaponActive(2);
            item3.text = playerInventory[2].name;
        }
    }

    //sets a weapon to be active depending on which spot 
    private void setWeaponActive(int spot)
    {
        foreach (GameObject weapon in playerInventory)
        {
            weapon.SetActive(false);
        }
        playerInventory[spot].SetActive(true);

    }
}