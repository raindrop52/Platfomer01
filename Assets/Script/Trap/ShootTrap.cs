using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTrap : Trap
{
    [SerializeField] GameObject _newImg;    // �̹��� ǥ��
    [SerializeField] bool _bShoot = false;  // �߻� ��Ÿ�� ����
    [SerializeField] AudioSource _sfx;      // ����ü �߻� �Ҹ�

    public override void Restore()
    {
        base.Restore();

        _bShoot = false;
        if (_newImg.activeSelf == true)
            _newImg.SetActive(false);
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        // �߻� �����ϸ�
        if (collision.CompareTag("Player") && !_bShoot)
        {
            StartCoroutine(Shoot());
        }
    }

    IEnumerator Shoot()
    {
        _bShoot = true;

        yield return new WaitForSeconds(1.0f);

        ShowImage();

        if(_obstacle != null)
        {
            GameObject go = Instantiate(_obstacle);
            go.transform.position = _showTrans.position;
            Obstacle obs = go.GetComponent<Obstacle>();
            if (obs != null)
                obs.Init();
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
