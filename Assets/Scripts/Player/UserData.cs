using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UserData", menuName = "ScriptableObjects/UserDataStorage", order = 1)]
public class UserData : ScriptableObject
{
    public float playerSensitivity;

    
    public UserData(float playerSensitivity)
    {
        this.playerSensitivity = playerSensitivity;
    }
    
}
