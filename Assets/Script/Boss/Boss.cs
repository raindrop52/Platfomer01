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
    // ���� �ִϸ��̼� ��� ����
    [SerializeField] bool _bPause = false;
    // ���� Ÿ�� ����Ʈ
    [SerializeField] ParticleSystem _psHit;
    // ���� ���� ������Ʈ
    [Header("���� ������Ʈ")]
    [SerializeField] GameObject _preAttack;
    [SerializeField] int _nMinCnt;
    [SerializeField] int _nMaxCnt;
    [SerializeField] List<Obstacle> _obsAtks;

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

        CreateAttack();

        _hp = _oldHp;
    }

    public void Restore()
    {
        // state�� 0���� ũ�� Die ���� �ʾұ⿡ ������ �ʿ� X
        if (_state >= 0)
        {
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

        while (_state >= 0)
        {
            yield return new WaitUntil(() => _bPause == false);

            ChangeState(2);

            yield return new WaitForSeconds(0.8f);

            // Attack �׼�
            Attack();

            yield return new WaitForSeconds(0.5f);

            ChangeState(1);

            yield return new WaitForSeconds(2f);
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

                    break;
                }
        }
    }

    void Hit()
    {
        _oldHp = _hp;

        if (_hp > 0)
        {
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

    void CreateAttack()
    {
        
        // B
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
