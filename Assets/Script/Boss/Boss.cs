using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // 애니메이션
    Animator _anim;
    // 보스 번호
    [SerializeField] int _nNo;
    // 보스 히트박스 충돌체
    CapsuleCollider2D _hitCol;
    // 보스 타격 이펙트
    [SerializeField] ParticleSystem _psHit;
    // 보스 공격 오브젝트
    [Header("공격 오브젝트")]
    [SerializeField] GameObject _preAttack;
    [SerializeField] int _minCnt;
    [SerializeField] int _maxCnt;
    [SerializeField] List<Obstacle> _obsAtks;

    // 체력
    int _hp = 0;
    int _maxHp = 3;
    // 상태 ( -1 = Die / 0 = Awake / 1 = Idle / 2 = Attack )
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
        // Item과 충돌하면
        if (collision.CompareTag("Item"))
        {
            // 체력 감소
            _hp -= 1;

            // 충돌 이펙트 발동
            if (_psHit != null)
            {
                // 비활성화 해제
                if (_psHit.gameObject.activeSelf == false)
                    _psHit.gameObject.SetActive(true);

                // 파티클 시스템 동작   
                _psHit.Play();
            }

            // 충돌 오브젝트 파괴
            Destroy(collision.gameObject);
        }
    }

    public void Restore()
    {
        // state가 0보다 크면 Die 하지 않았기에 동작할 필요 X
        if (_state >= 0)
        {
            // 휴면 상태로 전환
            ChangeState(-1);
            // Show 상태로 전환
            Show(true);
        }
    }

    public void StartAction()
    {
        // 보스전 시작
        GameManager.i.BossOn = true;
        GameManager.i.BossNo = _nNo;

        // 보스 행동 시작
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
        // Awake 전환
        ChangeState(0);
        yield return new WaitForSeconds(0.5f);

        while (_state >= 0)
        {
            ChangeState(2);

            yield return null;

            // Attack 액션
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
