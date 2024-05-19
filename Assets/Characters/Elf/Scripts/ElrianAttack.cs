using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.SceneManagement;


public class ElrianAttack : MonoBehaviour
{
    public Transform player;
    public ThirdPersonController playerPrefab;
    public GameObject projectile; 
    private float attackInterval = 4.0f;
    private bool isAttacking = false;
    private bool intervalSet = false;
    public float health = 18f; //10 (5 hits) dagger + 8 crystals
    public GameObject barrier;
    public Animator animator;


    //red flash
    private Material[] originalMaterials;
    public Material flashMaterial; // Reference to the red flash material
    public Renderer characterRenderer;
    private float flashTimer; // Timer to track the duration of the flash effect
    private bool isFlashing; // Flag to indicate if the character is currently flashing
    public float flashDuration = 0.2f; // Duration of the flash effect in seconds

    public bool grounded = false;

    public AudioSource sfxManager;
    public AudioClip attackSFX;
    public AudioClip hitSFX;

    void Start() 
    {
        originalMaterials = characterRenderer.materials;   
         //StartCoroutine(AttackPlayer());     
        // FallToGround();
    }

    

     void Update()
    {
        int destroyedCount = CrystalAnimation.destroyedCrystalCount; 
        if (destroyedCount > 3 && !intervalSet) //4th crystal
        {
            attackInterval = 2.0f;
            intervalSet = true;
        }

        if (destroyedCount > 3) //4th crystal
        {
            barrier.SetActive(false);
        }

        if (destroyedCount > 2 && !isAttacking) //3rd crystal
        {
            StartCoroutine(AttackPlayer());
            
        }

        
        if (isFlashing)
        {
            // Update the flash timer
            flashTimer += Time.deltaTime;

            // If the flash duration has elapsed, stop flashing and restore original materials
            if (flashTimer >= flashDuration)
            {
                StopFlash();
            }
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

            Vector3 spawnLocation = new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z) + transform.forward * 5f;

            GameObject magicProjectile = Instantiate(projectile, spawnLocation, Quaternion.identity);
            magicProjectile.transform.LookAt(player.position); // Ensure the projectile faces the player

            animator.SetTrigger("castMagic");
            sfxManager.PlayOneShot(attackSFX);
            StartCoroutine(ResetMagicCastTrigger(3f));

            Rigidbody rb = magicProjectile.GetComponent<Rigidbody>();
            rb.AddForce(magicProjectile.transform.forward * 20f, ForceMode.Impulse);
            rb.AddForce(magicProjectile.transform.up * 1f, ForceMode.Impulse);

            Destroy(magicProjectile.gameObject, 2f); 
        }
    }

    IEnumerator ResetMagicCastTrigger(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.ResetTrigger("castMagic");
    }


    public void TakeDamage(int damage)
    {
        health -= damage;
        sfxManager.PlayOneShot(hitSFX);
        StartFlash();

        Debug.Log("Taking Damage");

        if (health <= 0)
            FallToGround();
    }

    private void FallToGround()
    {
        playerPrefab.currentHealth = 25;
        StartCoroutine(FallAnimation());
    }

    private IEnumerator FallAnimation()
    {
        Invoke("LoadCampfireScene", 7f);

        float animationDuration = 2f;

        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = new Vector3(initialPosition.x, 0.3f, initialPosition.z);

        float elapsedTime = 0f;

        animator.SetBool("isFalling", true);

        while (!grounded)
        {
            float t = elapsedTime / animationDuration;
            transform.position = Vector3.Lerp(initialPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        animator.SetBool("isFalling", false);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log(playerIsAttacking);

        if (other.gameObject.layer == LayerMask.NameToLayer("whatIsGround"))
        {
            grounded = true;
        }
    }

    private void StartFlash()
    {
        // Create a new array to hold modified materials (copies of original materials)
        Material[] modifiedMaterials = new Material[originalMaterials.Length];
        for (int i = 0; i < originalMaterials.Length; i++)
        {
            // Create a copy of each original material
            modifiedMaterials[i] = new Material(originalMaterials[i]);
        }

        // Assign the flashMaterial to all modified materials
        for (int i = 0; i < modifiedMaterials.Length; i++)
        {
            modifiedMaterials[i].color = Color.red;
        }

        // Assign the modified materials to the character's Renderer component
        characterRenderer.materials = modifiedMaterials;

        // Initialize the flash timer and set the flashing flag
        flashTimer = 0f;
        isFlashing = true;
    }

    // Method to stop the red flash effect and restore original materials
    private void StopFlash()
    {
        // Restore the original materials
        characterRenderer.materials = originalMaterials;

        // Reset the flashing flag
        isFlashing = false;
    }


    private void LoadCampfireScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
