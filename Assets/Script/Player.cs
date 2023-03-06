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

    private void Awake()
    {
        _rigid2D = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        // ������ ���
        if (Input.GetKeyDown(KeyCode.Z))
        {
            // (���� �̱���)
        }

        // ����
        if (Input.GetKeyDown(KeyCode.X) && _anim.GetBool("IsJump") == false)
        {
            _rigid2D.AddForce(Vector2.up * _fJumpPower, ForceMode2D.Impulse);
            _anim.SetBool("IsJump", true);
        }

        // ���� �ִϸ��̼� ó��
        if (Mathf.Abs(_rigid2D.velocity.x) <= 0.1)        // ���� ����
            _anim.SetBool("IsRun", false);
        else
            _anim.SetBool("IsRun", true);
        // ���� ���ӵ�
        if (Input.GetButtonUp("Horizontal"))
        {
            _rigid2D.velocity = new Vector2(0.5f * _rigid2D.velocity.normalized.x, _rigid2D.velocity.y);
        }
        // ���� ��ȯ
        // �����̸� true, �������̸� false
        if(Input.GetButton("Horizontal"))
            _sprite.flipX = Input.GetAxisRaw("Horizontal") == -1;
    }

    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");

        // �̵�
        _rigid2D.AddForce(new Vector2(x * _fSpeed, 0f), ForceMode2D.Impulse);

        if (_rigid2D.velocity.x > _fMaxSpeed)
            _rigid2D.velocity = new Vector2(_fMaxSpeed, _rigid2D.velocity.y);

        if (_rigid2D.velocity.x < _fMaxSpeed * -1)
            _rigid2D.velocity = new Vector2(_fMaxSpeed * -1, _rigid2D.velocity.y);

        Debug.DrawRay(_rigid2D.position, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(_rigid2D.position, Vector3.down, 2, LayerMask.GetMask("Ground"));

        if (_rigid2D.velocity.y < 0)    // �Ʒ��� ������ ����
        {
            if (rayHit.collider != null && _anim.GetBool("IsJump") == true) // ���� ���� ������Ʈ�� ������
            {
                if (rayHit.distance < 0.98f)
                    _anim.SetBool("IsJump", false);
            }
        }
    }

    void Die(bool isFall = false)
    {
        if (GameManager.i.BossOn)
        {
            // ��� �� ������ ���� ����
            GameManager.i.BossOn = false;
        }

        if (isFall)
            Invoke("Disapear", 0f);
        else
        {
            _anim.SetBool("IsDeath", true);
            Invoke("Disapear", 1f);
        }
    }

    void Disapear()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ���� ������(���� �ڽ�)�� ���
        if (collision.gameObject.layer == LayerMask.NameToLayer("DeathBox"))
        {
            Debug.Log("�÷��̾� �߶���");
            Die(true);
        }

        // ��ֹ��� �ɸ� ���
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Debug.Log("�÷��̾� ���");
            Die();
        }

        // ���̺� ����Ʈ�� ���
        if (collision.gameObject.layer == LayerMask.NameToLayer("Save"))
        {
            Debug.Log("���̺� ����Ʈ ����");
            SavePoint savePoint = collision.gameObject.GetComponent<SavePoint>();
            if(savePoint != null)
            {
                savePoint.Touching();
            }
        }
    }

    
}
