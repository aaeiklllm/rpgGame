
using UnityEngine;
using UnityEngine.AI;
using StarterAssets;

public class GuardianFairy : MonoBehaviour
{
    public Transform player;
    private float health = 100f;
    public ThirdPersonController playerPrefab;
    
    //animation
    public AnimationClip[] animationClips;
    private Animation anim;

    //attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public Projectile projectile;

    //states
    public LayerMask whatIsGround, whatIsPlayer;
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    //cloning
    public GuardianFairy fairyPrefab;
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private void Awake()
    {
        anim = GetComponent<Animation>();
        player = GameObject.Find("PlayerArmature").transform;

        initialPosition = transform.position;
        initialRotation = transform.rotation;

        // Invoke("SpawnFairy", 5f); //test
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
    }

    private void AttackPlayer()
    {
        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            anim.CrossFade(animationClips[0].name);

            Projectile magicProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
            magicProjectile.player = playerPrefab;
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
        GuardianFairy fairyClone = Instantiate(fairyPrefab, initialPosition, initialRotation);
        fairyClone.player = player;
        fairyClone.playerPrefab = playerPrefab;
        fairyClone.fairyPrefab = fairyPrefab;

        GuardianFairyHitbox hitbox = fairyClone.GetComponent<GuardianFairyHitbox>();
        hitbox.SetGuardianFairy(this);

    }

    private void OnDrawGizmosSelected() //draws gizmos for visualization
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
