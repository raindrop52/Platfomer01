using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager i;

    // Player
    [SerializeField] GameObject _playerPrefab;  // 플레이어 프리팹
    Player _player;             // 플레이어
    public Player pPlayer
    {
        get { return _player; }
    }

    #region Game Option [bool]
    // 게임 시작 여부
    bool _bStart = false;
    // 보스 진입 여부
    [SerializeField] bool _bBossOn = false;
    public bool BossOn
    {
        get { return _bBossOn; }
        set { _bBossOn = value; }
    }
    // 보스룸 카메라 위치
    bool _bBossCamOn = false;
    [SerializeField] float _fCamOffsetX;
    // 보스 번호
    int _nBossNo = -1;
    public int BossNo
    {
        get { return _nBossNo; }
        set { _nBossNo = value; }
    }
    // 게임 오버 여부
    bool _bGameOver = false;
    public bool GameOver
    {
        get { return _bGameOver; }
        set { _bGameOver = value; }
    }
    #endregion
    // Save Point
    [SerializeField] Vector2 _vecSavePoint;      // 세이브 포인트 좌표

    private void Awake()
    {
        i = this;
    }

    void Start()
    {
        if (TrapManager.i != null)
            TrapManager.i.Init();

        if (BossManager.i != null)
            BossManager.i.Init();

        if (!_bStart)
        {
            _bStart = true;

            CreatePlayer();
        }
    }

    void Update()
    {
        // R키 누를 시 (게임 오버 상태에서만)
        if (Input.GetKeyDown(KeyCode.R) && _bGameOver)
        {
            // 캐릭터 세이브 포인트 부활
            CreatePlayer();

            // 트랩 매니저 초기화 동작
            TrapManager.i.AllTrapRestore();

            // 보스 매니저 초기화 동작
            if (_bBossOn)
                _bBossOn = false;
            BossManager.i.AllRestore();

            // 게임오버 초기화
            _bGameOver = false;
        }

        // 보스전 시작 시
        if (_bBossOn && !_bBossCamOn)
        {
            _bBossCamOn = true;
            Vector3 camPos = Camera.main.transform.position;
            Camera.main.transform.position = new Vector3(camPos.x + _fCamOffsetX, camPos.y, camPos.z);
        }
        else if (!_bBossOn && _bBossCamOn)
        {
            _bBossCamOn = false;
        }
    }

    #region public
    public void TouchSavePoint(Vector2 pos)
    {
        if (pos.Equals(_vecSavePoint))
        {
            Debug.Log("ERR : 세이브 포인트 동일");
            return;
        }

        _vecSavePoint = pos;
    }
    #endregion

    #region private
    void CreatePlayer()
    {
        // 플레이어가 없으면
        if (_player == null)
        {
            // 플레이어 제작
            GameObject go = Instantiate(_playerPrefab);
            go.transform.position = _vecSavePoint;
            _player = go.GetComponent<Player>();
            _player.Init();
        }
        
        // 플레이어가 존재 시
        if (_player != null)
        {
            Debug.Log("플레이어 정상 제작");
            _player.gameObject.SetActive(true);
            _player.Resurrection(_vecSavePoint);
        }
    }
    #endregion
}
