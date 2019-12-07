using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : EnemyMovement
{
    [SerializeField] float timeToTurn = 2;

    float initialTimeToTurn;


    private void Awake()
    {
        initialTimeToTurn = timeToTurn;
    }

    private void Update()
    {
        timeToTurn -= Time.deltaTime;
        if (timeToTurn < Mathf.Epsilon)
        {
            Debug.Log("Turn");
            timeToTurn = initialTimeToTurn;
            base.ChangeMovingDirection();
        }

        base.Move();
    }
}
