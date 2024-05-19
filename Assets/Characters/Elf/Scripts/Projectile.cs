using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;


public class Projectile : MonoBehaviour
{
    public ThirdPersonController player; 
    private bool playerIsBlocking = false;
    private bool attackCollision = false;
    private int playerHealth;

    void Update()
    {
        playerHealth = player.currentHealth; 
        playerIsBlocking = player.isBlocking;
        if (attackCollision) 
        {
            //Player Takedamage

            attackCollision = false;
        }
    }

  private void OnCollisionEnter(Collision other)
    {
        // Debug.Log("Hit layer: " + LayerMask.LayerToName(other.gameObject.layer));
        // // Debug.Log("Player Blocking: " + playerIsBlocking);
        if (other.gameObject.layer == LayerMask.NameToLayer("whatIsPlayer"))
        {
            if (!playerIsBlocking)
            {
                // Debug.Log("Player Hit"); //add code for subtracting player health
                attackCollision = true;
            }
            // else
            // {
            //     Debug.Log("Player is blocking");
            // }
        }
        // else
        // {
        //     Debug.Log("Hit layer: " + LayerMask.LayerToName(other.gameObject.layer));
        // }
    }


}
   