using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject playerCamera; 
    public SaveState saveState;
    public GameObject gameOver; 

    public bool isPaused = false;
    private bool isGameOver = false;

    void Start()
    {
        saveState.SaveCurrentScene();

    }

    void Update()
    {
        isGameOver = gameOver.activeSelf;

        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        playerCamera.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        playerCamera.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;
        saveState.SaveCurrentScene();
        SceneManager.LoadScene(0);
    }
}
