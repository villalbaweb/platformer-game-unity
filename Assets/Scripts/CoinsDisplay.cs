using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsDisplay : MonoBehaviour
{
    // cach objects
    GameSession _gameSession;
    Text _coinsText;


    // Start is called before the first frame update
    void Start()
    {
        _gameSession = FindObjectOfType<GameSession>();
        _coinsText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        _coinsText.text = $"x {_gameSession.NumberOfCoins()}";
    }
}
