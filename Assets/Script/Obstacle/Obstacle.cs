using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public virtual void Init(float time = 1f)
    {
        Invoke("Disapear", time);
    }

    protected void Disapear()
    {
        Destroy(gameObject);
    }
}
