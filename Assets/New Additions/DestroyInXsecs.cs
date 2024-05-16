using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInXsecs : MonoBehaviour
{
    private float timeSpawned;
    public float timeToDestruction = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        timeSpawned = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > timeSpawned + timeToDestruction) Destroy(this.gameObject);
    }
}
