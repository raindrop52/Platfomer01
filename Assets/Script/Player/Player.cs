using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Rigidbody2D _rigid2D;
    Animator _anim;
    SpriteRenderer _sprite;
    public bool LeftView
    {
        get { return _sprite.flipX; }
    }

    public float _fSpeed = 0.5f;
    public float _fMaxSpeed = 5f;
    public float _fJumpPower = 8f;

    [SerializeField] bool _bPlayerState = false;      // �÷��̾��� �̵� ���� Ȯ��
    [SerializeField] bool _bJumpState = false;       // ���� ���� Ȯ��
    public bool JumpState
    {
        get { return _bJumpState; }
    }

    [SerializeField] GameObject _fxDie;
    ParticleSystem _psDie;

    public bool _bGodMode = false;
    public bool _bStopMode = false;

    public GameObject _goMyItem;
    
    private void Awake()
    {
        _rigid2D = GetComponent<Rigidbody2D>();
        _rigid2D.velocity = Vector2.zero;
        _anim = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
    }

    public void Init()
    {
        // �״� ����Ʈ ����
        if (_fxDie != null)
        {
            GameObject go = Instantiate(_fxDie);
            go.transform.localPosition = transform.position;
            go.transform.SetParent(GameManager.i.transform);
            _psDie = go.GetComponent<ParticleSystem>();
        }
    }

    public void StopVel()
    {
        _rigid2D.velocity = Vector3.zero;
    }

    [SerializeField] float _testPower = 3000f;
    public void GodMove(Vector2 vec)
    {
        _bGodMode = true;
        _bStopMode = true;

        StopVel();
        
        _rigid2D.AddForce(vec * _testPower);
    }

    public void GodStop()
    {
        _bGodMode = false;
        _bStopMode = false;

        StopVel();
    }
        
    void Update()
    {
        if (_bStopMode)
            return;

        // ������ ���
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if(_goMyItem != null && _goMyItem.activeSelf == false)
            {
                Item myItem = _goMyItem.GetComponent<Item>();
                myItem.Use();
            }
        }

        // ����
        if (Input.GetKeyDown(KeyCode.X) && _bJumpState == false
            && _anim.GetBool("IsJump") == false)
        {
            _bJumpState = true;
            _rigid2D.AddForce(Vector2.up * _fJumpPower, ForceMode2D.Impulse);
            _anim.SetBool("IsJump", true);
        }

        // ���� �ִϸ��̼� ó��
        if (Mathf.Abs(_rigid2D.velocity.x) <= 0.1)        // ���� ����
        {
            _bPlayerState = false;
            _anim.SetBool("IsRun", false);
        }
        else
        {
            _bPlayerState = true;
            _anim.SetBool("IsRun", true);
        }

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
        if (_bStopMode)
            return;

        float x = Input.GetAxisRaw("Horizontal");

        // �̵�
        _rigid2D.AddForce(new Vector2(x * _fSpeed, 0f), ForceMode2D.Impulse);

        if (_rigid2D.velocity.x > _fMaxSpeed)
            _rigid2D.velocity = new Vector2(_fMaxSpeed, _rigid2D.velocity.y);

        if (_rigid2D.velocity.x < _fMaxSpeed * -1)
            _rigid2D.velocity = new Vector2(_fMaxSpeed * -1, _rigid2D.velocity.y);

        Debug.DrawRay(_rigid2D.position, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(_rigid2D.position, Vector3.down, 2, LayerMask.GetMask("Ground"));
        
        if (_rigid2D.velocity.y == 0f)
        {
            if (_bJumpState)
                _bJumpState = false;

            if (_anim.GetBool("IsJump"))
                _anim.SetBool("IsJump", false);
        }
        else if (_rigid2D.velocity.y < 0f)    // �Ʒ��� ������ ����
        {
            // ���߿� ������ ������
            if (rayHit.collider == null)
                // �ִϸ��̼� ����
                _anim.SetBool("IsJump", true);
            // ���� ���� ������Ʈ�� ������
            if (rayHit.collider != null)
            {
                // ������ �Ÿ��� üũ�ؼ�
                if (rayHit.distance < 0.98f)
                {
                    // �ִϸ��̼� ����
                    _anim.SetBool("IsJump", false);
                    // ���� ���� ����
                    _bJumpState = false;
                }
            }
        }
    }

    void Die()
    {
        if (_bGodMode)
            return;

        StartCoroutine(PlayerDie());
    }

    IEnumerator PlayerDie()
    {
        // �÷��̾��� ������ ����
        _rigid2D.bodyType = RigidbodyType2D.Kinematic;

        // �״� ����Ʈ ǥ��
        if (_psDie != null)
        {
            _psDie.transform.position = transform.position;
            _psDie.gameObject.SetActive(true);
            _psDie.Play();
        }

        // ���� �Ŵ����� ���ӿ��� ����
        if (GameManager.i != null)
            GameManager.i.GameOver = true;

        // �÷��̾� ��Ȱ��ȭ
        gameObject.SetActive(false);

        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �÷��̾� ���
        if (collision.CompareTag("DeathObj"))
        {
            Debug.Log("�÷��̾� ���");
            Die();
        }

        // ���̺� ����Ʈ�� ���
        if (collision.CompareTag("Save"))
        {
            Debug.Log("���̺� ����Ʈ ����");
            SavePoint savePoint = collision.gameObject.GetComponent<SavePoint>();
            if(savePoint != null)
            {
                savePoint.Touching();
            }
        }
    }

    public void Resurrection(Vector2 resPos)
    {
        // ��Ȱ �� ������ �� ���� ����
        _anim.SetBool("IsJump", true);
        _bJumpState = true;
        _rigid2D.bodyType = RigidbodyType2D.Dynamic;
        transform.position = resPos;

        if (transform.parent != null)
            transform.SetParent(null);

        if (_goMyItem != null)
            Destroy(_goMyItem);
    }
}
