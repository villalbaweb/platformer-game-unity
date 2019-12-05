using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsDisplay : MonoBehaviour
{
    // cach objects
    GameSession _gameSession;
    Text _starsText;


    // Start is called before the first frame update
    void Start()
    {
        _gameSession = FindObjectOfType<GameSession>();
        _starsText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        _starsText.text = $"x {_gameSession.NumberOfStars()}";
    }
}
