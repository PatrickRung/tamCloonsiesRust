    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class CharacterTemplate : MonoBehaviour
{
    public float health;
    public float defaulthealth;
    public float maxhealth = 100;
    public Image healthbar;
    public void Awake()
    {
        health = defaulthealth;
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
            gameObject.transform.position = new Vector3(0,2,0);
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
