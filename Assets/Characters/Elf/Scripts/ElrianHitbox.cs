using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class ElrianHitbox : MonoBehaviour
{
    public ElrianAttack elrian;
    public ThirdPersonController player; 

    private bool isMousePressed = false;
    private bool playerIsAttacking = false;

    private bool attackCollision = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerIsAttacking = player.isAttacking;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(playerIsAttacking);

        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerAttack") && playerIsAttacking)
        {
            Debug.Log("Mentor hit by Blade");
            elrian.TakeDamage(3);
            // player.countHit();
        }


        if (other.gameObject.layer == LayerMask.NameToLayer("DaggerAttack"))
        {
            Debug.Log("Elrian hit by Dagger");
            elrian.TakeDamage(2);
        }
    }
}
