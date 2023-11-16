using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TomyGunScript : MonoBehaviour
{
    public GameObject player, bullet;
    public int bulletCount, maxBulletCount;
    public Text ammoCount;
    // Start is called before the first frame update
    void Start()
    {
        bulletCount = maxBulletCount;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && bulletCount > 0) {
            bulletCount--;
            Instantiate(bullet, 
                        gameObject.transform.parent.transform.position + (gameObject.transform.parent.transform.forward * 2), 
                        transform.rotation);
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
