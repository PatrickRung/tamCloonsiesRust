using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TomyGunScript : GunTemplate
{
    private string prefabName = "TommyGun";

    public override int getBulletCount()
    {
        return bulletCount;
    }
    public override string getDamageInfo()
    {
        return bullet.GetComponent<BulletScript>().damage + " damage";
    }

    public override string getWeaponInfo()
    {
        return "A weapon for those who like beboy cowbob";
    }
    public override string nameOfPrefab()
    {
        return prefabName;
    }
}
