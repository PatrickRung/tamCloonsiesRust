using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncherScript : GunTemplate
{
    [HideInInspector] public GameObject projectile;
    public override string getDamageInfo()
    {
        return "rocket launch thing";
    }
    public override string getWeaponInfo()
    {
        return "yep this thing shoots rockets";
    }
    public override bool isGun()
    {
        return true;
    }
    public override string nameOfPrefab()
    {
        return "RocketLauncher";
    }

    public override int getBulletCount()
    {
        return 10;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        base.Update();

    }

    public override void shootingGun()
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

            }
            else {
                firedBullet = Instantiate(bullet,
                            gameObject.transform.parent.transform.position + (gameObject.transform.parent.transform.forward * 2),
                            transform.rotation);
                firedBullet.transform.rotation = base.player.transform.rotation;
                firedBullet.GetComponent<Rigidbody>().AddForce(player.transform.forward * 30000f);
                firedBullet.GetComponent<Rigidbody>().useGravity = false;
                base.ammoCount.text = base.bulletCount + "";
            }
        }
    }

    
}
