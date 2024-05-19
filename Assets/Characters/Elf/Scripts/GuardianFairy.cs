
using UnityEngine;
using UnityEngine.AI;
using StarterAssets;
using System.Collections;

public class GuardianFairy : MonoBehaviour
{
    public Transform player;
    private float health = 4f;
    public ThirdPersonController playerPrefab;
    
    //animation
    public AnimationClip[] animationClips;
    private Animation anim;

    //attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    private bool isDestroyed = false;

    //states
    public LayerMask whatIsGround, whatIsPlayer;
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    //cloning
    public GuardianFairy fairyPrefab;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    //red flash
    private Material[] originalMaterials;
    public Material flashMaterial; // Reference to the red flash material
    public Renderer characterRenderer;
    private float flashTimer; // Timer to track the duration of the flash effect
    private bool isFlashing; // Flag to indicate if the character is currently flashing
    public float flashDuration = 0.2f; // Duration of the flash effect in seconds

    public AudioSource sfxManager;
    public AudioClip attackSFX;
    public AudioClip hitSFX;

    private void Awake()
    {
        anim = GetComponent<Animation>();
        player = GameObject.Find("PlayerArmature").transform;

        initialPosition = transform.position;
        initialRotation = transform.rotation;

        if (sfxManager == null) sfxManager = GameObject.Find("AttackSFX").GetComponent<AudioSource>();
        GuardianFairyHitbox hitbox = GetComponent<GuardianFairyHitbox>();
        hitbox.SetGuardianFairy(this);
    }

    void Start()
    {
        Transform huayaoTransform = transform.Find("HuaYao_01");

        if (huayaoTransform == null)
        {
            Debug.LogError("Child GameObject named 'HuaYao_01' not found!");
        }
        else
        {
            characterRenderer = huayaoTransform.GetComponent<Renderer>();

            if (characterRenderer == null)
            {
                Debug.LogError("Renderer component not found on the child GameObject named 'HuaYao_01'!");
            }
            else
            {
                originalMaterials = characterRenderer.materials;
            }
        }
    }

    private void Update()
    {
        //check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer); //vector3 center, radius of sightrange, layermask 
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer); //performs collision check

        if (playerInAttackRange && playerInSightRange) AttackPlayer();
        else{
             anim.CrossFade(animationClips[4].name);
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

    private void AttackPlayer()
    {
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            anim.CrossFade(animationClips[0].name);

            sfxManager.PlayOneShot(attackSFX);

            GameObject magicProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
            magicProjectile.transform.LookAt(player.position); // Ensure the projectile faces the player

            Rigidbody rb = magicProjectile.GetComponent<Rigidbody>();
            rb.AddForce(magicProjectile.transform.forward * 2f, ForceMode.Impulse);
            rb.AddForce(magicProjectile.transform.up * 3f, ForceMode.Impulse);

            Destroy(magicProjectile.gameObject, 2f); 

            alreadyAttacked = true;
          
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }
    private void ResetAttack()
    {
        anim.CrossFade(animationClips[4].name);
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        if (!isDestroyed)
        {

            sfxManager.PlayOneShot(hitSFX);

            health -= damage;
            Debug.Log("Taking Damage");
            StartFlash();

            if (health <= 0)
            {
                StartCoroutine(SpawnFairy(1f));
                StartCoroutine(DestroyEnemy());
            }
        }
    }


    IEnumerator DestroyEnemy()
    {
        Debug.Log("Destroying Enemy");
        if (!isDestroyed)
        {
            isDestroyed = true; // Set isDestroyed flag here to prevent subsequent calls

            Debug.Log("Destroying Enemy 2");
            anim.CrossFade(animationClips[3].name);
            yield return new WaitForSeconds(1.2f);
        
            Destroy(gameObject);
        }
    }

    IEnumerator SpawnFairy(float delay)
    {
       yield return new WaitForSeconds(delay);
        GuardianFairy fairyClone = Instantiate(fairyPrefab, initialPosition, initialRotation);
        fairyClone.player = player;
        fairyClone.playerPrefab = playerPrefab;
        fairyClone.fairyPrefab = fairyPrefab;
        fairyClone.characterRenderer = characterRenderer;
        fairyClone.sfxManager = sfxManager;
        fairyClone.hitSFX = hitSFX;
        fairyClone.attackSFX = attackSFX;

        Debug.Log("Spawning Fairy");

        GuardianFairyHitbox hitbox = fairyClone.GetComponent<GuardianFairyHitbox>();
        hitbox.SetGuardianFairy(fairyClone);

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

    private void OnDrawGizmosSelected() //draws gizmos for visualization
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
