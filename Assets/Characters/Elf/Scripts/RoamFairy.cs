using UnityEngine;
using UnityEngine.AI;
using StarterAssets;
using System.Collections;

public class RoamFairy : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    private float health = 4f;

    // animation
    public AnimationClip[] animationClips;
    private Animation anim;

    // Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public ThirdPersonController playerPrefab;
    public Projectile projectile;
    private bool isDestroyed = false;

    // States
    public LayerMask whatIsGround, whatIsPlayer;
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    //cloning
    public RoamFairy fairyPrefab;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    //red flash
    private Material[] originalMaterials;
    public Material flashMaterial; // Reference to the red flash material
    private Renderer characterRenderer;
    private float flashTimer; // Timer to track the duration of the flash effect
    private bool isFlashing; // Flag to indicate if the character is currently flashing
    public float flashDuration = 0.2f; // Duration of the flash effect in seconds

    void Awake()
    {
        anim = GetComponent<Animation>();
        agent = GetComponent<NavMeshAgent>();

        initialPosition = transform.position;
        initialRotation = transform.rotation;

        RoamFairyHitbox hitbox = GetComponent<RoamFairyHitbox>();
        hitbox.SetRoamFairy(this);
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

    void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange)
            Patroling();
        if (playerInSightRange && !playerInAttackRange)
            ChasePlayer();
        if (playerInAttackRange && playerInSightRange)
            AttackPlayer();

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

    void Patroling()
    {
        if (!walkPointSet)
            SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        anim.CrossFade(animationClips[4].name);

        if (Vector3.Distance(transform.position, walkPoint) < 1f)
            walkPointSet = false;
    }

    void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    void ChasePlayer()
    {
        agent.SetDestination(player.position);
        anim.CrossFade(animationClips[4].name);
    }

   private void AttackPlayer()
    {
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            anim.CrossFade(animationClips[0].name);

            Projectile magicProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
            magicProjectile.player = playerPrefab;
            magicProjectile.transform.LookAt(player.position); 

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
        RoamFairy fairyClone = Instantiate(fairyPrefab, initialPosition, initialRotation);
        fairyClone.player = player;
        fairyClone.playerPrefab = playerPrefab;
        fairyClone.fairyPrefab = fairyPrefab;
        fairyClone.characterRenderer = characterRenderer;
        // anim.CrossFade(animationClips[1].name);

        Debug.Log("Spawning Fairy");

        RoamFairyHitbox hitbox = fairyClone.GetComponent<RoamFairyHitbox>();
        hitbox.SetRoamFairy(fairyClone);
    }

     private void StartFlash()
    {
         Debug.Log("flash");

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


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
