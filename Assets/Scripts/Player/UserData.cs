using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UserData", menuName = "ScriptableObjects/UserDataStorage", order = 1)]
public class UserData : ScriptableObject
{
    public float playerSensitivity = 50f;
    public bool isHost;
    public string code;

    
    public UserData(float playerSensitivity)
    {
        if(playerSensitivity != 0)
        {
            this.playerSensitivity = playerSensitivity;
        }
        else
        {
            this.playerSensitivity = 50f;
        }
    }
    
}
