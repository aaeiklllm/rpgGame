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
        Debug.Log(playerIsAttacking);
        Debug.Log(other.gameObject.layer == LayerMask.NameToLayer("PlayerAttack"));

        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerAttack") && playerIsAttacking)
        {
            attackCollision = true;
        }

    }

    public void SetRoamFairy(RoamFairy rf)
    {
        roamFairy = rf;
    }
}
