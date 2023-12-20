using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public abstract class WeaponTemplate : MonoBehaviour
{
    public abstract string getDamageInfo();
    public abstract string getWeaponInfo();
    public abstract bool isGun();
}
