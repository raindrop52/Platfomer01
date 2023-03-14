using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // 애니메이션
    Animator _anim;
    // 보스 번호
    [SerializeField] int _nNo;
    // 보스 타격 충돌체 오브젝트
    [SerializeField] GameObject _goHitGem;
    // 동작 애니메이션 대기 변수
    [SerializeField] bool _bPause = false;
    // 보스 타격 이펙트
    [SerializeField] ParticleSystem _psHit;
    // 보스 공격 오브젝트
    [Header("공격 오브젝트")]
    [SerializeField] GameObject _preAttack;
    [SerializeField] int _nMinCnt;
    [SerializeField] int _nMaxCnt;
    [SerializeField] List<Obstacle> _obsAtks;

    // 체력
    [SerializeField] int _hp = 0;
    public int HP
    {
        get { return _hp; }
        set { _hp = value; }
    }
    int _oldHp = 3;
    // 상태 ( -1 = Die / 0 = Awake / 1 = Idle / 2 = Attack )
    int _state = -1;

    void Awake()
    {
        _anim = GetComponent<Animator>();

        CreateAttack();

        _hp = _oldHp;
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
        // 보스전 시작
        GameManager.i.BossOn = true;
        GameManager.i.BossNo = _nNo;

        // 보스 행동 시작
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

        // 한대 맞은 경우
        if(_hp < _oldHp)
        {
            // Hit 함수 동작
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
        // Awake 전환
        ChangeState(0);
        yield return new WaitForSeconds(0.5f);

        while (_state >= 0)
        {
            yield return new WaitUntil(() => _bPause == false);

            ChangeState(2);

            yield return new WaitForSeconds(0.8f);

            // Attack 액션
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
                    // A타입 어택

                    break;
                }
            case 2:
                {
                    // B타입 어택
                    if (_preAttack != null)
                    {
                        // 가시의 X축 범위 확인
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
            // 충돌 이펙트 발동
            if (_psHit != null)
            {
                // 비활성화 해제
                if (_psHit.gameObject.activeSelf == false)
                    _psHit.gameObject.SetActive(true);

                // 파티클 시스템 동작   
                _psHit.Play();
            }
        }
        // 사망 처리
        else
        {
            // 사망 이펙트 발동

            // 사망 이펙트 종료 후 오브젝트 숨김 처리
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
