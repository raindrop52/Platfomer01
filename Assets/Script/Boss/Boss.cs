using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // �ִϸ��̼�
    Animator _anim;
    // ���� ��ȣ
    [SerializeField] int _nNo;
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
    int _oldHp = 3;
    // ���� ( -1 = Die / 0 = Awake / 1 = Idle / 2 = Attack )
    int _state = -1;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _projFire = GetComponent<Projectil_Fire>();

        CreateAttackTypeB();

        _hp = _oldHp;
        _bLast = false;
        if (_goUpBoss != null)
            _goUpBoss.SetActive(false);
    }

    public void Restore()
    {
        // state�� 0���� ũ�� Die ���� �ʾұ⿡ ������ �ʿ� X
        if (_state >= 0)
        {
            // �������� �ʱ�ȭ
            _bLast = false;
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
        if(_hp < _oldHp)
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
                    StartCoroutine(IBoss1_Action());
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

        // 2�� Hit������ �ִϸ��̼� ����
        while (_state >= 0)
        {
            if(_hp <= 1f)
            {
                ChangeState(1);
                if(_goUpBoss.activeSelf == false)
                {
                    _goUpBoss.SetActive(true);
                    
                    while(_goUpBoss.transform.localPosition.y <= _fUpY)
                    {
                        _goUpBoss.transform.Translate(Vector2.up * Time.deltaTime * 10f);

                        yield return null;
                    }
                }
                break;
            }

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

        while(_hp > 0f)
        {
            yield return new WaitForSeconds(3f);

            Attack();
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
                    if (_goUpTiles.Count > 0)
                    {

                    }
                    break;
                }
        }
    }

    void Hit()
    {
        _oldHp = _hp;

        if (_hp > 0)
        {
            if (_hp == 1)
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
                go.transform.localPosition = new Vector3(0f, -3.57f, 0f);
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
