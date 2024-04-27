using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElrianAttack : MonoBehaviour
{
    public Transform player;
    public GameObject magicProjectile; 
    public float attackInterval = 7f;


    void Start()
    {
        StartCoroutine(AttackPlayer());
    }

     IEnumerator AttackPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);
            
            transform.LookAt(player);

            GameObject projectile = Instantiate(magicProjectile, transform.position, Quaternion.identity);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 20f, ForceMode.Impulse);
            rb.AddForce(transform.up * 0.5f, ForceMode.Impulse);

            Destroy(projectile, 2f); //destroy after 2 seconds
            
        }
    }
}
