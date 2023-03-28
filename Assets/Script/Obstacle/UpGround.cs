using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UpGround : MonoBehaviour
{
    [SerializeField] bool _bTest = false;
    Tilemap _tile;
    Rigidbody2D _rigid;
    [SerializeField] Vector2 _vSpeed;
    [SerializeField] float _fReadySec = 1f;
    [SerializeField] float _fFireSec = 1f;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
        _tile = GetComponent<Tilemap>();
    }

    private void Update()
    {
        if (_bTest)
        {
            _bTest = false;
            DoEvent();
        }
    }

    public void DoEvent()
    {
        StartCoroutine(IUpGround());
    }

    IEnumerator IUpGround()
    {
        yield return new WaitUntil(() => transform.position.y == 0f);

        // �߻� ���� ����Ʈ
        ColorChange(new Color(1f, 1f, 1f, 0.5f));
        
        // �߻� ���
        yield return new WaitForSeconds(_fReadySec);
        Debug.Log("�߻� �غ� �Ϸ�");
        ColorChange(new Color(1f, 1f, 1f, 1f));

        _rigid.bodyType = RigidbodyType2D.Dynamic;

        // �߻�
        float fSpeed = Random.Range(_vSpeed.x, _vSpeed.y);

        _rigid.AddForce(Vector2.up * fSpeed, ForceMode2D.Impulse);

        yield return new WaitForSeconds(_fFireSec);

        // ���� �� ����
        Stop();
        _rigid.AddForce(Vector2.down * fSpeed / 2);

        while(true)
        {
            if (transform.position.y <= 0f)
            {
                Stop();
                transform.position = Vector2.zero;
                _rigid.bodyType = RigidbodyType2D.Kinematic;
                break;
            }

            yield return null;
        }
    }

    void ColorChange(Color color)
    {
        if (_tile != null)
        {
            _tile.color = color;
        }
    }

    void Stop()
    {
        if(_rigid != null)
            _rigid.velocity = Vector3.zero;
    }
}
