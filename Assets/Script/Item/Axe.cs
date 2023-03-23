using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Item
{
    [SerializeField] float _fLimitX = 2f;
    [SerializeField] float _fPower = 1f;
    [SerializeField] float _fSpin = 120f;
    Rigidbody2D _rigid;
    Vector2 _oldPos;        // �߻� �� ��ġ

    // ȹ�� �� ����
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
            // �÷��̾��� ���� üũ
            Vector2 dir;
            if (_player.LeftView)
                dir = Vector2.left;
            else
                dir = Vector2.right;

            // �߻� �� ��ġ ���
            _oldPos = transform.position;

            // �߻�
            _rigid.AddForce(dir * _fPower);

            // ȸ�� ����
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
