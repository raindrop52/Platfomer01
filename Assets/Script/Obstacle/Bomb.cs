using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Obstacle
{
    // 폭탄
    // 플레이어에게 닿거나 Ground에 닿으면 폭파
    // 폭파 FX
    [SerializeField] ParticleSystem _psBomb;
    // 폭파 사운드
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
        // 예외처리 ( 바닥으로 떨어지는 경우 )
        if (transform.position.y < -12f)
        {
            // Velocity 0으로 설정
            if (_rigid != null)
                _rigid.simulated = false;
            // 원점으로 초기화
            transform.localPosition = Vector3.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어 or 지면 충돌 시
        if (collision.CompareTag("Player") || collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            StartCoroutine(IChecker());
        }
    }

    IEnumerator IChecker()
    {
        // 충돌 시 동작 처리
        // 중력 제거하여 멈추도록 함
        if (_rigid.simulated)
        {
            _rigid.simulated = false;
        }

        // 폭발 이펙트 표시
        if (_psBomb != null)
        {
            _psBomb.gameObject.SetActive(true);
            _psBomb.Play();
        }

        // 폭발 사운드 송출
        if (_sfxBomb != null)
        {
            _sfxBomb.Play();
        }

        // 폭발 이펙트 숨김 처리 확인 후
        yield return new WaitUntil(()=> _psBomb.gameObject.activeSelf == false);

        // 숨김 처리
        Show(false);
    }
}
