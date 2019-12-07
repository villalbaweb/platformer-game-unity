using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePersist : MonoBehaviour
{
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
}
