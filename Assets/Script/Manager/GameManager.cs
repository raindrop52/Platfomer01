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

    // Game Option [bool]
    bool _bStart = false;           // 게임 시작 여부
    [SerializeField] bool _bBossOn = false;
    public bool BossOn
    {
        get { return _bBossOn; }
        set { _bBossOn = value; }
    }

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

        if (!_bStart)
        {
            _bStart = true;

            if (_player == null)
            {
                CreatePlayer();
            }
        }
    }

    void Update()
    {
        // R키 누를 시
        if (Input.GetKeyDown(KeyCode.R))
        {
            // 캐릭터 세이브 포인트 부활
            CreatePlayer();

            // 트랩 매니저 초기화 동작
            TrapManager.i.AllTrapRestore();
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
        if (_player != null)
        {
            Debug.Log("ERR : 플레이어 존재");
            return;
        }

        GameObject go = Instantiate(_playerPrefab);
        go.transform.position = _vecSavePoint;
        _player = go.GetComponent<Player>();
        if (_player != null)
        {
            Debug.Log("플레이어 정상 제작");
        }
    }
    #endregion
}
