using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // �ִϸ��̼�
    Animator _anim;
    // ���� ��ȣ
    [SerializeField] int _nNo;
    // ���� ��Ʈ�ڽ� �浹ü
    CapsuleCollider2D _hitCol;
    // ���� Ÿ�� ����Ʈ
    [SerializeField] ParticleSystem _psHit;
    // ���� ���� ������Ʈ
    [Header("���� ������Ʈ")]
    [SerializeField] GameObject _preAttack;
    [SerializeField] int _minCnt;
    [SerializeField] int _maxCnt;
    [SerializeField] List<Obstacle> _obsAtks;

    // ü��
    int _hp = 0;
    int _maxHp = 3;
    // ���� ( -1 = Die / 0 = Awake / 1 = Idle / 2 = Attack )
    int _state = -1;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _hitCol = GetComponent<CapsuleCollider2D>();

        if(_preAttack != null && _maxCnt > _minCnt)
        {
            _obsAtks = new List<Obstacle>();

            for (int i = 0; i < _maxCnt; i++)
            {
                GameObject go = Instantiate(_preAttack);
                go.transform.SetParent(transform);
                go.SetActive(false);
                go.transform.localPosition = new Vector3(0f, -4f, 0f);
                Obstacle obs = go.GetComponent<Obstacle>();
                if (obs != null)
                {
                    _obsAtks.Add(obs);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Item�� �浹�ϸ�
        if (collision.CompareTag("Item"))
        {
            // ü�� ����
            _hp -= 1;

            // �浹 ����Ʈ �ߵ�
            if (_psHit != null)
            {
                // ��Ȱ��ȭ ����
                if (_psHit.gameObject.activeSelf == false)
                    _psHit.gameObject.SetActive(true);

                // ��ƼŬ �ý��� ����   
                _psHit.Play();
            }

            // �浹 ������Ʈ �ı�
            Destroy(collision.gameObject);
        }
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

    public void StartAction()
    {
        // ������ ����
        GameManager.i.BossOn = true;
        GameManager.i.BossNo = _nNo;

        // ���� �ൿ ����
        BossAction();
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
            ChangeState(2);

            yield return null;

            // Attack �׼�
            Attack();

            yield return null;

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
        if (_preAttack != null)
        {
            int randCnt = Random.Range(_minCnt, _maxCnt);
            List<float> listDist = new List<float>();
            int randDist = Random.Range(5, 8);
            float alpha = 20f / randCnt;
            for (int i = 0; i < randCnt; i++)
            {
                listDist.Add(randDist + (i * alpha));                
                _obsAtks[i].gameObject.SetActive(true);
            }
        }
    }
}
