using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fist : MonoBehaviour
{
    public float hitRange;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, hitRange))
            {
                if(!object.ReferenceEquals(hit.transform.gameObject.GetComponent<EnemyAi>(), null))
                {
                    hit.transform.gameObject.GetComponent<EnemyAi>().changeHealth(-20, hit.transform.gameObject);
                }
            }
        }
    }
}
