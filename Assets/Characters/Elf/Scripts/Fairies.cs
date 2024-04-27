
using UnityEngine;
using UnityEngine.AI;

public class Fairies : MonoBehaviour
{
    public NavMeshAgent agent; //allows enemies to navigate game environment

    public Transform player; //gets the position & rotation of player

    public LayerMask whatIsGround, whatIsPlayer;

    public float health;

    public AnimationClip[] animationClips;
    private Animation anim;

    //patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange = 5;

    //attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //states
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        anim = GetComponent<Animation>();
        player = GameObject.Find("Capsule").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer); //vector3 center, radius of sightrange, layermask 
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer); //performs collision check

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        anim.CrossFade(animationClips[4].name);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround)) //checks if new walkpoint (ground) is valid for new position
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);

        anim.CrossFade(animationClips[4].name);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position); //fairies don't move while attacking

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            anim.CrossFade(animationClips[0].name);

            GameObject magicProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
            Rigidbody rb = magicProjectile.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 5f, ForceMode.Impulse);
            rb.AddForce(transform.up * 0.5f, ForceMode.Impulse);

            Destroy(magicProjectile, 2f); //destroy after 2 seconds

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

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected() //draws gizmos for visualization
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
