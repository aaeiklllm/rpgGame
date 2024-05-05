using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : MonoBehaviour
{
     public GameObject magicPrefab;
    
    void Start()
    {
        StartCoroutine(SpawnMagic());
    }

     IEnumerator SpawnMagic()
    {
        while (true)
        {
            // wait for 10 seconds
            yield return new WaitForSeconds(2f);
            Vector3 randomPosition = new Vector3(Random.Range(-3f, 50f), 0f, Random.Range(-10f, 50f));
            GameObject magicObject = Instantiate(magicPrefab, randomPosition, Quaternion.identity);
            Destroy(magicObject, 3f);
        }
    }
}
