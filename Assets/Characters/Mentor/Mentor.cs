
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Mentor : MonoBehaviour
{
    //mentor & player
    public NavMeshAgent agent;
    public Transform player; 

    public LayerMask whatIsGround, whatIsPlayer;
    public float chaseSpeed;
    public float health;

    //animation
    private Animator animator;
    public string[] attackTriggers;
    public int randomIndex;

    //attacking
    public float timeBetweenAttacks;
    bool isAttacking;

     //states
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("Skeleton").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        attackTriggers = new string[] { "Attacking1", "Attacking2", "Attacking3", "Attacking4" };
    }

    private void Update()
    {
        
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer); //vector3 center, radius of sightrange, layermask 
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer); //performs collision check

        if (!playerInSightRange && !playerInAttackRange && agent.remainingDistance <= agent.stoppingDistance) Idle();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();

    }

    private void Idle()
    {
       animator.SetBool("isWalking", false);
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
        animator.SetBool("isWalking", true);
        agent.speed = chaseSpeed; 
    }

     private void AttackPlayer()
    {
        agent.SetDestination(transform.position); //mordon doesn't move while attacking
        transform.LookAt(player); //add code when player is jumping

        if (playerInAttackRange)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                randomIndex = Random.Range(0, attackTriggers.Length);
                Debug.Log(randomIndex);
                animator.SetTrigger(attackTriggers[randomIndex]);
                animator.SetBool("isWalking", false);
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }
        }
        else{
            ResetAttack();
        }
    }

    private void ResetAttack()
    {
        animator.ResetTrigger(attackTriggers[randomIndex]);
        isAttacking = false;
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
