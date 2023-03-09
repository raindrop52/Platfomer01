using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    [SerializeField] bool _bLeft;
    [SerializeField] float _length;
    [SerializeField] float _speed;
    Vector2 _pos;

    void Awake()
    {
        if (_bLeft)
            _pos = new Vector2(transform.position.x + _length, transform.position.y);
        else
            _pos = transform.position;
    }

    void Update()
    {
        float alpha = Mathf.PingPong(Time.time * _speed, _length);
        if(_bLeft)
            transform.position = new Vector2(_pos.x - alpha , _pos.y);
        else
            transform.position = new Vector2(_pos.x + alpha, _pos.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if(player != null)
            {
                if (!player.JumpState)
                    collision.transform.SetParent(transform);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player") && collision.gameObject.activeSelf)
            collision.transform.SetParent(null);
    }
}
