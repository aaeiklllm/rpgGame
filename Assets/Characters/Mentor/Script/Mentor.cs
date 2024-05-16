
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using StarterAssets;
using UnityEngine.SceneManagement;

public class Mentor : MonoBehaviour
{
    //mentor & player
    public NavMeshAgent agent;
    public Transform player; 

    public LayerMask whatIsGround, whatIsPlayer;

    public int health = 35;
    
    //animation
    private Animator animator;
    public string[] attackTriggers;
    public int randomIndex;

    //attacking
    
    public bool isAttacking;
    public bool isBlocking;
    private int lastAttackIndex = -1;
    public float rotationSpeed = 20f;

     //states
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public static Mentor instance;

    public float lastAttacktime = 0f;
    public float timeBetweenAttacks = 2f;
    // check cooldown with :  Time.time >= lastAttacktime + timeBetweenAttacks

    public float lastBlocktime = 0f;
    public float maxBlockduration = 3f;


    // detect inputs
    private bool isMousePressed = false;

    public Renderer[] characterRenderers; // Array of Renderer components for the character
    public Material flashMaterial; // Reference to the red flash material
    public float flashDuration = 0.2f; // Duration of the flash effect in seconds

    private Material[] originalMaterials; // Array to store original materials of the character
    private float flashTimer; // Timer to track the duration of the flash effect
    private bool isFlashing; // Flag to indicate if the character is currently flashing

    public TrailRenderer weaponTrail;
    public GameObject aoeFX;
    public GameObject aoeHitbox;


    public GameObject mentorSword;
    public GameObject mentorShield;

    public bool endFight = false;


    public float endTime;

    private void Start()
    {
        // Initialize the originalMaterials array with the same length as the characterRenderers array
        originalMaterials = new Material[characterRenderers.Length];

        // Store the original materials of each renderer
        for (int i = 0; i < characterRenderers.Length; i++)
        {
            originalMaterials[i] = characterRenderers[i].material;
        }
    }

    private void Awake()
    {
        attackTriggers = new string[] { "Attacking1", "Attacking2", "Attacking3", "Attacking4", "Attacking5" };

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        instance = this;

    }

    private void Update()
    {
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer); //vector3 center, radius of sightrange, layermask 
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer); //performs collision check

        if (isBlocking && Time.time >= lastBlocktime + maxBlockduration) 
        {
            animator.SetBool("isBlocking", false);
        }

        if (endFight) 
        {
            Vector3 targetDirection = (player.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Time.time >= endTime + 4f) nextScene();
        }
        else 
        {
            if (!playerInSightRange && !playerInAttackRange)
            {
                Idle();
            }
            else if (playerInSightRange && !playerInAttackRange)
            {
                ChasePlayer();
            }
            else if (playerInAttackRange && playerInSightRange)
            {
                AttackPlayer();
            }

            if (isAttacking)
            {
                weaponTrail.emitting = true;
            }
            else
            {
                weaponTrail.emitting = false;
            }

        }
        
        // Check if the character is currently flashing
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

    private void nextScene() 
    {
        SceneManager.LoadScene("1Campfire", LoadSceneMode.Single);
    }

    private void Idle()
    {
       animator.SetBool("isWalking", false);
    }

    private void ChasePlayer()
    {
        animator.SetBool("isWalking", true);

        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0f; 
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        //agent.SetDestination(player.position);

    }

    private void AttackPlayer()
    {
        //agent.SetDestination(transform.position);

        animator.SetBool("isWalking", false);
        if (playerInAttackRange && !isBlocking)
        {
            Vector3 targetDirection = (player.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (!isAttacking && Vector3.Dot(transform.forward, targetDirection) > 0 && Time.time >= lastAttacktime + timeBetweenAttacks)
            {
                animator.SetBool("isBlocking", false);
                int nextIndex = GetNextAttackIndex();
                randomIndex = nextIndex;
                Debug.Log(randomIndex);

                animator.SetTrigger(attackTriggers[randomIndex]);
                lastAttacktime = Time.time + 10f;

                //Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }
    }

    private int GetNextAttackIndex()
    {
        int nextIndex = Random.Range(0, attackTriggers.Length);
    
        while (nextIndex == lastAttackIndex)
        {
            nextIndex = Random.Range(0, attackTriggers.Length);
        }

        lastAttackIndex = nextIndex;
        return nextIndex;
    }

    // deprecated
    private void ResetAttack()
    {
        Debug.Log("ATTACK RESET ====================================");
        animator.ResetTrigger(attackTriggers[randomIndex]);
        animator.SetBool("isBlocking", false);
    }

    public void TakeDamage(int damage)
    {
        if (!isBlocking)
        {
            health -= damage;
            Debug.Log("HP: " + health); 

            StartFlash();

            if (health <= 0)
            {
                // end
                animator.SetBool("isWalking", false);
                animator.SetBool("isBlocking", false);


                animator.SetBool("isClapping", true);

                //disappear WEAPONS
                Destroy(mentorSword);
                Destroy(mentorShield);

                endTime = Time.time;
                endFight = true;

            }
            else if (!isAttacking)
            {
                // if still alive, keep blocking
                animator.SetBool("isBlocking", true);
                lastBlocktime = Time.time;
            }
        }
        else 
        {
            // PLAY BLOCK SFX
            Debug.Log("Blocked");
        }
    }

    // Method to start the red flash effect
    private void StartFlash()
    {
        // Set the flash material to all renderers
        for (int i = 0; i < characterRenderers.Length; i++)
        {
            characterRenderers[i].material = flashMaterial;
        }

        // Initialize the flash timer and set the flashing flag
        flashTimer = 0f;
        isFlashing = true;
    }

    // Method to stop the red flash effect and restore original materials
    private void StopFlash()
    {
        // Restore the original materials
        for (int i = 0; i < characterRenderers.Length; i++)
        {
            characterRenderers[i].material = originalMaterials[i];
        }

        // Reset the flashing flag
        isFlashing = false;
    }

    public void spawnShockwave() 
    {
        GameObject fxClone = Instantiate(aoeFX, transform.position, Quaternion.identity);
        GameObject aoeClone = Instantiate(aoeHitbox, transform.position, Quaternion.identity);

        Destroy(fxClone, 2f);
        Destroy(aoeClone, 0.25f);
    }


     private void OnDrawGizmosSelected() //draws gizmos for visualization
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
