using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Item
{
    [SerializeField] float _fLimitX = 2f;
    [SerializeField] float _fPower = 1f;
    [SerializeField] float _fSpin = 120f;
    Rigidbody2D _rigid;
    Vector2 _oldPos;        // 발사 전 위치

    // 획득 시 동작
    public override void Equip(Player player)
    {
        base.Equip(player);

        _rigid = GetComponent<Rigidbody2D>();
    }

    public override void Use()
    {
        base.Use();

        transform.localPosition = Vector3.zero;

        if (_rigid != null)
        {
            // 플레이어의 방향 체크
            Vector2 dir;
            if (_player.LeftView)
                dir = Vector2.left;
            else
                dir = Vector2.right;

            // 발사 전 위치 기록
            _oldPos = transform.position;

            // 발사
            _rigid.AddForce(dir * _fPower);

            // 회전 동작
            StartCoroutine(IFire());
        }
    }

    IEnumerator IFire()
    {
        while(true)
        {
            if (Vector2.Distance(_oldPos, transform.position) >= _fLimitX)
                break;

            transform.Rotate(new Vector3(0f, 0f, Time.deltaTime * _fSpin));

            yield return null;
        }

        Show(false);
    }
}
