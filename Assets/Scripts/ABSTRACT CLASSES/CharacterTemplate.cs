    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public abstract class CharacterTemplate : NetworkBehaviour
{
    [Header("Health")]
    public float health;
    public float maxhealth = 100;
    public Image healthbar;
    public Transform spawnPoint;
    public void Awake()
    {
        health = maxhealth;
        if(gameObject.layer == 7 && !(SceneManager.GetActiveScene().name == "menuScene")) {
            healthbar = GameObject.Find("healthBar (1)").GetComponent<Image>();
        }
    }
    public void FixedUpdate()
    {
        UpdateHealth(health/maxhealth);
    }

    public void changeHealth(float value)
    {
        this.health += value;
        if (health <= 0 && gameObject.GetComponent<movement>() != null)
        {
            if(spawnPoint != null)
            {
                gameObject.transform.position = spawnPoint.position;
            }
            else
            {
                gameObject.transform.position = new Vector3(0, 40, 0);
            }
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            health = maxhealth;
        }
        else if(health <= 0 && gameObject.GetComponent<movement>() == null)
        {
            GameObject.Destroy(gameObject);
        }

    }

    public void UpdateHealth(float fraction)
    {
        healthbar.fillAmount = fraction;
    }
}
