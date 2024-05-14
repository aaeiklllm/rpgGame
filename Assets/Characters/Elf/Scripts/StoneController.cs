using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneController : MonoBehaviour
{
    public GameObject[] stones; // array of the stones
    public float yOffset = 5.0f; // offset to adjust Y position
    private int currentIndex = 0; // index of the stone to move up
    private float timer = 0.0f;
    public float interval = 15.0f; // interval of stone spawn 
    private Dictionary<GameObject, Vector3> originalPositions; // store original positions

    private void Start()
    {
        originalPositions = new Dictionary<GameObject, Vector3>();
        foreach (GameObject stone in stones)
        {
            originalPositions[stone] = stone.transform.position;
        }
    }
   
    private void Update()
    {
        int destroyedCount = CrystalAnimation.destroyedCrystalCount; 

        if (destroyedCount > 0)
        {
            timer += Time.deltaTime;

            if (timer >= interval)
            {
                MoveStones();
                timer = 0.0f;
                currentIndex = (currentIndex + 1) % stones.Length;  // get index of next stone in array
            }
        }
    }

    private void MoveStones()
    {
        for (int i = 0; i < stones.Length; i++)
        {
            if (i == currentIndex) // move current stone up
            {
                MoveUpStone(stones[i].transform);
            }
            else // move other stones back to their orig position
            {
                MoveDownStone(stones[i].transform);
            }
        }
    }

    private void MoveUpStone(Transform stoneTransform) // the position, rotation, and scale of a gameobject
    {
        Vector3 targetPosition = stoneTransform.position + new Vector3(0.0f, yOffset, 0.0f);
        // Vector3 targetPosition = new Vector3(stoneTransform.position.x, yOffset, stoneTransform.position.z);
        StartCoroutine(MoveStoneCoroutine(stoneTransform, targetPosition, 3.0f)); //3.0 duration from start to target position 
    }

    private void MoveDownStone(Transform stoneTransform)
    {
        // Vector3 targetPosition = new Vector3(stoneTransform.position.x, 25.0f, stoneTransform.position.z);
        // Vector3 targetPosition = stoneTransform.position - new Vector3(0.0f, yOffset, 0.0f);
        Vector3 targetPosition = originalPositions[stoneTransform.gameObject];
        StartCoroutine(MoveStoneCoroutine(stoneTransform, targetPosition, 3.0f));
    }

    private IEnumerator MoveStoneCoroutine(Transform stoneTransform, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = stoneTransform.position; //current position 
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            stoneTransform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / duration); //allows gradual transition
            elapsedTime += Time.deltaTime;
            yield return null; //yield control back to unity engine per iteration; so game wont crash
        }

        stoneTransform.position = targetPosition; // ensure final position is exact
    }
}

