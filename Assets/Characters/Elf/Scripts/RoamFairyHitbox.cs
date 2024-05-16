using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;


public class RoamFairyHitbox : MonoBehaviour
{
    private RoamFairy roamFairy;
    private ThirdPersonController player;

    private bool playerIsAttacking = false;
    private bool attackCollision = false;

    
    void Start()
    {
        player = FindObjectOfType<ThirdPersonController>();
        Debug.Log("Hitbox assigned");
    }

    void Update()
    {
        playerIsAttacking = player.isAttacking;

        if (attackCollision) 
        {
            roamFairy.TakeDamage(2);

            attackCollision = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
    //      Debug.Log("OnTriggerEnter called with: " + other.gameObject.name);
    // Debug.Log("Player is attacking: " + playerIsAttacking);
    // Debug.Log("Other object layer: " + other.gameObject.layer);
    // Debug.Log("PlayerAttack layer: " + LayerMask.NameToLayer("PlayerAttack"));


    if (other.gameObject.layer == LayerMask.NameToLayer("PlayerAttack") && playerIsAttacking)
    {
        // Debug.Log("Attack collision detected and player is attacking");
        attackCollision = true;
    }

    }

    public void SetRoamFairy(RoamFairy rf)
    {
        roamFairy = rf;
    // if (roamFairy != null)
    // {
    //     Debug.Log("RoamFairy reference assigned successfully.");
    // }
    // else
    // {
    //     Debug.LogError("Failed to assign RoamFairy reference!");
    // }
    }
}
