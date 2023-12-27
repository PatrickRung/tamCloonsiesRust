using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShrapnelShotgun : GunTemplate
{
    private string prefabName = "ShrapnelShotgun";
    public float bulletSpread;
    public override void shootingGun()
    {
        gunShootInterval += Time.deltaTime;
        if (Input.GetMouseButton(0) && bulletCount > 0 && gunShootInterval > gunFireRate)
        {
            CancelInvoke();
            getShotsFiredInRecoil(getShotsFiredInRecoil() + 1);
            recoilState = true;
            gunShootInterval = 0;
            timeInRecoil = -recoilAmount;
            bulletCount--;

            for(int i = 0; i < 4; i++)
            {
                //and yes I am a glowing brain genius for figuring out this math even though it's not that complicated
                firedBullet = Instantiate(bullet,
                            gameObject.transform.parent.transform.position + (gameObject.transform.parent.transform.forward * 2),
                            transform.rotation);
                float angleShotFired = Mathf.Atan(transform.parent.transform.forward.z / transform.parent.transform.forward.x);
                switch (i)
                {
                    case 0:
                        firedBullet.transform.localPosition += (new Vector3(Mathf.Cos(angleShotFired - 90),
                            firedBullet.transform.localPosition.y,
                            Mathf.Sin(angleShotFired - 90))) * bulletSpread;
                        break;
                    case 1:
                        firedBullet.transform.localPosition += (new Vector3(Mathf.Cos(angleShotFired + 90),
                            firedBullet.transform.localPosition.y,
                            Mathf.Sin(angleShotFired + 90))) * bulletSpread;
                        break;
                    case 2:
                        firedBullet.transform.localPosition = new Vector3(firedBullet.transform.localPosition.x,
                            firedBullet.transform.localPosition.y + (1 * bulletSpread),
                            firedBullet.transform.localPosition.z);
                        break;
                    case 3:
                        firedBullet.transform.localPosition = new Vector3(firedBullet.transform.localPosition.x,
                            firedBullet.transform.localPosition.y - (1  * bulletSpread),
                            firedBullet.transform.localPosition.z);
                        break;
                }
                firedBullet.transform.rotation = player.transform.rotation;
                firedBullet.GetComponent<Rigidbody>().AddForce(player.transform.forward * 3000f);
            }
            ammoCount.text = bulletCount + "";
        }
    }

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
        return "A religious weapon commonly used by those of the ??? cult";
    }
    public override string nameOfPrefab()
    {
        return prefabName;
    }
}
