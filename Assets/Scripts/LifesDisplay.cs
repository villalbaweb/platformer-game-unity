using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifesDisplay : MonoBehaviour
{
    // cach objects
    GameSession _gameSession;
    Text _lifesText; 
    

    // Start is called before the first frame update
    void Start()
    {
        _gameSession = FindObjectOfType<GameSession>();
        _lifesText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        _lifesText.text = $"x {_gameSession.NumberOfLifes()}";
    }
}
