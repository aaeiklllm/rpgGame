using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class MagicController : MonoBehaviour
{
    public Projectile magicPrefab;
    public GameObject magicDesign;
    public ThirdPersonController playerPrefab;
    public Transform playerTransform;
    private bool isSpawning = false;
    float heightOffset = 10.0f; 
    private float downwardForce = 20f;

    void Start()
    {
        // StartCoroutine(SpawnMagic()); //testing
    }
    
    void Update()
    {
        int destroyedCount = CrystalAnimation.destroyedCrystalCount; 
        if (destroyedCount > 1 && !isSpawning)
        {
            StartCoroutine(SpawnMagic());
        }
    }

     IEnumerator SpawnMagic()
    {
        isSpawning = true;

        while (true)
        {
            {
                yield return new WaitForSeconds(5f);  
                Vector3 playerPosition = playerTransform.position;
                Vector3 spawnPosition = playerPosition + new Vector3(0, heightOffset, 0);

                GameObject magicDesign2 = Instantiate(magicDesign, playerPosition, Quaternion.identity);

                yield return new WaitForSeconds(0.5f);  
                //  Debug.Log("magic explosions");
                Projectile magicObject = Instantiate(magicPrefab, spawnPosition, Quaternion.identity);
                magicObject.player = playerPrefab;

                Rigidbody rb = magicObject.GetComponent<Rigidbody>();
                rb.useGravity = true;
                rb.AddForce(Vector3.down * downwardForce, ForceMode.Impulse);
                Destroy(magicObject.gameObject, 3f);


            }
        }
    }
}
