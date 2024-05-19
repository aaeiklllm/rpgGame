using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitboxElf : MonoBehaviour
{
    public ThirdPersonController player;

    private bool mentorIsAttacking = false;
    private bool attackCollision = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyAttack") && mentorIsAttacking)
        {
            Debug.Log("Hit by Mordon's blade");
            player.takeDamage(3);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("ElrianAttack"))
        {
            Debug.Log("Hit by Elrian");
            player.takeDamage(3);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("GuardianFairyAttack"))
        {
            Debug.Log("Hit by Guardian Fairy");
            player.takeDamage(3);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("MeteorAttack"))
        {
            Debug.Log("Hit by Meteor");
            player.takeDamage(2);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("RoamFairyAttack"))
        {
            Debug.Log("Hit by Roam Fairy");
            player.takeDamage(1);
        }
    }
}
