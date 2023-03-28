using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    // 애니메이션
    Animator _anim;
    // 충돌체
    Rigidbody2D _rigid2D;
    BoxCollider2D _col2D;
    // 보스 번호
    [SerializeField] int _nNo;
    // 보스 코루틴
    Coroutine _runningCo = null;
    // 보스 타격 충돌체 오브젝트
    [SerializeField] GameObject _goHitGem;
    // 보스 사망 이펙트
    [SerializeField] ParticleSystem _psDie;
    // 보스 공격 오브젝트
    [Header("공격 A")]
    Projectil_Fire _projFire;
    [Header("공격 B")]
    [SerializeField] GameObject _preAttack;
    [SerializeField] int _nMinCnt;
    [SerializeField] int _nMaxCnt;
    [SerializeField] List<Obstacle> _obsAtks;
    [Header("공격 C")]
    [SerializeField] bool _bLast = false;
    [SerializeField] float _fUpY = 8f;       // _goUpBoss의 높이
    [SerializeField] GameObject _goUpBoss;
    [SerializeField] List<GameObject> _goUpTiles;

    // 체력
    [SerializeField] int _hp = 0;
    public int HP
    {
        get { return _hp; }
        set { _hp = value; }
    }
    int _oldHp = 0;
    int _maxHp = 3;
    // 상태 ( -1 = Die / 0 = Awake / 1 = Idle / 2 = Attack )
    int _state = -1;

    void Awake()
    {
        _anim = GetComponent<Animator>();
        _rigid2D = GetComponent<Rigidbody2D>();
        _col2D = GetComponent<BoxCollider2D>();
        _projFire = GetComponent<Projectil_Fire>();

        CreateAttackTypeB();

        Init();
    }

    [SerializeField] bool _bTest = false;
    void Update()
    {
        // 패턴 동작 테스트용 코드
        if (_bTest)
        {
            _bTest = false;
            StartAction();
        }

        // 한대 맞은 경우
        if (_hp < _oldHp)
        {
            // Hit 함수 동작
            Hit();
        }
    }

    void Init()
    {
        if(gameObject.activeSelf == false)
            gameObject.SetActive(true);

        _hp = _oldHp = _maxHp;

        // 최종 페이즈 변수 초기화
        _bLast = false;
        _col2D.isTrigger = false;

        if (_goUpBoss != null)
        {
            _goUpBoss.transform.position = new Vector3(0f, -1f, 0f);
            _goUpBoss.SetActive(false);
        }

        if (_rigid2D.bodyType != RigidbodyType2D.Dynamic)
            _rigid2D.bodyType = RigidbodyType2D.Dynamic;

        EventAnimation_ShowGem(0);

        // 휴면 상태로 전환
        ChangeState(-1);

        // Show 상태로 전환
        Show(true);
    }

    public void Restore()
    {
        // state가 0보다 크면 Die 하지 않았기에 동작할 필요 X
        if (_state >= 0)
        {
            // 코루틴 동작 정지
            if(_runningCo != null)
                StopCoroutine(_runningCo);

            Init();
        }
    }

    // 0 = Hide | 1 = Show
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
        // Awake 전환
        ChangeState(0);
        yield return new WaitForSeconds(0.5f);

        // 2페이즈 까지의 동작
        while (GameManager.i.pPlayer.gameObject.activeSelf)
        {
            if(_hp <= 1f)
            {
                if(_goUpBoss.activeSelf == false && !_bLast)
                {
                    _goUpBoss.SetActive(true);

                    GameManager.i.pPlayer.GodMove(Vector2.left);

                    while (_goUpBoss.transform.localPosition.y <= _fUpY)
                    {
                        _goUpBoss.transform.Translate(Vector2.up * Time.deltaTime * 10f);

                        yield return null;
                    }

                    GameManager.i.pPlayer.GodStop();

                    _rigid2D.bodyType = RigidbodyType2D.Kinematic;
                    _col2D.isTrigger = true;
                    ChangeState(1);

                    EventAnimation_ShowGem(1);
                    _bLast = true;
                }

                yield return new WaitForSeconds(3f);

                Attack();
            }
            else
            {
                // Attak 전환
                ChangeState(2);

                yield return new WaitForSeconds(0.8f);

                // Attack 액션
                Attack();

                yield return new WaitForSeconds(1f);

                // Idle 전환
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
                    // A타입 어택
                    if(_projFire != null)
                    {
                        _projFire.AllFire();
                    }
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
                    if(_bLast)
                    {
                        // C타입 어택
                        foreach (GameObject go in _goUpTiles)
                        {
                            UpGround up = go.GetComponent<UpGround>();
                            up.DoEvent();
                        }
                    }
                    
                    break;
                }
        }
    }

    void Hit()
    {
        if (_hp > 0)
        {
            // 이전 체력 기록
            _oldHp = _hp;

            // 젬 숨김
            EventAnimation_ShowGem(0);

            // 히트 애니메이션 동작
            _anim.SetTrigger("Hit");
        }
        // 사망 처리
        else
        {
            // 사망 이펙트 발동
            _psDie.gameObject.SetActive(true);
            _psDie.Play();
            // 사망 이펙트 종료 후 오브젝트 숨김 처리
            gameObject.SetActive(false);

            GameManager.i.BossOn = false;
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
