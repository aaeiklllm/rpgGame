using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AbilityIconScript : MonoBehaviour
{

    public Image icon;
    public ThirdPersonController player;

    public Sprite daggerIcon;
    public Sprite healIcon;
    public Sprite lightningIcon;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        changeSprite(player.currentAbilityIndex);
    }

    void changeSprite(int index) 
    {
        if (index == 0) 
        {
            icon.sprite = daggerIcon;
        }
        else if (index == 1) 
        {
            icon.sprite = healIcon;
        }
        else if (index == 2) 
        {
            icon.sprite = lightningIcon;
        }
    }
}
