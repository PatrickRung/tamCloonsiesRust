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

    private GameObject[] playerInventory;
    private GameObject playerHealthBar;
    public GameObject titan;
    private int barLookingAt;
    private GameObject worldItems;
    public Material onMat, offMatt;

    public void Start()
    {
        playerHealthBar = GameObject.Find("PlayerHealthBar");
        titan = GameObject.Find("titan pill");
        onMat = Resources.Load<Material>("green");
        offMatt = Resources.Load<Material>("default");
  
        worldItems = GameObject.Find("World Items");
        barLookingAt = 0;
        playerInventory = new GameObject[3];
        playerInventory[1] = tomyGun;
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

    public bool addToInventory(GameObject weapon)
    {
        if (object.ReferenceEquals(playerInventory[barLookingAt], fistOfFury))
        {
            playerInventory[barLookingAt] = Instantiate(weapon, transform);
            setWeaponActive(barLookingAt);
            updateHotBar();
            return true;
        }
        return false;
    }

    private void updateHotBar()
    {

        item1.text = playerInventory[0].name;
        item2.text = playerInventory[1].name;
        item3.text = playerInventory[2].name;
    }

    //this is actually the camera controller
    //updates camera rotation and position according to movement
    void Update()
    {
        transform.position = player.transform.position;
        if (GameObject.Find("titan pill").GetComponent<TitanSwitchHandler>().isintitan)
        {
            transform.position = titan.transform.position;
            Debug.Log("balls");
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
            Destroy(playerInventory[barLookingAt].GetComponent<TomyGunScript>());
            playerInventory[barLookingAt].transform.SetParent(worldItems.transform);
            playerInventory[barLookingAt].AddComponent<BoxCollider>();
            ItemOnGround itemInfo = playerInventory[barLookingAt].AddComponent(typeof(ItemOnGround)) as ItemOnGround;
            //just a whole lot of setting values so not too important
            itemInfo.onHoverMat = onMat; itemInfo.offHoverMat = offMatt; 
            itemInfo.itemWhenPickedUp = Resources.Load<GameObject>("Prefabs/Tommy_gun_2");
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
        }
        else
        {
            ammoCount.SetActive(false);
        }
    }
}