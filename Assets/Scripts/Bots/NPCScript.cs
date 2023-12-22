using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCScript : Button
{
    GameObject WorldStorage, ItemDesc;
    private void Awake()
    {
        WorldStorage = GameObject.Find("World Items");
        ItemDesc = WorldStorage.GetComponent<WorldItemStorage>().itemDesc;
    }
    private bool descRemoved;
    public void FixedUpdate()
    {
        if (base.returnLookingAtStatus())
        {
            descRemoved = false;
            ItemDesc.SetActive(true);
            ItemDesc.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
            ItemDesc.transform.GetChild(1).GetComponent<TextMeshProUGUI>().
                text = "What is up pookie bear!";
            ItemDesc.transform.GetChild(2).GetComponent<TextMeshProUGUI>().
                text = "";
        }
        else if (!descRemoved)
        {
            descRemoved = true;
            ItemDesc.SetActive(false);
        }
    }
    public override void buttonInteraction()
    {

    }
}
