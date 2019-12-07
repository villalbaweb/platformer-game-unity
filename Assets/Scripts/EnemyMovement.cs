using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Config Params
    [SerializeField] float moveSpeed = 1f;

    // Cache
    Rigidbody2D _rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void Move()
    {
        Vector2 playerVelocity;
        if (IsFacingRight())
        {
            playerVelocity = new Vector2(moveSpeed, _rigidbody2D.velocity.y);
        }
        else
        {
            playerVelocity = new Vector2(-moveSpeed, _rigidbody2D.velocity.y);
        }

        _rigidbody2D.velocity = playerVelocity;
    }

    private bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-(Mathf.Sign(_rigidbody2D.velocity.x)), 1f);
    }

    public void ChangeMovingDirection()
    {
        Debug.Log("Changin direction...");
        transform.localScale = new Vector2(-(Mathf.Sign(_rigidbody2D.velocity.x)), 1f);
    }
}
