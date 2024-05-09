using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    private const string SceneIndexKey = "LastSceneIndexPlayed";

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadGame()
    {
        int lastSceneIndex = PlayerPrefs.GetInt(SceneIndexKey, 0);
        SceneManager.LoadScene(lastSceneIndex);
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
}


