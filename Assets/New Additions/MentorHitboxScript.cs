using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MentorHitboxScript : MonoBehaviour
{
    public Mentor mentor;
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

        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerAttack") && playerIsAttacking)
        {
            Debug.Log("Mentor hit by Blade");
            mentor.TakeDamage(3);
            player.countHit();
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("DaggerAttack"))
        {
            Debug.Log("Mentor hit by Dagger");
            mentor.TakeDamage(2);
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("LightningAttack")) 
        {
            Debug.Log("Mentor hit by Lightning");
            mentor.TakeDamage(5);
        }

    }
}
