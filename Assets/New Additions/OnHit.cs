using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnHit : MonoBehaviour
{
    GameObject Target;
    private bool isMousePressed = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isMousePressed = Input.GetKey(KeyCode.Mouse0);
    }

    void OnTriggerStay(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerAttack") && isMousePressed)
        {
        }
    }
}
