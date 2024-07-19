using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Netcode;
using UnityEngine;

public abstract class WeaponTemplate : NetworkBehaviour
{
    public abstract string getDamageInfo();
    public abstract string getWeaponInfo();
    public abstract bool isGun();
    public abstract string nameOfPrefab();
}
