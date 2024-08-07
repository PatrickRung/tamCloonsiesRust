//handles what the player is looking at
//handles what the player is looking at
//handles inventory and starting weapons

using System;
using Unity.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PlayerController : NetworkBehaviour
{
    [Header("ALL NEED TO BE ASSIGNED")]
    public Transform player;
    public FlagsAttribute maxLookLength;
    public GameObject fistOfFury, ammoCount;
    public Text item1, item2, item3;
    public GameObject UI;
    public UserData userData;


    [HideInInspector] public movement playerMovement;
    private GameObject[] playerInventory;
    private GameObject playerHealthBar, worldItems, weaponSpot, LookingAt;
    private int barLookingAt;
    private Material onMat, offMatt;
    private GameObject MenuObject;
    private GameObject sensitivitySlider;
    private bool titanExistInLevel;
    public int PlayerNetCodeID;

    //sets all the items in the inventory to be either a fist, health pot, or gun
    //then updates hot bar, UI text and asigns game objects from world
    public void Awake()
    {
        //assigning stuff
        worldItems = GameObject.Find("World Items");
        playerHealthBar = GameObject.Find("PlayerHealthBar");
        onMat = Resources.Load<Material>("green");
        offMatt = Resources.Load<Material>("default");
        weaponSpot = GameObject.Find("WeaponSpot");
        UI = worldItems.GetComponent<WorldItemStorage>().PlayerCamera;
        MenuObject = worldItems.GetComponent<WorldItemStorage>().menu;
        sensitivitySlider = worldItems.GetComponent<WorldItemStorage>().sensitivitySlider;
        if(worldItems.GetComponent<WorldItemStorage>().multiplayerEnabled) {
            PlayerNetCodeID = (int)NetworkManager.LocalClientId;
        }

        //making the menu invisible
        for (int i = 0; i < MenuObject.transform.childCount; i++)
        {
            if(!MenuObject.transform.GetChild(i).gameObject.name.Equals("Objective")) {
                MenuObject.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        if(worldItems.GetComponent<WorldItemStorage>().itemDesc.activeSelf)
        {
            worldItems.GetComponent<WorldItemStorage>().itemDesc.SetActive(false);
        }


        //creating inventory
        barLookingAt = 0;
        playerInventory = new GameObject[3];
        for (int i = 0; i < playerInventory.Length; i++) {
            if(object.ReferenceEquals(playerInventory[i], null))
            {
                playerInventory[i] = fistOfFury;
            }
        }

        //getting UI and player ready for game
        setWeaponActive(0);
        item1.text = playerInventory[0].name;
        updateHotBar();

        //check if titan is in level
        if(object.ReferenceEquals(GameObject.Find("titan pill"), null)) {
            titanExistInLevel = false;
        } else
        {
            titanExistInLevel = true;
        }
    }

    public void DisableControls() {
        enabled = false;
    }
    //returns the item that the player is holding
    public GameObject itemInHand() { return playerInventory[barLookingAt]; }
    //returns what the player is looking at to other classes
    public GameObject getLookingAt() { return LookingAt; }

    //if there is a fist then it will add the item
    //if you input null into parameter then it ill just asign the fist to that spot
    //only returns true if the item assigned is not a fist
    int EntityID= 0;
    public bool addToInventory(GameObject weapon)
    {
        if (object.ReferenceEquals(playerInventory[barLookingAt], fistOfFury))
        {
            //instead of instantiating localy this if statement will send an RPC call to the server to spawn the weapon on the server
            playerInventory[barLookingAt] = Instantiate(weapon, transform);

            if (playerInventory[barLookingAt].TryGetComponent<GunTemplate>(out GunTemplate gunTemplate))
            {
                gunTemplate.movementScript = playerMovement;
            }
            playerInventory[barLookingAt].transform.position = weaponSpot.transform.position;
            setWeaponActive(barLookingAt);
            updateHotBar();
            //spawn on both server and client
            if(worldItems.GetComponent<WorldItemStorage>().multiplayerEnabled && IsOwner && (IsServer || IsHost)) {
                Debug.Log("spawning weapon on both server and client");
                playerInventory[barLookingAt].transform.parent = transform;
            }

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
    [Rpc(SendTo.ClientsAndHost)]
    public void addItemToClientRPC(FixedString512Bytes weaponName) {
        if(!IsOwner && IsServer) return;
        playerInventory[barLookingAt] = GameObject.Find(weaponName + "(Clone)");
        playerInventory[barLookingAt].name = weaponName + " " + EntityID;
        if (playerInventory[barLookingAt].TryGetComponent<GunTemplate>(out GunTemplate gunTemplate))
        {
            gunTemplate.movementScript = playerMovement;
        }
        playerInventory[barLookingAt].transform.position = weaponSpot.transform.position;
        setWeaponActive(barLookingAt);
        updateHotBar();
    }

    //updates the UI and the position of the weapon
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

        if(transform.Equals(null) || player.Equals(null)) return;
        if (Input.GetKeyDown(KeyCode.P))
        {
            if(!playerMovement.inMenu)
            {
                openUI("Menu");
            }
            else if (playerMovement.inMenu)
            {
                closeUI();
            }
        }
        transform.position = player.transform.position;
        swapWeapon();
        dropWeapon();
    }

    public void openUI(String UILabel) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            for (int i = 0; i < UI.transform.childCount; i++)
            {
                if(!UI.transform.GetChild(i).gameObject.name.Equals(UILabel) && !UI.transform.GetChild(i).gameObject.name.Equals("Objective"))
                {
                    UI.transform.GetChild(i).gameObject.SetActive(false);
                }
                else
                {
                    UI.transform.GetChild(i).gameObject.SetActive(true);
                }
            }
            for (int i = 0; i < MenuObject.transform.childCount; i++)
            {
                MenuObject.transform.GetChild(i).gameObject.SetActive(true);
            }
            playerMovement.inMenu = true;
            sensitivitySlider.GetComponent<Slider>().value = userData.playerSensitivity / 100f;
    }
    public void closeUI() {
        playerMovement.inMenu = false;
        for (int i = 0; i < UI.transform.childCount; i++)
        {
            if (UI.transform.GetChild(i).tag.Equals("MustCloseUI"))
            {
                UI.transform.GetChild(i).gameObject.SetActive(false);
            }
            else
            {
                UI.transform.GetChild(i).gameObject.SetActive(true);
            }
        }

        playerMovement.inMenu = false;
        playerMovement.setSensitivity(Mathf.Round(sensitivitySlider.GetComponent<Slider>().value * 100f));
        userData.playerSensitivity = Mathf.Round(sensitivitySlider.GetComponent<Slider>().value * 100f);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
            Destroy(playerInventory[barLookingAt].GetComponent<WeaponTemplate>());
            playerInventory[barLookingAt] = fistOfFury;
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