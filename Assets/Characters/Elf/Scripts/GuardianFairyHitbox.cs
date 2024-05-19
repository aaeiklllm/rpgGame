using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;


public class GuardianFairyHitbox : MonoBehaviour
{
    private GuardianFairy GuardianFairy;
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
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(playerIsAttacking);
        Debug.Log(other.gameObject.layer == LayerMask.NameToLayer("PlayerAttack"));

        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerAttack") && playerIsAttacking)
        {
            GuardianFairy.TakeDamage(3);
            player.countHit();

        }
        
        if (other.gameObject.layer == LayerMask.NameToLayer("DaggerAttack"))
        {
            GuardianFairy.TakeDamage(2);

        }

        if (other.gameObject.layer == LayerMask.NameToLayer("LightningAttack"))
        {
            GuardianFairy.TakeDamage(5);

        }

    }

    public void SetGuardianFairy(GuardianFairy gf)
    {
        GuardianFairy = gf;
    }
}
