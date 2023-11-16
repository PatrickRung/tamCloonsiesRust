using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterTemplate : MonoBehaviour
{
    public float health = 0;
    public float defaulthealth = 0;
    public float maxhealth = 100;
    public Image healthbar;
    private void Awake()
    {
        health = defaulthealth;
    }

    public void changeHealth(float value, GameObject character)
    {
        this.health += value;
        UpdateHealth(health / maxhealth);
        if (health <= 0 && character.GetComponent<movement>() != null)
        {
            character.transform.position = new Vector3(0,2,0);
            health = 100;
        }
        else if(health <= 0 && character.GetComponent<movement>() == null)
        {
            GameObject.Destroy(character);
        }

    }

    public void UpdateHealth(float fraction)
    {
        healthbar.fillAmount = fraction;
    }
}
