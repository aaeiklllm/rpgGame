using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalAnimation : MonoBehaviour
{
    public GameObject Crystal; 
    public GameObject ShatteredCrystal; 
    private bool isMousePressed = false;
    public static int destroyedCrystalCount = 0;

    void Start()
    {
        ShatteredCrystal.SetActive(false);
    }

    void Update()
    {
        isMousePressed = Input.GetKey(KeyCode.Mouse0);
    }


    void OnTriggerStay(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("PlayerSword") && isMousePressed)
        {
            // Debug.Log("Collision with PlayerSword layer and mouse button pressed.");

            if (ShatteredCrystal != null)
            {
                Crystal.SetActive(false);
                ShatteredCrystal.SetActive(true);

                // Debug.Log("Crystal " + Crystal.name + " deactivated.");
                // Debug.Log("Shattered crystal " + ShatteredCrystal.name + " activated.");
                destroyedCrystalCount++;
            }
        }
    }
}
