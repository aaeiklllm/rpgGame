using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    public Transform target; // Reference to the player's transform (drag & drop the player GameObject into this field in the Inspector)
    public float followSpeed = 10f; // Speed at which the object follows the player

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            // Calculate the desired position to move towards (the player's position)
            Vector3 desiredPosition = target.position;
            // Move the current game object towards the desired position smoothly
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        }
    }
}
