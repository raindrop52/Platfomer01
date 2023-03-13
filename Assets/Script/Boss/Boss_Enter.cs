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

    private void OnTriggerExit2D(Collider2D collision)
    {
        // �÷��̾ ����������
        if (collision.CompareTag("Player"))
        {
            Boss boss = transform.parent.GetComponent<Boss>();
            if (boss != null)
            {
                boss.StartAction();
            }
        }
    }
}
