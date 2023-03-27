using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // �ִϸ��̼�
    Animator _anim;
    // �浹ü
    Rigidbody2D _rigid2D;
    BoxCollider2D _col2D;
    // ���� ��ȣ
    [SerializeField] int _nNo;
    // ���� �ڷ�ƾ
    Coroutine _runningCo = null;
    // ���� Ÿ�� �浹ü ������Ʈ
    [SerializeField] GameObject _goHitGem;
    // ���� Ÿ�� ����Ʈ
    [SerializeField] ParticleSystem _psHit;
    // ���� ���� ������Ʈ
    [Header("���� A")]
    Projectil_Fire _projFire;
    [Header("���� B")]
    [SerializeField] GameObject _preAttack;
    [SerializeField] int _nMinCnt;
    [SerializeField] int _nMaxCnt;
    [SerializeField] List<Obstacle> _obsAtks;
    [Header("���� C")]
    [SerializeField] bool _bLast = false;
    [SerializeField] float _fUpY = 8f;       // _goUpBoss�� ����
    [SerializeField] GameObject _goUpBoss;
    [SerializeField] List<GameObject> _goUpTiles;

    // ü��
    [SerializeField] int _hp = 0;
    public int HP
    {
        get { return _hp; }
        set { _hp = value; }
    }
    int _maxHp = 3;
    // ���� ( -1 = Die / 0 = Awake / 1 = Idle / 2 = Attack )
    int _state = -1;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _rigid2D = GetComponent<Rigidbody2D>();
        _col2D = GetComponent<BoxCollider2D>();
        _projFire = GetComponent<Projectil_Fire>();

        CreateAttackTypeB();

        _hp = _maxHp;
        _bLast = false;
        if (_goUpBoss != null)
            _goUpBoss.SetActive(false);
    }

    public void Restore()
    {
        // state�� 0���� ũ�� Die ���� �ʾұ⿡ ������ �ʿ� X
        if (_state >= 0)
        {
            // �ڷ�ƾ ���� ����
            if(_runningCo != null)
                StopCoroutine(_runningCo);
            _hp = _maxHp;
            // �������� �ʱ�ȭ
            _bLast = false;
            _col2D.isTrigger = false;
            if (_goUpBoss != null)
                _goUpBoss.SetActive(false);
            // �޸� ���·� ��ȯ
            ChangeState(-1);
            // Show ���·� ��ȯ
            Show(true);
        }
    }

    public void EventAnimation_ShowGem(int nShow)
    {
        if (_goHitGem != null)
        {
            if(nShow == 0)
                _goHitGem.SetActive(false);
            else if (nShow == 1)
                _goHitGem.SetActive(true);
        }
    }

    public void StartAction()
    {
        // ������ ����
        GameManager.i.BossOn = true;
        GameManager.i.BossNo = _nNo;

        // ���� �ൿ ����
        BossAction();
    }

    [SerializeField] bool _bTest = false;
    private void Update()
    {
        // ���� ���� �׽�Ʈ�� �ڵ�
        if(_bTest)
        {
            _bTest = false;
            StartAction();
        }

        // �Ѵ� ���� ���
        if(_hp < _maxHp)
        {
            // Hit �Լ� ����
            Hit();
        }
    }

    void BossAction()
    {
        switch(_nNo)
        {
            case 1:
                {
                    _runningCo = StartCoroutine(IBoss1_Action());
                    break;
                }
        }
    }

    IEnumerator IBoss1_Action()
    {
        yield return new WaitForSeconds(1f);
        // Awake ��ȯ
        ChangeState(0);
        yield return new WaitForSeconds(0.5f);

        // 2������ ������ ����
        while (GameManager.i.pPlayer.gameObject.activeSelf)
        {
            if(_hp <= 1f)
            {
                if(_goUpBoss.activeSelf == false)
                {
                    _goUpBoss.SetActive(true);
                    
                    while(_goUpBoss.transform.localPosition.y <= _fUpY)
                    {
                        _goUpBoss.transform.Translate(Vector2.up * Time.deltaTime * 10f);

                        yield return null;
                    }

                    _rigid2D.bodyType = RigidbodyType2D.Kinematic;
                    _col2D.isTrigger = true;
                    ChangeState(1);
                }

                yield return new WaitForSeconds(3f);

                Attack();
            }
            else
            {
                // Attak ��ȯ
                ChangeState(2);

                yield return new WaitForSeconds(0.8f);

                // Attack �׼�
                Attack();

                yield return new WaitForSeconds(1f);

                // Idle ��ȯ
                ChangeState(1);

                yield return new WaitForSeconds(2f);
            }            
        }
    }

    void ChangeState(int no)
    {
        _state = no;
        _anim.SetInteger("State", _state);
    }

    void Show(bool show)
    {
        gameObject.SetActive(show);
    }

    void Attack()
    {
        switch(_hp)
        {
            case 3:
                {
                    // AŸ�� ����
                    if(_projFire != null)
                    {
                        _projFire.AllFire();
                    }
                    break;
                }
            case 2:
                {
                    // BŸ�� ����
                    if (_preAttack != null)
                    {
                        // ������ X�� ���� Ȯ��
                        int randCnt = Random.Range(_nMinCnt, _nMaxCnt);
                        int randDist = Random.Range(5, 8);
                        float alpha = 20f / (float)randCnt;

                        for (int i = 0; i < randCnt; i++)
                        {
                            float distX = -1 * (randDist + (i * alpha));
                            _obsAtks[i].gameObject.SetActive(true);
                            Root root = _obsAtks[i].GetComponent<Root>();
                            if (root != null)
                            {
                                root.SetPosX(distX);
                                root.Grow();
                            }
                        }
                    }
                    break;
                }
            case 1:
                {
                    // CŸ�� ����
                    foreach(GameObject go in _goUpTiles)
                    {
                        UpGround up = go.GetComponent<UpGround>();
                        up.DoEvent();
                    }
                    
                    break;
                }
        }
    }

    void Hit()
    {
        if (_hp > 0)
        {
            if (_hp == 1 && !_bLast)
            {
                _bLast = true;
            }

            // �浹 ����Ʈ �ߵ�
            if (_psHit != null)
            {
                // ��Ȱ��ȭ ����
                if (_psHit.gameObject.activeSelf == false)
                    _psHit.gameObject.SetActive(true);

                // ��ƼŬ �ý��� ����
                _psHit.Play();
            }
        }
        // ��� ó��
        else
        {
            // ��� ����Ʈ �ߵ�

            // ��� ����Ʈ ���� �� ������Ʈ ���� ó��
            gameObject.SetActive(false);
        }
    }

    void CreateAttackTypeB()
    {
        if (_preAttack != null && _nMaxCnt > _nMinCnt)
        {
            _obsAtks = new List<Obstacle>();

            for (int i = 0; i < _nMaxCnt; i++)
            {
                GameObject go = Instantiate(_preAttack);
                go.transform.SetParent(transform);
                go.SetActive(false);
                go.transform.localPosition = new Vector3(0f, -2.35f, 0f);
                Obstacle obs = go.GetComponent<Obstacle>();
                if (obs != null)
                {
                    obs.Init();
                    _obsAtks.Add(obs);
                }
            }
        }
    }
}
