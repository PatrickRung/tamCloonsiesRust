using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TomyGunScript : GunTemplate
{
    public GameObject player, bullet;
    public int bulletCount, maxBulletCount;
    public float gunShootInterval, gunFireRate;
    public Text ammoCount;

    private movement movementScript;
    private string prefabName = "TommyGun";

    // Start is called before the first frame update
    void Start()
    {
        gunFireRate = 0.2f;
        bulletCount = maxBulletCount;
    }

    private void Awake()
    {
        recoilState = false;
        movementScript = GameObject.Find("pill").GetComponent<movement>();
        player = GameObject.Find("playerCam");
        ammoCount = GameObject.Find("World Items").GetComponent<WorldItemStorage>().
            ammoCount.GetComponent<Text>();
        ammoCount.text = "" + bulletCount;
    }

    // Update is called once per frame
    GameObject firedBullet;
    private bool recoilState;
    private float timeInRecoil;
    private int shotsFiredInRecoil;
    void Update()
    {
        gunShootInterval += Time.deltaTime;
        if (Input.GetMouseButton(0) && bulletCount > 0 && gunShootInterval > gunFireRate) {
            shotsFiredInRecoil++;
            recoilState = true;
            gunShootInterval = 0;
            bulletCount--;
            firedBullet = Instantiate(bullet, 
                        gameObject.transform.parent.transform.position + (gameObject.transform.parent.transform.forward * 2), 
                        transform.rotation);
            firedBullet.transform.rotation = player.transform.rotation;
            firedBullet.GetComponent<Rigidbody>().AddForce(player.transform.forward * 3000f);
            ammoCount.text = bulletCount + "";
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Invoke("reload", 2f);
        }
        if(recoilState)
        {
            applyRecoil();
            if (movementScript.addedRecoil > 0.5)
            {
                movementScript.addedRecoil = 0;
                recoilState = false;
            }
        }
        else
        {
            shotsFiredInRecoil = 0;
            timeInRecoil = 0;
        }
    }
    public void applyRecoil()
    {
        timeInRecoil += Time.deltaTime * 8;
        movementScript.addedRecoil = Mathf.Exp(timeInRecoil) - 0.5f - shotsFiredInRecoil;
    }
    public override int getBulletCount()
    {
        return bulletCount;
    }
    public override bool isGun()
    {
        return true;
    }

    //reloads gun lol
    void reload()
    {
        bulletCount = maxBulletCount;
        ammoCount.text = bulletCount + "";
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
