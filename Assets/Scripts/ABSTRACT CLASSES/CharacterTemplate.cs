    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public abstract class CharacterTemplate : NetworkBehaviour
{
    [Header("Health")]
    public NetworkVariable<int> health = new NetworkVariable<int>(0);
    public int maxhealth = 100;
    public Image healthbar;
    public Transform spawnPoint;
    public GameObject worldStorage;
    public void Awake()
    {
        changeHealthRPC(maxhealth);
        if(gameObject.layer == 7 && !(SceneManager.GetActiveScene().name == "menuScene")) {
            healthbar = GameObject.Find("healthBar (1)").GetComponent<Image>();
        }
    }
    public void FixedUpdate()
    {
        UpdateHealth((float)health.Value/maxhealth);
    }

    public void changeHealth(int value)
    {
        health.Value += value;
        if (health.Value <= 0 && gameObject.TryGetComponent<movement>(out movement PlayerMovement))
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
            health.Value = maxhealth;
        }
        else if(health.Value <= 0 && gameObject.GetComponent<movement>() == null)
        {
            GameObject.Destroy(gameObject);
        }

    }
    [Rpc(SendTo.Server)]
    public void changeHealthRPC(int value)
    {
        health.Value += value;
        if (health.Value <= 0 && gameObject.TryGetComponent<movement>(out movement PlayerMovement))
        {
            Debug.Log("Player has died");
            PlayerMovement.PlayerDiedRPC(PlayerMovement.OwnerClientId);
            if(spawnPoint != null)
            {
                gameObject.transform.position = spawnPoint.position;
            }
            else
            {
                gameObject.transform.position = new Vector3(0, 40, 0);
            }
            gameObject.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            health.Value = maxhealth;
        }
        else if(health.Value <= 0 && gameObject.GetComponent<movement>() == null)
        {
            GameObject.Destroy(gameObject);
        }
    }


    public void UpdateHealth(float fraction)
    {
        if(!IsOwner) return;
        healthbar.fillAmount = fraction;
    }
}
