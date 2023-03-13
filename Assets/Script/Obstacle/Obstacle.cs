using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public virtual void Init(float time = 1f)
    {
        
    }

    protected void Disapear()
    {
        Destroy(gameObject);
    }
}
