using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public abstract class hitScan : GunTemplate
{
    public GameObject cam;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;
    public int damage;
    public int range;
    public bool readyToShoot = true;
    private GameObject playerBody;

    private void Awake()
    {
        //needed to add this cam thing for raycast later
        cam = GameObject.Find("playerCam");
        totalRecoilAdded = 0;
        totalRecoilReturned = 0;
        timeInRecoil = -recoilAmount;
        recoilState = false;
        playerBody = GameObject.Find("pill");
        movementScript = playerBody.GetComponent<movement>();
        player = GameObject.Find("playerCam");
        ammoCount = GameObject.Find("World Items").GetComponent<WorldItemStorage>().
            ammoCount.GetComponent<Text>();
        ammoCount.text = "" + bulletCount;
    }

    public override void shootingGun()
    {
        gunShootInterval += Time.deltaTime;
        //checks bullet count and keystroke
        if (Input.GetKeyDown(KeyCode.Mouse0) && bulletCount > 0 && gunShootInterval > gunFireRate)
        {
            CancelInvoke();
            shotsFiredInRecoil++;
            recoilState = true;
            gunShootInterval = 0;
            //does raycast checks if it hit the layer the enemy is on

            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out rayHit, range))
            {
                Debug.Log("hit something");
                // checks if the thing on the layer has enemy tag
                if (rayHit.collider.GetComponent<EnemyAi>() != null)
                {
                    rayHit.collider.GetComponent<EnemyAi>().changeHealth(-damage);
                }
                else if (rayHit.collider.GetComponent<concussionMine>() != null)
                {
                    rayHit.collider.GetComponent<concussionMine>().explosion(playerBody);
                }
            }
            bulletCount--;
            ammoCount.text = bulletCount + "";
            Invoke("resetShot", gunShootInterval);
        }
    }
    public void resetShot()
    {
        readyToShoot = true;
    }
}
