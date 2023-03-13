using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTrap : Trap
{
    [SerializeField] GameObject _newImg;    // �̹��� ǥ��
    [SerializeField] AudioSource _sfx;      // ����ü �߻� �Ҹ�
    [SerializeField] float _speed;
    [SerializeField] float _scale;
    bool _bShoot = false;  // �߻� ��Ÿ�� ����
    Spike _spike;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // �߻� �����ϸ�
        if (collision.CompareTag("Player") && !_bShoot)
        {
            StartCoroutine(Shoot());
        }
    }

    public override void Init()
    {
        base.Init();

        if (_prefabObs != null)
        {
            GameObject go = Instantiate(_prefabObs);
            if (_showTrans != null)
                go.transform.position = _showTrans.position;
            else
                go.transform.position = transform.position;
            if (_scale != 0)
                go.transform.localScale = new Vector2(_scale, _scale);

            go.transform.SetParent(TrapManager.i._transObstacle);

            Spike spike = go.GetComponent<Spike>();
            spike.Speed = _speed;
            spike.gameObject.SetActive(false);
            spike.Init();
            _spike = spike;
        }
    }

    public override void Restore()
    {
        base.Restore();

        _bShoot = false;
        if (_newImg != null && _newImg.activeSelf == true)
            _newImg.SetActive(false);
    }

    IEnumerator Shoot()
    {
        _bShoot = true;

        yield return new WaitForSeconds(1.0f);

        ShowImage();

        if (_spike != null)
        {
            _spike.Fire();
        }

        yield return new WaitForSeconds(1.0f);
        _bShoot = false;

        ShowImage();
    }

    void ShowImage()
    {
        if (_newImg != null)
        {
            if (_newImg.activeSelf == false)
                _newImg.SetActive(true);
            else
                _newImg.SetActive(false);
        }
    }
}
