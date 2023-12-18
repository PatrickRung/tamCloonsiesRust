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
    public string name;
    public GameObject itemWhenPickedUp;


    public void FixedUpdate()
    {
        if(base.returnLookingAtStatus())
        {
            gameDesc.SetActive(true);
            gameDesc.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
            gameDesc.transform.GetChild(1).GetComponent<TextMeshProUGUI>().
                text = itemWhenPickedUp.GetComponent<WeaponTemplate>().getDamageInfo();
            gameDesc.transform.GetChild(2).GetComponent<TextMeshProUGUI>().
                text = itemWhenPickedUp.GetComponent<WeaponTemplate>().getWeaponInfo();
        }
        else
        {
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
