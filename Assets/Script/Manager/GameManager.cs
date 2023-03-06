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

    // Game Option [bool]
    bool _bStart = false;           // ���� ���� ����
    [SerializeField] bool _bBossOn = false;
    public bool BossOn
    {
        get { return _bBossOn; }
        set { _bBossOn = value; }
    }

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
        // RŰ ���� ��
        if (Input.GetKeyDown(KeyCode.R))
        {
            // ĳ���� ���̺� ����Ʈ ��Ȱ
            CreatePlayer();

            // Ʈ�� �Ŵ��� �ʱ�ȭ ����
            TrapManager.i.AllTrapRestore();
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
        if (_player != null)
        {
            Debug.Log("ERR : �÷��̾� ����");
            return;
        }

        GameObject go = Instantiate(_playerPrefab);
        go.transform.position = _vecSavePoint;
        _player = go.GetComponent<Player>();
        if (_player != null)
        {
            Debug.Log("�÷��̾� ���� ����");
        }
    }
    #endregion
}
