using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class NPCScript : Button
{
    GameObject WorldStorage, ItemDesc;
    private string currentDislayMessage;
    private int interactionIteration;
    private void Awake()
    {
        interactionIteration = 0;
        WorldStorage = GameObject.Find("World Items");
        ItemDesc = WorldStorage.GetComponent<WorldItemStorage>().itemDesc;
        currentDislayMessage = "What is up pookie bear!";
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
                text = currentDislayMessage;
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
        string tempMessage = "";
        switch (interactionIteration)
        {
            case 0:
                tempMessage = "I am Sebastion";
                break;
            case 1:
                tempMessage = "I love Fortnite";
                break;
            case 2:
                tempMessage = "Left click to shoot gun!";
                break;
            case 3:
                tempMessage = "Fortnite";
                break;
            default:
                tempMessage = "Sebastion says that the code is ass because there should be no way iteration becomes negative";
                break;
        }
        slowlyTypeOutCharacters(tempMessage);
        interactionIteration++;
    }

    public async void slowlyTypeOutCharacters(string input)
    {
        currentDislayMessage = "";
        for (int i = 0; i < input.Length; i++)
        {
            currentDislayMessage += input[i];
            await Task.Delay(50);

        }
    }
}
