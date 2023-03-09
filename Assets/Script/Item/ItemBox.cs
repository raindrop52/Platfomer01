using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBox : MonoBehaviour
{
    Transform _trans;
    [SerializeField] Item _item;
    [SerializeField] GameObject _bg;
    [SerializeField] GameObject _box;

    [SerializeField] float _speed;

    private void Awake()
    {
        _trans = _bg.transform.parent.transform;
        _trans.localScale = Vector3.zero;
        _trans.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어 충돌 시
        if(collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if(player != null && player._myItem == null)
            {
                // 아이템 획득
                if (_item != null)
                {
                    // 아이템 획득 동작
                    StartCoroutine(UnBoxing());
                    // UI에 아이템 표시

                    // 플레이어에 아이템 전달
                    player._myItem = _item;
                }
            }
        }
    }

    IEnumerator UnBoxing()
    {
        if (_bg != null && _box != null)
        {
            _trans.gameObject.SetActive(true);

            float scale = 0f;

            while(scale <= 1f)
            {
                scale += Time.deltaTime * _speed;

                _trans.localScale = new Vector3(scale, scale, scale);

                yield return null;
            }

            yield return new WaitForSeconds(0.5f);

            _trans.gameObject.SetActive(false);

            _trans.localScale = Vector3.zero;
        }

        yield return null;
    }
}
