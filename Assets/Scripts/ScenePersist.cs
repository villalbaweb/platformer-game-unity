using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    // Cache
    LevelLoader _levelLoader;
    int startSceneIndex;

    private void Awake()
    {
        int numScenePersistObjects = FindObjectsOfType<ScenePersist>().Length;

        if (numScenePersistObjects > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _levelLoader = FindObjectOfType<LevelLoader>();
        startSceneIndex = _levelLoader.GetCurrentSceneIndex();
    }

    // Update is called once per frame
    void Update()
    {
        int currentSceneIndex = _levelLoader.GetCurrentSceneIndex();
        if(currentSceneIndex != startSceneIndex)
        {
            Destroy(gameObject);
        }
    }

}
