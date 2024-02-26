using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SniperScript : hitScan
{
    

    private string prefabName = "sniper";

   
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
        return "360 no scope";
    }
    public override string nameOfPrefab()
    {
        return prefabName;
    }

   
}
