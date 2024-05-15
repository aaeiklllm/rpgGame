using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class CrystalAnimation : MonoBehaviour
{
    public ThirdPersonController player; 
    public GameObject Crystal; 
    public GameObject ShatteredCrystal; 
    public static int destroyedCrystalCount = 0;
    
    private bool playerIsAttacking = false;
    private bool attackCollision = false;
    

    void Start()
    {
        ShatteredCrystal.SetActive(false);
    }

    void Update()
    {
        playerIsAttacking = player.isAttacking;
        
        if (attackCollision) 
        {
            Crystal.SetActive(false);
            ShatteredCrystal.SetActive(true);

            destroyedCrystalCount++;

            attackCollision = false;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerSword") && playerIsAttacking) //layer for destroying crystals only
        {
            attackCollision = true;
        }
    }
}
