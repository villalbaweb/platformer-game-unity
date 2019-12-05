using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalScroll : MonoBehaviour
{
    // Config Params
    [Tooltip ("Game unites per second")]
    [SerializeField] float scrollRate = 0.02f;

    // Update is called once per frame
    void Update()
    {
        MoveUp();
    }

    private void MoveUp()
    {
        float yMove = scrollRate * Time.deltaTime;
        Vector2 lavaPosition = new Vector2(0, yMove);
        transform.Translate(lavaPosition, Space.World);
    }
}
