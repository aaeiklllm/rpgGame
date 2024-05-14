using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElrianJump : MonoBehaviour
{
    public GameObject[] magicEffects;
    private int currentPositionIndex = 0; 
    private int lastPositionIndex = 0; 
    public ParticleSystem barrier;
    private bool intervalSet = false;
    private float jumpInterval = 10.0f;

    [System.Serializable]
    public struct PositionData
    {
        public Vector3 position;
        public float rotationY;
    }

    public PositionData[] jumpPositions;

    void Start()
    {
        transform.position = jumpPositions[currentPositionIndex].position;
        ApplyRotationY(jumpPositions[currentPositionIndex].rotationY);
        ActivateMagicEffect(currentPositionIndex);
        StartCoroutine(MoveCharacter()); //coroutine lets you execute code several times
    }

     void Update()
    {
        int destroyedCount = CrystalAnimation.destroyedCrystalCount; 
        if (destroyedCount > 3 && !intervalSet)
        {
            jumpInterval = 5.0f;
            intervalSet = true;
        }
    }

    IEnumerator MoveCharacter() //coroutines implemented using this
    {
        while (true)
        {
            Debug.Log("Start particle system");
            barrier.Play();

            yield return new WaitForSeconds(jumpInterval); //wait for 10 seconds before continuing execution
            currentPositionIndex = (currentPositionIndex + 1) % jumpPositions.Length;
            lastPositionIndex = (currentPositionIndex == 0) ? jumpPositions.Length - 1 : currentPositionIndex - 1;
            Debug.Log("Stop particle system");
            barrier.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            DeactivateAllMagicEffects();
            ActivateMagicEffect(currentPositionIndex);
            ActivateMagicEffect(lastPositionIndex);

            transform.position = jumpPositions[currentPositionIndex].position;
            ApplyRotationY(jumpPositions[currentPositionIndex].rotationY);
            
        }
    }

    void ApplyRotationY(float rotationY)
    {
        Vector3 currentRotation = transform.eulerAngles;
        transform.rotation = Quaternion.Euler(currentRotation.x, rotationY, currentRotation.z);
    }

    void ActivateMagicEffect(int index)
    {
        if (index >= 0 && index < magicEffects.Length)
        {
            magicEffects[index].SetActive(true);
        }
    }

     void DeactivateAllMagicEffects()
    {
        foreach (GameObject magicEffect in magicEffects)
        {
            magicEffect.SetActive(false);
        }
    }
}
