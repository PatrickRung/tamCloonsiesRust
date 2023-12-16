using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TomyGunScript : MonoBehaviour
{
    public GameObject player, bullet;
    public int bulletCount, maxBulletCount;
    float gunShootInterval, gunFireRate;
    public Text ammoCount;

    // Start is called before the first frame update
    void Start()
    {
        gunFireRate = 0.2f;
        bulletCount = maxBulletCount;
    }

    // Update is called once per frame
    GameObject firedBullet;
    void Update()
    {
        gunShootInterval += Time.deltaTime;
        if (Input.GetMouseButton(0) && bulletCount > 0 && gunShootInterval > gunFireRate) {
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
    }
    void reload()
    {
        bulletCount = maxBulletCount;
        ammoCount.text = bulletCount + "";
    }
}
