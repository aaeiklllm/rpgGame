using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElrianAttack : MonoBehaviour
{
    public Transform player;
    public GameObject magicProjectile; 
    private float attackInterval = 8.0f;
    private bool isAttacking = false;
    private bool intervalSet = false;

     void Update()
    {
        int destroyedCount = CrystalAnimation.destroyedCrystalCount; 
        if (destroyedCount > 3 && !intervalSet)
        {
            attackInterval = 4.0f;
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
            Debug.Log(attackInterval);
            transform.LookAt(player);

            GameObject projectile = Instantiate(magicProjectile, transform.position, Quaternion.identity);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 20f, ForceMode.Impulse);
            rb.AddForce(transform.up * 0.5f, ForceMode.Impulse);

            Destroy(projectile, 2f); //destroy after 2 seconds
            
        }
    }
}
