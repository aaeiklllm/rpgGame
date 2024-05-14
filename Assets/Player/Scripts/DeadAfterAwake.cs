using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadAfterAwake : MonoBehaviour
{
    public float delay = 2f; // Delay in seconds before destroying the object

    void Start()
    {
        // Destroy the GameObject after 'delay' seconds
        Destroy(gameObject, delay);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
