using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemOnGround : Button
{
    public GameObject gameDesc;
    public GameObject descName;
    public GameObject itemWhenPickedUp;


    private bool descRemoved;

    public void Awake()
    {
        gameDesc = GameObject.Find("World Items").GetComponent<WorldItemStorage>().itemDesc;
        descName = GameObject.Find("World Items").GetComponent<WorldItemStorage>().itemName;
    }

    public void FixedUpdate()
    {
        if(base.returnLookingAtStatus())
        {
            descRemoved = false;
            gameDesc.SetActive(true);
            gameDesc.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
            gameDesc.transform.GetChild(1).GetComponent<TextMeshProUGUI>().
                text = itemWhenPickedUp.GetComponent<WeaponTemplate>().getDamageInfo();
            gameDesc.transform.GetChild(2).GetComponent<TextMeshProUGUI>().
                text = itemWhenPickedUp.GetComponent<WeaponTemplate>().getWeaponInfo();
        }
        else if(!descRemoved)
        {
            descRemoved = true;
            gameDesc.SetActive(false);
        }
    }
    public override void buttonInteraction()
    {
        if(playerController.addToInventory(itemWhenPickedUp))
        {
            gameDesc.SetActive(false);
            GameObject.Destroy(gameObject);
        }

    }
}
