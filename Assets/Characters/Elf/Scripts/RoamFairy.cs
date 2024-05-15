using UnityEngine;
using UnityEngine.AI;
using StarterAssets;
using System.Collections;

public class RoamFairy : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    private float health = 100f;

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

    // States
    public LayerMask whatIsGround, whatIsPlayer;
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    //cloning
    public RoamFairy fairyPrefab;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Awake()
    {
        anim = GetComponent<Animation>();
        agent = GetComponent<NavMeshAgent>();

        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // Invoke("SpawnFairy", 5f); //test
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
        health -= damage;
        Debug.Log("Taking Damage");

        if (health <= 0)
            Destroy(gameObject);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
        Invoke("SpawnFairy", 10f);
    }

    private void SpawnFairy()
    {
        Debug.Log("Spawning Fairy");
        RoamFairy fairyClone = Instantiate(fairyPrefab, initialPosition, initialRotation);
        fairyClone.player = player;
        fairyClone.playerPrefab = playerPrefab;
        fairyClone.fairyPrefab = fairyPrefab;

        RoamFairyHitbox hitbox = fairyClone.GetComponent<RoamFairyHitbox>();
        hitbox.SetRoamFairy(this);

    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
