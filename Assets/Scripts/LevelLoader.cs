using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    [Header("Debug Level Loading")]
    [SerializeField] bool loadSpecificLevel = false;
    [SerializeField] int levelIndex = 0;

    public void LoadNextScene()
    {
        DeleteScenePersistObject();

        if (!loadSpecificLevel)    // Normal flow
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else  // debug levels
        {
            SceneManager.LoadScene(levelIndex);
        }

    }

    public void LoadMainMenuScene()
    {
        DeleteScenePersistObject();
        SceneManager.LoadScene("Main Menu");
    }

    public void LoadCurrentScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    public void LoadSpecificLevel(int level)
    {

        DeleteScenePersistObject();
        SceneManager.LoadScene(level);
    }

    private void DeleteScenePersistObject()
    {
        // try to find ScenePSerist Game Object if found delete it and let the new
        // level create another object if required
        ScenePersist scenePersistObject = FindObjectOfType<ScenePersist>();
        if (scenePersistObject)
        {
            Destroy(scenePersistObject.gameObject);
        }
    }
}
