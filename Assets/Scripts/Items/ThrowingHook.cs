using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ThrowingHook : MonoBehaviour
{
    public GameObject hook;
    public float speed, length;
    private GameObject player;
    private float totalLengthAdded;
    private bool stopChain, chainGoingOutward, playerGrabbed;
    void Awake()
    {
        chainGoingOutward = true;
        playerGrabbed = false;
        player = GameObject.Find("pill");
    }

    //throws the hook, brings it back and destorys game object
    void Update()
    {
        if(chainGoingOutward)
        {
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x ,
                gameObject.transform.localScale.y,
                gameObject.transform.localScale.z + (speed * Time.deltaTime));

            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x,
                gameObject.transform.localPosition.y ,
                gameObject.transform.localPosition.z + (speed / 2 * Time.deltaTime));

            hook.transform.localPosition = new Vector3(hook.transform.localPosition.x,
                hook.transform.localPosition.y ,
                hook.transform.localPosition.z + (speed * Time.deltaTime));
            totalLengthAdded += 10 * Time.deltaTime;
            if(totalLengthAdded > length)
            {
                chainGoingOutward = false;
            }
        }
        else if(totalLengthAdded > 0)
        {
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x,
                gameObject.transform.localScale.y,
                gameObject.transform.localScale.z - (speed * Time.deltaTime));

            gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x ,
                gameObject.transform.localPosition.y,
                gameObject.transform.localPosition.z - (speed / 2 * Time.deltaTime));

            hook.transform.localPosition = new Vector3(hook.transform.localPosition.x,
                hook.transform.localPosition.y,
                hook.transform.localPosition.z - (speed * Time.deltaTime));

            if(playerGrabbed)
            {
                player.transform.localPosition = new Vector3(hook.transform.position.x, hook.transform.position.y, hook.transform.position.z);
            }
            totalLengthAdded -= 10 * Time.deltaTime;
        }
        else
        {
            if(playerGrabbed)
            {
                player.GetComponent<movement>().changeHealth(-50);
            }
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (object.ReferenceEquals(player, collision.gameObject))
        {
            chainGoingOutward = false;
            playerGrabbed = true;
        }
    }
}
