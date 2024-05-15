using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveState : MonoBehaviour
{
    private const string SceneKey = "SavedScene";

    public bool isThereSavedScene()
    {
        return PlayerPrefs.HasKey(SceneKey);
    }

    public void SaveCurrentScene()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString(SceneKey, currentScene);
        PlayerPrefs.Save();
    }

    public void LoadSavedScene()
    {
        if (PlayerPrefs.HasKey(SceneKey))
        {
            string savedScene = PlayerPrefs.GetString(SceneKey);
            SceneManager.LoadScene(savedScene);
        }
        else
        {
            Debug.LogWarning("No saved scene found.");
        }
    }
}
