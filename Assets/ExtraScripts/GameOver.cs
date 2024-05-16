using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public GameObject gameOverUI;
    public GameObject playerCamera; 
    public SaveState saveState;

    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void StartGameOver()
    {
        gameOverUI.SetActive(true);

        playerCamera.SetActive(false);
        Time.timeScale = 0f;
    }

    public void Retry()
    {
        // Hide the game over UI
        // gameOverUI.SetActive(false);
        // playerCamera.SetActive(true);

        saveState.LoadSavedScene();
    }

    public void MainMenu()
    {
        // gameOverUI.SetActive(false);
        // Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}