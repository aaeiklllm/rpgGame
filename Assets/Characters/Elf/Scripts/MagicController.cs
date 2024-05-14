using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : MonoBehaviour
{
    public GameObject magicPrefab;
    public Transform playerTransform;
    private bool isSpawning = false;
    
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
                Debug.Log("magic explosions");
                yield return new WaitForSeconds(5f);  
                Vector3 playerPosition = playerTransform.position;
                Vector3 spawnPosition = playerPosition;
                GameObject magicObject = Instantiate(magicPrefab, spawnPosition, Quaternion.identity);
                Destroy(magicObject, 3f);
            }
        }
    }
}
