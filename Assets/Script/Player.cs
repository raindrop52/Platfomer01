using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D _rigid2D;
    Animator _anim;
    SpriteRenderer _sprite;

    public float _fSpeed = 0.5f;
    public float _fMaxSpeed = 5f;
    public float _fJumpPower = 8f;

    bool _isJump = false;

    [SerializeField] GameObject _fxDie;

    private void Awake()
    {
        _rigid2D = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 아이템 사용
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // (현재 미구현)
        }

        // 점프
        if (Input.GetKeyDown(KeyCode.X) && _isJump == false
            && _anim.GetBool("IsJump") == false)
        {
            _isJump = true;
            _rigid2D.AddForce(Vector2.up * _fJumpPower, ForceMode2D.Impulse);
            _anim.SetBool("IsJump", true);
        }

        // 정지 애니메이션 처리
        if (Mathf.Abs(_rigid2D.velocity.x) <= 0.1)        // 멈춤 상태
            _anim.SetBool("IsRun", false);
        else
            _anim.SetBool("IsRun", true);
        // 정지 가속도
        if (Input.GetButtonUp("Horizontal"))
        {
            _rigid2D.velocity = new Vector2(0.5f * _rigid2D.velocity.normalized.x, _rigid2D.velocity.y);
        }
        // 방향 전환
        // 왼쪽이면 true, 오른쪽이면 false
        if(Input.GetButton("Horizontal"))
            _sprite.flipX = Input.GetAxisRaw("Horizontal") == -1;
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");

        // 이동
        _rigid2D.AddForce(new Vector2(x * _fSpeed, 0f), ForceMode2D.Impulse);

        if (_rigid2D.velocity.x > _fMaxSpeed)
            _rigid2D.velocity = new Vector2(_fMaxSpeed, _rigid2D.velocity.y);

        if (_rigid2D.velocity.x < _fMaxSpeed * -1)
            _rigid2D.velocity = new Vector2(_fMaxSpeed * -1, _rigid2D.velocity.y);

        Debug.DrawRay(_rigid2D.position, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(_rigid2D.position, Vector3.down, 2, LayerMask.GetMask("Ground"));

        if (_rigid2D.velocity.y < 0)    // 아래로 떨어질 때만
        {
            // 빔을 맞은 오브젝트가 있으면
            if (rayHit.collider != null && _isJump == true
                && _anim.GetBool("IsJump") == true)
            {
                if (rayHit.distance < 0.98f)
                {
                    _anim.SetBool("IsJump", false);
                    _isJump = false;
                }
            }
        }
    }

    void Die()
    {
        if (GameManager.i.BossOn)
        {
            // 사망 시 보스전 변수 해제
            GameManager.i.BossOn = false;
        }
        
        Invoke("Disapear", 0f);
    }

    void Disapear()
    {
        if(_fxDie != null)
        {
            GameObject go = Instantiate(_fxDie);
            go.transform.localPosition = transform.position;
        }    

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어 사망
        if (collision.CompareTag("DeathObj"))
        {
            Debug.Log("플레이어 사망");
            Die();
        }

        // 세이브 포인트인 경우
        if (collision.CompareTag("Save"))
        {
            Debug.Log("세이브 포인트 저장");
            SavePoint savePoint = collision.gameObject.GetComponent<SavePoint>();
            if(savePoint != null)
            {
                savePoint.Touching();
            }
        }
    }

    public void Resurrection()
    {
        // 부활 시 내려올 때 점프 방지
        _anim.SetBool("IsJump", true);
        _isJump = true;
    }
}
