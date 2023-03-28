using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Enter : MonoBehaviour
{
    BoxCollider2D _col;

    void Awake()
    {
        _col = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if(GameManager.i.BossOn)
        {
            if(_col.isTrigger)
                _col.isTrigger = false;
        }
        else
        {
            if (!_col.isTrigger)
                _col.isTrigger = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 플레이어가 부딪히면
        if (collision.gameObject.CompareTag("Player") && !GameManager.i.BossOn)
        {
            Player player = collision.transform.GetComponent<Player>();
            if (player != null)
            {
                player.StopVel();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 플레이어가 빠져나가면
        if (collision.CompareTag("Player") && !GameManager.i.BossOn)
        {
            Boss boss = transform.parent.GetComponent<Boss>();
            if (boss != null)
            {
                boss.StartAction();
            }
        }
    }
}
