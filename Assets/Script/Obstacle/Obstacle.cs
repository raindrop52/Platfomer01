using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public virtual void Init()
    {
    }

    protected void Disapear()
    {
        Destroy(gameObject);
    }

    protected void Show(bool show)
    {
        gameObject.SetActive(show);
    }
}
