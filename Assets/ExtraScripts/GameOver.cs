using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using StarterAssets;

public class GameOver : MonoBehaviour
{
    // public GameObject gameOverChild;
    // public GameObject playerCamera; 
    // public SaveState saveState;

    // note prevent pause when gameover

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0f;
    }

    void Update() 
    {

    }

    public void Retry()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Retrying scene");
    }

    public void MainMenu()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}