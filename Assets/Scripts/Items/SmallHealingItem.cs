using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallHealingItem : WeaponTemplate
{
    private PlayerController controller;
    private movement playerMovementScript;
    public int healingAmount = 20;
    private string prefabName = "smallHealthItem";
    void Start()
    {
        controller = GameObject.Find("playerCam").GetComponent<PlayerController>();
        playerMovementScript = GameObject.Find("pill").GetComponent<movement>();
    }


    void Update()
    {
        if(object.ReferenceEquals(controller.itemInHand(), gameObject) && Input.GetMouseButtonDown(0))
        {
            playerMovementScript.changeHealth(healingAmount);
            Destroy(gameObject);
            controller.addToInventory(null);
        }
    }
    public override bool isGun()
    {
        return false;
    }

    public override string getDamageInfo()
    {
        return "heals u dummy";
    }

    public override string getWeaponInfo()
    {
        return "heals u dummy";
    }

    public override string nameOfPrefab()
    {
        return prefabName;
    }
}
