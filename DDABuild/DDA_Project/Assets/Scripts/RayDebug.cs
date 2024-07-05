using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayDebug : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * 2, Color.blue);
    }
}
