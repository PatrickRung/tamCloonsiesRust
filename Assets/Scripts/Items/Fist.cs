using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : MonoBehaviour
{
    public float hitRange;
    public bool readyToHit;
    public float punchCooldown = 5.0f;
    // Update is called once per frame
    void Update()
    {
        Debug.Log(readyToHit);
        if (Input.GetMouseButtonDown(0) && readyToHit)
        {
            punch();
        }
    }

    void punch()
    {
        
        readyToHit = false;
        RaycastHit hit;
        if (Physics.Raycast(transform.parent.position, transform.TransformDirection(Vector3.forward), out hit, hitRange))
        {
            Debug.Log(hit.transform.gameObject.name);
            Debug.Log(hit.transform.gameObject.GetComponent<EnemyAi>());
            if (!object.ReferenceEquals(hit.transform.gameObject.GetComponent<EnemyAi>(), null))
            {
                Debug.Log("punch thrown");
                Vector3 direction = hit.transform.position - transform.position;
                GameObject.Find("pill").GetComponent<Rigidbody>().AddForce(1000 * (direction.normalized));
                hit.transform.gameObject.GetComponent<EnemyAi>().changeHealth(-20);
                GameObject.Find("pill").GetComponent<Rigidbody>().AddForce(1000 * (-direction.normalized));
                hit.transform.gameObject.GetComponent<Rigidbody>().AddForce(1000 * (direction.normalized));
            }
        }
        Invoke("ResetPunch", 1.0f);
    }

    void ResetPunch()
    {
        readyToHit = true;
    }
}
