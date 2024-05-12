using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gateOpener : MonoBehaviour
{

    public GameObject Gate;
    public GameObject player;
    public float gateOpenYPosition = 4.5f; 
    public float gateCloseYPosition = 2.41f; 
    public float moveDuration = 1f;

    private bool isMoving = false;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && !isMoving)
        {
            StartCoroutine(MoveGateCoroutine(gateOpenYPosition));
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player && !isMoving)
        {
            StartCoroutine(MoveGateCoroutine(gateCloseYPosition));
        }
    }

    IEnumerator MoveGateCoroutine(float targetY)
    {
        isMoving = true;

        Vector3 initialPosition = Gate.transform.position;
        Vector3 targetPosition = new Vector3(initialPosition.x, targetY, initialPosition.z);

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            Gate.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Gate.transform.position = targetPosition;

        isMoving = false;
    }
}
