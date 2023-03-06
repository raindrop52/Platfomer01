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
        if (GameManager.i.pPlayer != null)
        {
            transform.position = new Vector2(GameManager.i.pPlayer.transform.position.x, 0);
        }
    }
}
