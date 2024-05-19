using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitboxScript : MonoBehaviour
{
    public Mentor mentor;
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
        mentorIsAttacking = mentor.isAttacking;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("EnemyAttack") && mentorIsAttacking)
        {
            Debug.Log("Hit by Mordon's blade");
            player.takeDamage(3);
        }
    }
}
