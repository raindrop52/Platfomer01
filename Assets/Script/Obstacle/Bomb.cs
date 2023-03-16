using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Obstacle
{
    // ��ź
    // �÷��̾�� ��ų� Ground�� ������ ����
    // ���� FX
    [SerializeField] ParticleSystem _psBomb;
    // ���� ����
    [SerializeField] AudioSource _sfxBomb;
    Rigidbody2D _rigid;
    Vector2 _vMin;
    Vector2 _vMax;

    private void Awake()
    {
        _psBomb.gameObject.SetActive(false);
        _rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector2 min, Vector2 max)
    {
        _vMin = min;
        _vMax = max;
    }

    public void Reload()
    {
        _rigid.simulated = true;
        transform.localPosition = Vector3.zero;
        Show(true);
    }

    public void Fire()
    {
        if (_rigid != null)
        {
            float x = Random.Range(_vMin.x, _vMax.x);
            float y = Random.Range(_vMin.y, _vMax.y);

            Vector2 dir = new Vector2(-1f * x, 1f * y);
            _rigid.AddForce(dir, ForceMode2D.Impulse);
        }
    }

    private void Update()
    {
        // ����ó�� ( �ٴ����� �������� ��� )
        if (transform.position.y < -12f)
        {
            // Velocity 0���� ����
            if (_rigid != null)
                _rigid.simulated = false;
            // �������� �ʱ�ȭ
            transform.localPosition = Vector3.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �÷��̾� or ���� �浹 ��
        if (collision.CompareTag("Player") || collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            StartCoroutine(IChecker());
        }
    }

    IEnumerator IChecker()
    {
        // �浹 �� ���� ó��
        // �߷� �����Ͽ� ���ߵ��� ��
        if (_rigid.simulated)
        {
            _rigid.simulated = false;
        }

        // ���� ����Ʈ ǥ��
        if (_psBomb != null)
        {
            _psBomb.gameObject.SetActive(true);
            _psBomb.Play();
        }

        // ���� ���� ����
        if (_sfxBomb != null)
        {
            _sfxBomb.Play();
        }

        // ���� ����Ʈ ���� ó�� Ȯ�� ��
        yield return new WaitUntil(()=> _psBomb.gameObject.activeSelf == false);

        // ���� ó��
        Show(false);
    }
}
