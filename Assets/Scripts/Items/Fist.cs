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
            if (Physics.Raycast(transform.parent.position, transform.TransformDirection(Vector3.forward), out hit, hitRange))
            {
                Debug.Log(hit.transform.gameObject.name);
                Debug.Log(hit.transform.gameObject.GetComponent<EnemyAi>());
                if (!object.ReferenceEquals(hit.transform.gameObject.GetComponent<EnemyAi>(), null))
                {
                    Debug.Log("punch thrown");
                    hit.transform.gameObject.GetComponent<EnemyAi>().changeHealth(-20);
                }
            }
        }
    }
}
