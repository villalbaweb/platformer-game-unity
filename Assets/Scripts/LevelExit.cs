using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    // Config Parameters
    [Header("Timming")]
    [SerializeField] int timeToWait = 3;

    [Header("Game Flow")]
    [SerializeField] bool isLastLevelExit = false;
    [SerializeField] bool isLevelDebugEnabled = false;
    [SerializeField] int debugLevel = 0;

    // Cache
    LevelLoader _levelLoader;
    Player _playerGameObject;

    private void Start()
    {
        _levelLoader = FindObjectOfType<LevelLoader>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerLeavingDissapearVFX(collision.gameObject);
        StartCoroutine(ExitLevel());
    }

    IEnumerator ExitLevel()
    {
        yield return new WaitForSeconds(timeToWait);
        if(isLastLevelExit)
        {
            _levelLoader.LoadMainMenuScene();
        }
        else
        {
            if (!isLevelDebugEnabled)
            {
                _levelLoader.LoadNextScene();
            }
            else
            {
                _levelLoader.LoadSpecificLevel(debugLevel);
            }
        }
    }

    private void PlayerLeavingDissapearVFX(GameObject playerGameObject)
    {
        Player _player = playerGameObject.GetComponent<Player>();
        if (_player)
        {
            playerGameObject.GetComponent<Player>().DissapearVFX();
        }
    }
}
