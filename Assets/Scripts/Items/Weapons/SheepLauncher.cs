using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepLauncher : GunTemplate
{
    [HideInInspector] public GameObject projectile;
    public EntitySpawnHandler spawnHandler;
    new void Awake() {
        base.Awake();
        spawnHandler = worldItems.GetComponent<WorldItemStorage>().entitySpawnHandling.GetComponent<EntitySpawnHandler>();
    }
    public override string getDamageInfo()
    {
        return "how do they fit a sheep in there??";
    }
    public override string getWeaponInfo()
    {
        return "yep this thing shoots sheep";
    }
    public override bool isGun()
    {
        return true;
    }
    public override string nameOfPrefab()
    {
        return "SheepLauncher";
    }

    public override int getBulletCount()
    {
        return 5;
    }
    public override async void shootingGun()
    {
        gunShootInterval += Time.deltaTime;
        if (Input.GetMouseButton(0) && base.bulletCount > 0 && base.gunShootInterval > base.gunFireRate)
        {
            CancelInvoke();
            base.shotsFiredInRecoil++;
            recoilState = true;
            base.gunShootInterval = 0;
            base.timeInRecoil = -base.recoilAmount;
            base.bulletCount--;
            if(worldItems.GetComponent<WorldItemStorage>().multiplayerEnabled) {
                Debug.Log("tying to spawn proj");
                spawnHandler.spawnProjectileRPC(bullet.name, 
                    gameObject.transform.parent.transform.position + (gameObject.transform.parent.transform.forward * 2),
                    transform.rotation,
                    player.transform.forward * 30000f);
                
            }
            else {
                firedBullet = Instantiate(bullet,
                    gameObject.transform.parent.transform.position + (gameObject.transform.parent.transform.forward * 2),
                    transform.rotation);
                firedBullet.GetComponent<Rigidbody>().useGravity = false;
                firedBullet.GetComponent<Rigidbody>().AddForce(player.transform.forward * 30000f);


            }
            base.ammoCount.text = base.bulletCount + "";
        }
    }
}
