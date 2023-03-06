using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineTrap : Trap
{
    [SerializeField] float _power;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Ʈ���� �浹 �� ���� ���ϰ� ���� �ش�.
            Rigidbody2D rigid = collision.GetComponent<Rigidbody2D>();
            if (rigid != null)
            {
                rigid.AddForce(Vector2.up * _power, ForceMode2D.Impulse);
            }
        }
    }
}
