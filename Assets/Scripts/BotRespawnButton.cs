using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotRespawnButton : Button
{

    public GameObject enemy;
    public Transform botSpawnPoint;
    //the rest of the code is in the button class
    public override void buttonInteraction()
    {
        GameObject currentEnemy = GameObject.Find("enemy");
        Debug.Log(currentEnemy);
        if (Object.ReferenceEquals(currentEnemy, null))
        {
            Debug.Log("test");
            currentEnemy = Instantiate(enemy, botSpawnPoint);
            currentEnemy.name = "enemy";
        }
    }
}
