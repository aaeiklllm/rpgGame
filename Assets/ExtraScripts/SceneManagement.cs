using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
   
    void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
    }
}
