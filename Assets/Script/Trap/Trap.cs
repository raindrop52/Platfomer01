using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] protected GameObject _obstacle;
    [SerializeField] protected Transform _showTrans;

    void Start()
    {
        
    }

    public virtual void Restore()
    {
        // R key 눌릴 시 초기화
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    protected void Disapear()
    {
        Destroy(gameObject);
    }

    protected void Disapear(bool isShow)
    {
        gameObject.SetActive(isShow);
    }
}
