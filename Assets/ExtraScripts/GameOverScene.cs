using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.SceneManagement;

public class GameOverScene : MonoBehaviour
{
    public ThirdPersonController player;
    private bool playerDefeated;
    public GameObject gameOver;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerDefeated = player.isDefeated;
        if (playerDefeated)
        {
            gameOver.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0f;
        } 
        else{
            gameOver.SetActive(false);
        }
    }
}
