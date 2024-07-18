using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class GunTemplate : WeaponTemplate
{
    public GameObject player, bullet;
    public int bulletCount, maxBulletCount;
    public float gunShootInterval, gunFireRate;
    public Text ammoCount;

    public movement movementScript;
    public abstract int getBulletCount();

    void Start()
    {
        bulletCount = maxBulletCount;
    }
    private void Awake()
    {
        totalRecoilAdded = 0; totalRecoilReturned = 0;
        timeInRecoil = -recoilAmount;
        recoilState = false;
        player = GameObject.Find("playerCam");
        ammoCount = GameObject.Find("World Items").GetComponent<WorldItemStorage>().
            ammoCount.GetComponent<Text>();
        ammoCount.text = "" + bulletCount;
    }
    // Update is called once per frame
    protected GameObject firedBullet;
    public bool recoilState;
    protected float timeInRecoil, totalRecoilAdded, totalRecoilReturned;
    public int shotsFiredInRecoil;
    public int getShotsFiredInRecoil() { return shotsFiredInRecoil; }
    public int getShotsFiredInRecoil(int newNum) { shotsFiredInRecoil = newNum;  return shotsFiredInRecoil; }
    public void Update()
    {
        shootingGun();

        //reloading
        if (Input.GetKeyDown(KeyCode.R))
        {
            Invoke("reload", 1f);
        }

        //recoil stuff
        recoilManagement();
    }

    public virtual void shootingGun()
    {
        gunShootInterval += Time.deltaTime;
        if (Input.GetMouseButton(0) && bulletCount > 0 && gunShootInterval > gunFireRate)
        {
            CancelInvoke();
            shotsFiredInRecoil++;
            recoilState = true;
            gunShootInterval = 0;
            timeInRecoil = -recoilAmount;
            bulletCount--;
            firedBullet = Instantiate(bullet,
                        gameObject.transform.parent.transform.position + (gameObject.transform.parent.transform.forward * 2),
                        transform.rotation);
            firedBullet.transform.rotation = player.transform.rotation;
            firedBullet.GetComponent<Rigidbody>().AddForce(player.transform.forward * 3000f);
            ammoCount.text = bulletCount + "";
        }
    }

    public void recoilManagement()
    {
        if (recoilState)
        {
            timeInRecoil += Time.deltaTime * 8;
            if(movementScript.addedRecoil < 0)
            {
                movementScript.addedRecoil = Mathf.Pow(timeInRecoil, 3);
            }
            else
            {
                movementScript.addedRecoil = Mathf.Pow(timeInRecoil, 5);
            }
            if (movementScript.addedRecoil < 0)
            {
                totalRecoilAdded += movementScript.addedRecoil;
            }
            else
            {
                totalRecoilReturned += movementScript.addedRecoil;
            }
            if (-totalRecoilAdded <= totalRecoilReturned)
            {
                totalRecoilAdded = 0; totalRecoilReturned = 0;
                movementScript.addedRecoil = 0;
                recoilState = false;
            }
        }
        else
        {
            shotsFiredInRecoil = 0;
            timeInRecoil = -recoilAmount;
        }
    }
    public float recoilAmount = 1;

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
}
