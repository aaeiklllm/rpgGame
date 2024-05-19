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
    }

    private void OnTriggerEnter(Collider other)
    {

    if (other.gameObject.layer == LayerMask.NameToLayer("PlayerAttack") && playerIsAttacking)
    {
        roamFairy.TakeDamage(3);
        player.countHit();

    }

    if (other.gameObject.layer == LayerMask.NameToLayer("DaggerAttack"))
    {
        roamFairy.TakeDamage(2);

    }

    if (other.gameObject.layer == LayerMask.NameToLayer("LightningAttack"))
    {
        roamFairy.TakeDamage(5);

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
