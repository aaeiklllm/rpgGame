using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ElrianHealthBar : MonoBehaviour
{
    public Slider healthbar;
    public ElrianAttack target;

    // Start is called before the first frame update
    void Start()
    {
        healthbar.maxValue = target.health;
    }

    // Update is called once per frame
    void Update()
    {
        healthbar.value = target.health;
    }
}
