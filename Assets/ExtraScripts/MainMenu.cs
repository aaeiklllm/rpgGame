using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{

    private const string SceneIndexKey = "LastSceneIndexPlayed";
    public Button loadSavedSceneButton;
    public SaveState saveState;

    private void Start()
    {
        CheckSavedScene();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadGame()
    {
        saveState.LoadSavedScene();
    }

     private void SaveLastSceneIndex()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        PlayerPrefs.SetInt(SceneIndexKey, currentSceneIndex);
    }

    public void Quit()
    {
        Debug.Log("Quit Game");
        Application.Quit();
    }

    private void CheckSavedScene()
    {
        if (saveState.isThereSavedScene())
        {
            loadSavedSceneButton.interactable = true; 
        }
        else
        {
            loadSavedSceneButton.interactable = false; 
        }
    }
}


