using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveState : MonoBehaviour
{
    private const string SceneKey = "SavedScene";

    private void OnApplicationQuit()
    {
        SaveCurrentScene();
    }

    public bool isThereSavedScene()
    {
        return PlayerPrefs.HasKey(SceneKey);
    }

    public void SaveCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString(SceneKey, currentScene);
        PlayerPrefs.Save();
        Debug.Log("Saved scene: " + currentScene);
    }

    public void LoadSavedScene()
    {
        if (PlayerPrefs.HasKey(SceneKey))
        {
            string savedScene = PlayerPrefs.GetString(SceneKey);
            Debug.Log("Loaded scene: " + savedScene);
            SceneManager.LoadSceneAsync(savedScene, LoadSceneMode.Single);
        }
        else
        {
            Debug.LogWarning("No saved scene found.");
        }
    }
}
