using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.SceneManagement;


public class ElrianAttack : MonoBehaviour
{
    public Transform player;
    public ThirdPersonController playerPrefab;
    public Projectile projectile; 
    private float attackInterval = 4.0f;
    private bool isAttacking = false;
    private bool intervalSet = false;
    private float health = 25f;
    public GameObject barrier;
    public Animator animator;


    //red flash
    private Material[] originalMaterials;
    public Material flashMaterial; // Reference to the red flash material
    public Renderer characterRenderer;
    private float flashTimer; // Timer to track the duration of the flash effect
    private bool isFlashing; // Flag to indicate if the character is currently flashing
    public float flashDuration = 0.2f; // Duration of the flash effect in seconds


    void Start() 
    {
        originalMaterials = characterRenderer.materials;   
         StartCoroutine(AttackPlayer());     
    }

    

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

        if (destroyedCount > 3)
        {
            barrier.SetActive(false);
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

            ThirdPersonController clonedPlayer = Instantiate(playerPrefab);

            Projectile magicProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
            magicProjectile.player = clonedPlayer;
            magicProjectile.transform.LookAt(player.position); // Ensure the projectile faces the player

            animator.SetTrigger("castMagic");
            StartCoroutine(ResetMagicCastTrigger(3f));

            Rigidbody rb = magicProjectile.GetComponent<Rigidbody>();
            rb.AddForce(magicProjectile.transform.forward * 20f, ForceMode.Impulse);
            rb.AddForce(magicProjectile.transform.up * 1f, ForceMode.Impulse);

            Destroy(magicProjectile.gameObject, 2f); 
            Destroy(clonedPlayer.gameObject, 2f);
            
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
        StartFlash();

        Debug.Log("Taking Damage");

        if (health <= 0)
            FallToGround();
    }

    private void FallToGround()
    {
        StartCoroutine(FallAnimation());
    }

    private IEnumerator FallAnimation()
    {
        float animationDuration = 2f;

        Vector3 initialPosition = transform.position;
        Vector3 targetPosition = new Vector3(initialPosition.x, 0.3f, initialPosition.z);

        float elapsedTime = 0f;

        animator.SetBool("isFalling", true);

        while (elapsedTime < animationDuration)
        {
            float t = elapsedTime / animationDuration;
            transform.position = Vector3.Lerp(initialPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        animator.SetBool("isFalling", false);

        transform.position = targetPosition;

         // Invoke("LoadCampfireScene", 10f);
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
        SceneManager.LoadScene(5); //6campfire
    }
}
