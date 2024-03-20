using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : MonoBehaviour
{
    public float hitRange;
    public bool readyToHit;
    // dunno why this is necessary but it absolutely is
    public float punchCooldown = 5.0f;
    // Update is called once per frame

    private void Start()
    {
        readyToHit = true;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && readyToHit)
        {
            StartCoroutine(punch());
        }
    }

    IEnumerator punch()
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
                //collecting the vector for player to enemy
                Vector3 direction = hit.transform.position - transform.position;
                //if grounded subtract y to prevent flying
                if (GameObject.Find("pill").GetComponent<movement>().grounded)
                {
                    direction.y = 0;
                }
                GameObject.Find("pill").GetComponent<Rigidbody>().AddForce(1000 * (direction.normalized));
                //super cool time delay thing to put forces away from enemy and character
                yield return new WaitForSeconds(.3f);
                hit.transform.gameObject.GetComponent<EnemyAi>().changeHealth(-20);
                GameObject.Find("pill").GetComponent<Rigidbody>().AddForce(1000 * (-direction.normalized));
                //currently not working right :(
                hit.transform.gameObject.GetComponent<Rigidbody>().AddForce(1000 * (direction.normalized));
            }
        }
        //cooldown stuff
        Invoke("ResetPunch", 1.0f);
    }
    //just resets the variable for cooldown
    void ResetPunch()
    {
        readyToHit = true;
    }


}
