using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager i;

    // Player
    [SerializeField] GameObject _playerPrefab;  // �÷��̾� ������
    Player _player;             // �÷��̾�
    public Player pPlayer
    {
        get { return _player; }
    }

    #region Game Option [bool]
    // ���� ���� ����
    bool _bStart = false;
    // ���� ���� ����
    [SerializeField] bool _bBossOn = false;
    public bool BossOn
    {
        get { return _bBossOn; }
        set { _bBossOn = value; }
    }
    // ������ ī�޶� ��ġ
    bool _bBossCamOn = false;
    [SerializeField] float _fCamOffsetX;
    // ���� ��ȣ
    int _nBossNo = -1;
    public int BossNo
    {
        get { return _nBossNo; }
        set { _nBossNo = value; }
    }
    // ���� ���� ����
    bool _bGameOver = false;
    public bool GameOver
    {
        get { return _bGameOver; }
        set { _bGameOver = value; }
    }
    #endregion
    // Save Point
    [SerializeField] Vector2 _vecSavePoint;      // ���̺� ����Ʈ ��ǥ

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
        // RŰ ���� �� (���� ���� ���¿�����)
        if (Input.GetKeyDown(KeyCode.R) && _bGameOver)
        {
            // ĳ���� ���̺� ����Ʈ ��Ȱ
            CreatePlayer();

            // Ʈ�� �Ŵ��� �ʱ�ȭ ����
            TrapManager.i.AllTrapRestore();

            // ���� �Ŵ��� �ʱ�ȭ ����
            if (_bBossOn)
                _bBossOn = false;
            BossManager.i.AllRestore();

            // ���ӿ��� �ʱ�ȭ
            _bGameOver = false;
        }

        // ������ ���� ��
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
            Debug.Log("ERR : ���̺� ����Ʈ ����");
            return;
        }

        _vecSavePoint = pos;
    }
    #endregion

    #region private
    void CreatePlayer()
    {
        // �÷��̾ ������
        if (_player == null)
        {
            // �÷��̾� ����
            GameObject go = Instantiate(_playerPrefab);
            go.transform.position = _vecSavePoint;
            _player = go.GetComponent<Player>();
            _player.Init();
        }
        
        // �÷��̾ ���� ��
        if (_player != null)
        {
            Debug.Log("�÷��̾� ���� ����");
            _player.gameObject.SetActive(true);
            _player.Resurrection(_vecSavePoint);
        }
    }
    #endregion
}
