//handles what the player is looking at
//handles inventory and starting weapons

using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Transform player;
    public FlagsAttribute maxLookLength;
    public GameObject tomyGun, fistOfFury, ammoCount;
    public Text item1, item2, item3;
    public GameObject UI;
    public GameObject smallHealthPot;

    private GameObject[] playerInventory;
    private GameObject playerHealthBar, worldItems, weaponSpot;
    private int barLookingAt;
    private Material onMat, offMatt;
    private movement playerMovement;

    //sets all the items in the inventory to be either a fist, health pot, or gun
    //then updates hot bar, UI text and asigns game objects from world
    public void Start()
    {
        playerMovement = player.gameObject.GetComponent<movement>();
        playerHealthBar = GameObject.Find("PlayerHealthBar");
        onMat = Resources.Load<Material>("green");
        offMatt = Resources.Load<Material>("default");
        weaponSpot = GameObject.Find("WeaponSpot");
        worldItems = GameObject.Find("World Items");
        barLookingAt = 0;
        playerInventory = new GameObject[3];
        playerInventory[1] = tomyGun;
        playerInventory[2] = smallHealthPot;
        for (int i = 0; i < playerInventory.Length; i++) {
            if(object.ReferenceEquals(playerInventory[i], null))
            {
                playerInventory[i] = fistOfFury;
            }
        }
        setWeaponActive(0);
        item1.text = playerInventory[0].name;
        updateHotBar();
    }
    public GameObject itemInHand()
    {
        return playerInventory[barLookingAt];
    }
    //if there is a fist then it will add the item
    //if you input null into parameter then it ill just asign the fist to that spot
    //only returns true if the item assigned is not a fist
    public bool addToInventory(GameObject weapon)
    {
        if (object.ReferenceEquals(playerInventory[barLookingAt], fistOfFury))
        {
            playerInventory[barLookingAt] = Instantiate(weapon, transform);
            playerInventory[barLookingAt].transform.position = weaponSpot.transform.position;
            setWeaponActive(barLookingAt);
            updateHotBar();
            return true;
        }
        else if(object.ReferenceEquals(playerInventory[barLookingAt], null)
            || object.ReferenceEquals(weapon, null))
        {
            playerInventory[barLookingAt] = fistOfFury;
            updateHotBar();
        }
        return false;
    }

    private void updateHotBar()
    {
        item1.text = playerInventory[0].name;
        playerInventory[0].transform.position = weaponSpot.transform.position;
        item2.text = playerInventory[1].name;
        playerInventory[1].transform.position = weaponSpot.transform.position;
        item3.text = playerInventory[2].name;
        playerInventory[2].transform.position = weaponSpot.transform.position;

    }

    //this is actually the camera controller
    //updates camera roation and position according to movement
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(!playerMovement.inMenu)
            {
                for (int i = 0; i < UI.transform.childCount; i++)
                {
                    UI.transform.GetChild(i).gameObject.SetActive(false);
                    playerMovement.inMenu = true;
                }
            }
            else
            {
                playerMovement.inMenu = false;
                for (int i = 0; i < UI.transform.childCount; i++)
                {
                    if(UI.transform.GetChild(i).gameObject.active)
                    {
                        UI.transform.GetChild(i).gameObject.SetActive(false);
                    }
                    else
                    {
                        UI.transform.GetChild(i).gameObject.SetActive(true);
                    }
                }
                playerMovement.inMenu = false;
            }
        }
        transform.position = player.transform.position;
        if (GameObject.Find("titan pill").GetComponent<TitanSwitchHandler>().isintitan)
        {
            transform.position = GameObject.Find("titan pill").transform.position;
        }
        swapWeapon();
        dropWeapon();
    }

    //returns what the player is looking at to other classes
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
    public void dropWeapon()
    {
        if(Input.GetKeyDown(KeyCode.G) && !playerInventory[barLookingAt].Equals(fistOfFury))
        {   
            
            playerInventory[barLookingAt].transform.SetParent(worldItems.transform);
            playerInventory[barLookingAt].AddComponent<BoxCollider>();
            ItemOnGround itemInfo = playerInventory[barLookingAt].AddComponent(typeof(ItemOnGround)) as ItemOnGround;
            //just a whole lot of setting values so not too important
            itemInfo.onHoverMat = onMat; itemInfo.offHoverMat = offMatt; 
            itemInfo.itemWhenPickedUp = Resources.Load<GameObject>("Prefabs/" + playerInventory[barLookingAt].GetComponent<WeaponTemplate>().nameOfPrefab());
            playerInventory[barLookingAt] = fistOfFury;
            Destroy(playerInventory[barLookingAt].GetComponent<WeaponTemplate>());
            updateHotBar();
        }
    }

    //makes a weapon disapear when other weapon selected
    private void swapWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            setWeaponActive(0);
            item1.text = playerInventory[0].name;
            barLookingAt = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            setWeaponActive(1);
            item2.text = playerInventory[1].name;
            barLookingAt = 1;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            setWeaponActive(2);
            item3.text = playerInventory[2].name;
            barLookingAt = 2;
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
        if (!object.ReferenceEquals(playerInventory[spot], fistOfFury) && playerInventory[spot].GetComponent<WeaponTemplate>().isGun())
        {
            ammoCount.SetActive(true);
            ammoCount.GetComponent<Text>().text = "" + playerInventory[spot].GetComponent<GunTemplate>().getBulletCount();
        }
        else
        {
            ammoCount.SetActive(false);
        }
    }
}