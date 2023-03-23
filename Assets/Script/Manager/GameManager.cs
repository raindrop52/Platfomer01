using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager i;

    int _nSceneNo = 0;

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
    Boss _boss;
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

        if (UIManager.i != null)
            UIManager.i.Init();

        if (_boss == null)
            _boss = GameObject.FindWithTag("Boss").GetComponent<Boss>();

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

            // UI �Ŵ��� �ʱ�ȭ
            UIManager.i.Restore();

            // ���� �ʱ�ȭ ����
            if (_bBossOn)
                _bBossOn = false;

            if (_boss != null)
                _boss.Restore();

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

    public void GoNextScene()
    {
        _nSceneNo++;
        StartCoroutine(ILoadAsyncScene());
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

    IEnumerator ILoadAsyncScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_nSceneNo);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
