using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpGround : MonoBehaviour
{
    Rigidbody2D _rigid;
    [SerializeField] float _fSpeed = 3f;
    [SerializeField] float _fSec = 1f;
    [SerializeField] bool _bFire = false;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    public void DoEvent()
    {
        StartCoroutine(IUpGround());
    }

    IEnumerator IUpGround()
    {
        // 발사 전조 이펙트

        // 발사 대기
        yield return new WaitUntil(() => _bFire == true);

        // 발사
        _rigid.AddForce(Vector2.up * _fSpeed);

        yield return new WaitForSeconds(_fSec);

        // 정지 후 낙하
        Stop();
        _rigid.AddForce(Vector2.down * _fSpeed / 2);

        while(true)
        {
            if (transform.position.y <= 0f)
            {
                Stop();
                transform.position = Vector2.zero;
                break;
            }

            yield return null;
        }
    }

    void Stop()
    {
        if(_rigid != null)
            _rigid.velocity = Vector3.zero;
    }
}
