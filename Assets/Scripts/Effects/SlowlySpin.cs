using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SlowlySpin : MonoBehaviour
{
    public float rotateSpeed;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up* Time.deltaTime * rotateSpeed);
    }
}
