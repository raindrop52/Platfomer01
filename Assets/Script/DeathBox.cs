using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBox : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        if (GameManager.i.player != null)
        {
            transform.position = new Vector2(GameManager.i.player.transform.position.x, -11);
        }
    }
}
