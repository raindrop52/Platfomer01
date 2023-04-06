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

    [SerializeField] Tween_Item _tweenItem;

    private void Awake()
    {
        _trans = _bg.transform.parent.transform;
        _trans.localScale = Vector3.zero;
        _trans.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �÷��̾� �浹 ��
        if(collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if(player != null && player._goMyItem == null)
            {
                // ������ ȹ��
                if (_item != null)
                {
                    // ������ ȹ�� ����
                    StartCoroutine(UnBoxing());
                    // ������ ������ ����
                    GameObject goItem = Instantiate(_item.gameObject);
                    goItem.transform.SetParent(player.transform);
                    goItem.transform.localPosition = Vector3.zero;
                    Item item = goItem.GetComponent<Item>();
                    // UI Tween ����
                    if (_tweenItem != null)
                    {
                        // UI�� ������ ǥ��
                        if (item != null)
                            _tweenItem._item = item;

                        _tweenItem.Move();
                    }

                    // �÷��̾ ������ ����
                    player.Play_Sound((int)Player_Sound.PICK);
                    player._goMyItem = goItem;
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
