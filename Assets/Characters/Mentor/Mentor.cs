
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
    private int lastAttackIndex = -1;
    public float rotationSpeed = 20f;

     //states
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        attackTriggers = new string[] { "Attacking1", "Attacking2", "Attacking3", "Attacking4" };
        player = GameObject.Find("Skeleton").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = agent.GetComponent<Animator>();
        
    }

    private void Update()
    {

        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer); //vector3 center, radius of sightrange, layermask 
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer); //performs collision check

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
    }

     private void AttackPlayer()
    {
         animator.SetBool("isWalking", false);
        if (playerInAttackRange)
        {
            Vector3 targetDirection = (player.position - transform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

             if (!isAttacking && Vector3.Dot(transform.forward, targetDirection) > 0)
            {
                animator.SetBool("isBlocking", false);
                isAttacking = true;
                int nextIndex = GetNextAttackIndex();
                randomIndex = nextIndex;
                Debug.Log(randomIndex);

                animator.SetTrigger(attackTriggers[randomIndex]);
                Invoke(nameof(ResetAttack), timeBetweenAttacks);
            }

            else{
                 animator.SetBool("isBlocking", true);
            }
        }
        else{
            ResetAttack();
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

    private void ResetAttack()
    {
        animator.ResetTrigger(attackTriggers[randomIndex]);
        animator.SetBool("isBlocking", false);
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
