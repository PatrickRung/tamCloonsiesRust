using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolScript : GunTemplate
{
    private string prefabName = "ancientPistol";

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
        return "people still use bullets?";
    }
    public override string nameOfPrefab()
    {
        return prefabName;
    }
}
