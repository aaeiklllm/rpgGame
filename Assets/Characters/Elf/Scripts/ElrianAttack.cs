using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;


public class ElrianAttack : MonoBehaviour
{
    public Transform player;
    public ThirdPersonController playerPrefab;
    public Projectile projectile; 
    private float attackInterval = 4.0f;
    private bool isAttacking = false;
    private bool intervalSet = false;

    // void Start() //testing purposes
    // {
    //     StartCoroutine(AttackPlayer());
    // }

     void Update()
    {
        int destroyedCount = CrystalAnimation.destroyedCrystalCount; 
        if (destroyedCount > 3 && !intervalSet)
        {
            attackInterval = 2.0f;
            intervalSet = true;
        }

        if (destroyedCount > 2 && !isAttacking)
        {
            StartCoroutine(AttackPlayer());
        }
    }

     IEnumerator AttackPlayer()
    {
        isAttacking = true;
        
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);
            // Debug.Log(attackInterval);
            transform.LookAt(player);

            ThirdPersonController clonedPlayer = Instantiate(playerPrefab);

            Projectile magicProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
            magicProjectile.player = clonedPlayer;
            magicProjectile.transform.LookAt(player.position); // Ensure the projectile faces the player

            Rigidbody rb = magicProjectile.GetComponent<Rigidbody>();
            rb.AddForce(magicProjectile.transform.forward * 20f, ForceMode.Impulse);
            rb.AddForce(magicProjectile.transform.up * 1f, ForceMode.Impulse);

            Destroy(magicProjectile.gameObject, 2f); 
            Destroy(clonedPlayer.gameObject, 2f);
            
        }
    }
}
