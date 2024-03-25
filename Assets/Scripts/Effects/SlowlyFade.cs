using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SlowlyFade : MonoBehaviour
{
    public float coolwDownTillDestroy;
    private float currentTime;
    private Color initColor;
    private void Start()
    {
        initColor = gameObject.GetComponent<LineRenderer>().material.color;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        currentTime+= Time.deltaTime;
        gameObject.GetComponent<LineRenderer>().material.color = 
                new Color(initColor.r, initColor.g, initColor.b, Mathf.Lerp(gameObject.GetComponent<LineRenderer>().material.color.a, 0, coolwDownTillDestroy * Time.deltaTime));
        if (coolwDownTillDestroy <= currentTime)
        {
            Destroy(gameObject);
        }
    }
}
