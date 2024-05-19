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
    public ElrianAttack elrian;

    public AudioSource sfxManager;
    public AudioClip breakSFX;
    

    void Awake(){
        destroyedCrystalCount = 0;
    }

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
            sfxManager.PlayOneShot(breakSFX);

            elrian.TakeDamage(2);

            destroyedCrystalCount++;

            attackCollision = false;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerAttack") && playerIsAttacking) //layer for destroying crystals only
        {
            attackCollision = true;
        }
    }
}
