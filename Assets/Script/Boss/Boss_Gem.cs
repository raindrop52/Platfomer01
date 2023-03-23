using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Gem : MonoBehaviour
{
    Boss _boss;

    void Awake()
    {
        _boss = transform.parent.GetComponent<Boss>();
        if (_boss != null)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Item") || collision.CompareTag("Player"))
        {
            if (_boss != null)
            {
                _boss.HP--;
            }
        }
    }
}
