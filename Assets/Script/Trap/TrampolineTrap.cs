using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineTrap : Trap
{
    [SerializeField] float _power;
    Animator _anim;
    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _anim.SetBool("check", true);
            Invoke("AnimationOff", 0.15f);

            // 트리거 충돌 시 위로 강하게 힘을 준다.
            Rigidbody2D rigid = collision.GetComponent<Rigidbody2D>();
            if (rigid != null)
            {
                rigid.AddForce(Vector2.up * _power, ForceMode2D.Impulse);
            }
        }
    }
        
    void AnimationOff()
    {
        if(_anim != null)
            _anim.SetBool("check", false);
    }
}
