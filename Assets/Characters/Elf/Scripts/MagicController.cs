using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class MagicController : MonoBehaviour
{
    public Projectile magicPrefab;
    public ThirdPersonController playerPrefab;
    public Transform playerTransform;
    private bool isSpawning = false;

    void Start()
    {
        StartCoroutine(SpawnMagic()); //testing
    }
    
    void Update()
    {
        // int destroyedCount = CrystalAnimation.destroyedCrystalCount; 
        // if (destroyedCount > 1 && !isSpawning)
        // {
        //     StartCoroutine(SpawnMagic());
        // }
    }

     IEnumerator SpawnMagic()
    {
        isSpawning = true;

        while (true)
        {
            {
                // Debug.Log("magic explosions");
                yield return new WaitForSeconds(5f);  
                Vector3 playerPosition = playerTransform.position;
                Vector3 spawnPosition = playerPosition;
                Projectile magicObject = Instantiate(magicPrefab, spawnPosition, Quaternion.identity);
                magicObject.player = playerPrefab;
                Destroy(magicObject, 3f);
            }
        }
    }
}
